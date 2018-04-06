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
using System.Collections.Generic;
using System.IO;
using System.Text;

using FluentMigrator.BatchParser;
using FluentMigrator.BatchParser.RangeSearchers;
using FluentMigrator.BatchParser.Sources;
using FluentMigrator.BatchParser.SpecialTokenSearchers;

using McMaster.Extensions.CommandLineUtils;

namespace TestSqlServerBatchParser
{
    internal static class Program
    {
        private static StringBuilder _sqlText;
        private static string _sqlStatement;
        private static bool _outputEverySqlStatement;

        static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.HelpOption();

            var scriptFileName = app.Argument<string>("script", "SQL script file name")
                                    .IsRequired();
            var stripComments = app.Option("--strip", "Strip comments", CommandOptionType.NoValue);
            var singleStatement = app.Option("-s|--single", "Output single statements", CommandOptionType.NoValue);

            app.OnExecute(
                () =>
                {
                    // The default range searchers
                    var rangeSearchers = new List<IRangeSearcher>
                    {
                        new MultiLineComment(),
                        new DoubleDashSingleLineComment(),
                        new PoundSignSingleLineComment(),
                        new SqlString(),
                    };

                    // The special token searchers
                    var specialTokenSearchers = new List<ISpecialTokenSearcher>();

                    // Add SQL server specific range searcher
                    rangeSearchers.Add(new SqlServerIdentifier());

                    // Add SQL server specific token searcher
                    specialTokenSearchers.Add(new GoSearcher());

                    // We want every single SQL statement
                    _outputEverySqlStatement = singleStatement.HasValue();
                    if (_outputEverySqlStatement)
                    {
                        specialTokenSearchers.Add(new SemicolonSearcher());
                    }

                    var batchParser = new SqlBatchParser(rangeSearchers, specialTokenSearchers);
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
            var content = sqlTextEventArgs.SqlText.Trim();
            if (_outputEverySqlStatement)
            {
                if (_sqlText == null)
                    _sqlText = new StringBuilder();
                if (!string.IsNullOrEmpty(content))
                {
                    _sqlText.Append(content).Append(';').AppendLine();
                    _sqlStatement = content;
                }
            }
            else
            {
                _sqlText = new StringBuilder(sqlTextEventArgs.SqlText.Trim());
            }
        }

        private static void BatchParserOnSpecialToken(object sender, SpecialTokenEventArgs specialTokenEventArgs)
        {
            if (specialTokenEventArgs.Opaque is GoSearcher.GoSearcherParameters goParameters)
            {
                RunSql(goParameters.Count);
                _sqlText = null;
            }
            else if (!string.IsNullOrEmpty(_sqlStatement))
            {
                Console.Out.WriteLine("Statement: {0};", _sqlStatement);
                _sqlStatement = null;
            }
        }

        private static void RunSql(int count = 1)
        {
            if ((_sqlText?.Length ?? 0) == 0)
                return;

            Console.Out.WriteLine("Executing batch:");
            for (var i = 0; i != count; ++i)
            {
                Console.WriteLine(_sqlText);
            }
        }
    }
}
