using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

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
