﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace MadsKristensen.EditorExtensions
{
    public class JsHintCompiler : NodeExecutorBase
    {
        private static readonly string _compilerPath = Path.Combine(WebEssentialsResourceDirectory, @"nodejs\node_modules\jshint\bin\jshint");
        // JsHint Reported is located in Resources\Scripts\ directory. Read more at http://www.jshint.com/docs/reporters/
        private static readonly string _jsHintReporter = Path.Combine(WebEssentialsResourceDirectory, @"Scripts\jshint-node-reporter.js");
        private static readonly Regex _errorParsingPattern = new Regex(@"^/s", RegexOptions.Multiline);

        protected override string ServiceName
        {
            get { return "JsHint"; }
        }
        protected override string CompilerPath
        {
            get { return _compilerPath; }
        }
        protected override Regex ErrorParsingPattern
        {
            get { return _errorParsingPattern; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration"
                       , MessageId = "1#"
                       , Justification = "Read more on this feature request on Connect https://connect.microsoft.com/VisualStudio/feedback/details/812838/parameter-name-aliases-in-c")]
        protected override string GetArguments(string sourceFileName, string configurationFileName)
        {
            return String.Format(CultureInfo.CurrentCulture, "--config \"{0}\" --reporter \"{1}\" \"{2}\""
                               , configurationFileName
                               , _jsHintReporter
                               , sourceFileName);
        }

        protected override string PostProcessResult(string resultSource, string sourceFileName, string targetFileName)
        {
            Logger.Log("JSHint: " + Path.GetFileName(sourceFileName) + " executed.");

            return resultSource;
        }
    }
}