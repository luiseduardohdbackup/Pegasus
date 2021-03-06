﻿// -----------------------------------------------------------------------
// <copyright file="ReportResourcesMissingPass.cs" company="(none)">
//   Copyright © 2014 John Gietzen.  All Rights Reserved.
//   This source is subject to the MIT license.
//   Please see license.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Pegasus.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using Pegasus.Expressions;
    using Pegasus.Properties;

    internal class ReportResourcesMissingPass : CompilePass
    {
        public override IList<string> BlockedByErrors
        {
            get { return new[] { "PEG0001" }; }
        }

        public override IList<string> ErrorsProduced
        {
            get { return new[] { "PEG0016" }; }
        }

        public override void Run(Grammar grammar, CompileResult result)
        {
            new MissingRuleExpressionTreeWalker(result).WalkGrammar(grammar);
        }

        private class MissingRuleExpressionTreeWalker : ExpressionTreeWalker
        {
            private readonly CompileResult result;

            public MissingRuleExpressionTreeWalker(CompileResult result)
            {
                this.result = result;
            }

            public override void WalkGrammar(Grammar grammar)
            {
                if (!grammar.Settings.Any(s => s.Key.Name == "resources"))
                {
                    base.WalkGrammar(grammar);
                }
            }

            protected override void WalkLiteralExpression(LiteralExpression literalExpression)
            {
                if (literalExpression.FromResource)
                {
                    this.result.AddCompilerError(literalExpression.Start, () => Resources.PEG0016_ERROR_ResourcesNotSpecified);
                }
            }
        }
    }
}
