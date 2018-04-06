#region License
// Copyright (c) 2018, Fluent Migrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.IO;
using System.Text.RegularExpressions;

using FluentMigrator.BatchParser;
using FluentMigrator.BatchParser.Sources;

using McMaster.Extensions.CommandLineUtils;

namespace TestSqlServerBatchParser
{
    class Program
    {
        private static readonly Regex _regex = new Regex(@"^\s*(?<statement>GO(\s+(?<count>\d)+)?)\s*$", RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static string _sqlText;

        static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.HelpOption();

            var scriptFileName = app.Argument<string>("script", "SQL script file name")
                                    .IsRequired();
            var stripComments = app.Option("-s|--strip", "Strip comments", CommandOptionType.NoValue);

            app.OnExecute(
                () =>
                {
                    var batchParser = new SqlServerBatchParser();
                    batchParser.SpecialToken += BatchParserOnSpecialToken;
                    batchParser.SqlText += BatchParserOnSqlText;

                    using (var source = new TextReaderSource(new StreamReader(scriptFileName.Value), true))
                    {
                        batchParser.Process(source, stripComments.HasValue());
                    }

                    RunSql();

                    return 0;
                });

            return app.Execute(args);
        }

        private static void BatchParserOnSqlText(object sender, SqlTextEventArgs sqlTextEventArgs)
        {
            _sqlText = sqlTextEventArgs.SqlText.Trim();
        }

        private static void BatchParserOnSpecialToken(object sender, SpecialTokenEventArgs specialTokenEventArgs)
        {
            var match = _regex.Match(specialTokenEventArgs.Token);
            if (!match.Success)
                throw new InvalidOperationException("Unknown special token");

            var countGroup = match.Groups["count"];
            var count = countGroup.Success && countGroup.Length != 0 ? Convert.ToInt32(countGroup.Value, 10) : 1;

            RunSql(count);

            _sqlText = null;
        }

        private static void RunSql(int count = 1)
        {
            if (string.IsNullOrEmpty(_sqlText))
                return;

            for (var i = 0; i != count; ++i)
            {
                Console.WriteLine(_sqlText);
            }
        }
    }
}
