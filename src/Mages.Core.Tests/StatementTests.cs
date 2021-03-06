﻿namespace Mages.Core.Tests
{
    using Mages.Core.Ast;
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using Mages.Core.Ast.Walkers;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class StatementTests
    {
        [Test]
        public void ParseTwoAssignmentStatements()
        {
            var source = "d = 5; a = b + c * d";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(2, statements.Count);
            Assert.IsInstanceOf<SimpleStatement>(statements[0]);
            Assert.IsInstanceOf<SimpleStatement>(statements[1]);

            var assignment1 = (statements[0] as SimpleStatement).Expression as AssignmentExpression;
            var assignment2 = (statements[1] as SimpleStatement).Expression as AssignmentExpression;

            Assert.IsNotNull(assignment1);
            Assert.IsNotNull(assignment2);

            Assert.AreEqual("d", assignment1.VariableName);
            Assert.AreEqual("a", assignment2.VariableName);
            Assert.AreEqual(5.0, (Double)((ConstantExpression)assignment1.Value).Value);
            Assert.IsInstanceOf<BinaryExpression.Add>(assignment2.Value);
        }

        [Test]
        public void ParseOneReturnStatementWithEmptyPayload()
        {
            var source = "return";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<ReturnStatement>(statements[0]);

            var return1 = (statements[0] as ReturnStatement).Expression as EmptyExpression;

            Assert.IsNotNull(return1);
        }

        [Test]
        public void ParseOneReturnStatementWithConstantPayload()
        {
            var source = "return 5";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<ReturnStatement>(statements[0]);

            var return1 = (statements[0] as ReturnStatement).Expression as ConstantExpression;

            Assert.IsNotNull(return1);
        }

        [Test]
        public void ParseOneBreakStatementWithoutPayload()
        {
            var source = "break";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<BreakStatement>(statements[0]);

            var errors = Validate(statements);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ErrorCode.LoopMissing, errors[0].Code);
        }

        [Test]
        public void ParseOneBreakStatementWithPayloadShouldContainErrors()
        {
            var source = "break true";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<BreakStatement>(statements[0]);

            var errors = Validate(statements);

            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual(ErrorCode.LoopMissing, errors[0].Code);
            Assert.AreEqual(ErrorCode.TerminatorExpected, errors[1].Code);
        }

        [Test]
        public void ParseOneContinueStatementWithoutPayload()
        {
            var source = "continue";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<ContinueStatement>(statements[0]);

            var errors = Validate(statements);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual(ErrorCode.LoopMissing, errors[0].Code);
        }

        [Test]
        public void ParseOneContinueStatementWithPayloadShouldContainErrors()
        {
            var source = "continue 2+3";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<ContinueStatement>(statements[0]);

            var errors = Validate(statements);

            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual(ErrorCode.LoopMissing, errors[0].Code);
            Assert.AreEqual(ErrorCode.TerminatorExpected, errors[1].Code);
        }

        [Test]
        public void ParseEmptyIfStatementShouldBeFine()
        {
            var source = "if () {}";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<IfStatement>(statements[0]);

            var errors = Validate(statements);

            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void ParseIfStatementWithNoStatementsShouldBeFine()
        {
            var source = "if (true) {}";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<IfStatement>(statements[0]);
            Assert.IsInstanceOf<BlockStatement>(((IfStatement)statements[0]).Primary);

            var errors = Validate(statements);

            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void ParseIfStatementWithSingleStatementsShouldBeFine()
        {
            var source = "if (true) n = 2 + 3";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<IfStatement>(statements[0]);
            Assert.IsInstanceOf<SimpleStatement>(((IfStatement)statements[0]).Primary);

            var errors = Validate(statements);

            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void ParseIfStatementWithMissingTailShouldYieldError()
        {
            var source = "if (true) { n = 2 + 3";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<IfStatement>(statements[0]);
            Assert.IsInstanceOf<BlockStatement>(((IfStatement)statements[0]).Primary);

            var errors = Validate(statements);

            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void ParseIfStatementWithMissingClosingScopeShouldYieldError()
        {
            var source = "if (true) { n = 2 + 3;";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<IfStatement>(statements[0]);
            Assert.IsInstanceOf<BlockStatement>(((IfStatement)statements[0]).Primary);

            var errors = Validate(statements);

            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void ParseIfStatementWithComposedConditionAndSingleStatementInBlockShouldBeFine()
        {
            var source = "if (a + b + c == d / 2) { n = k; }";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<IfStatement>(statements[0]);
            Assert.IsInstanceOf<BlockStatement>(((IfStatement)statements[0]).Primary);

            var errors = Validate(statements);

            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void ParseWhileStatementWithMissingCloseCurlyBracketShouldYieldError()
        {
            var source = "while (true) {";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<WhileStatement>(statements[0]);
            Assert.IsInstanceOf<BlockStatement>(((WhileStatement)statements[0]).Body);

            var errors = Validate(statements);

            Assert.AreEqual(1, errors.Count);
        }

        [Test]
        public void ParseWhileStatementWithEmptyStatementShouldBeFine()
        {
            var source = "while (true);";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<WhileStatement>(statements[0]);
            Assert.IsInstanceOf<SimpleStatement>(((WhileStatement)statements[0]).Body);

            var errors = Validate(statements);

            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void ParseTwoStatementWhereReturnHasBinaryPayload()
        {
            var source = "var x = 9; return x + pi";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(2, statements.Count);
            Assert.IsInstanceOf<VarStatement>(statements[0]);
            Assert.IsInstanceOf<ReturnStatement>(statements[1]);

            var assignment1 = (statements[0] as VarStatement).Assignment as AssignmentExpression;
            var return1 = (statements[1] as ReturnStatement).Expression as BinaryExpression.Add;

            Assert.IsNotNull(assignment1);
            Assert.IsNotNull(return1);

            Assert.AreEqual("x", assignment1.VariableName);
        }

        [Test]
        public void ParseNestedWhileLoopsShouldSeeNestedBlocks()
        {
            var source = "while (i < 5) { n++; while (i < 4) { i++; } i++; }";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<WhileStatement>(statements[0]);
            Assert.IsInstanceOf<BlockStatement>(((WhileStatement)statements[0]).Body);

            var errors = Validate(statements);

            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void IfBlockWithEndedElseShouldBeAnError()
        {
            var source = "if () { } else";
            var parser = new ExpressionParser();
            var statements = parser.ParseStatements(source);

            Assert.AreEqual(1, statements.Count);
            Assert.IsInstanceOf<IfStatement>(statements[0]);
            Assert.IsInstanceOf<BlockStatement>(((IfStatement)statements[0]).Primary);

            var errors = Validate(statements);

            Assert.AreEqual(1, errors.Count);
        }

        private static List<ParseError> Validate(List<IStatement> statements)
        {
            var errors = new List<ParseError>();
            var validator = new ValidationTreeWalker(errors);

            statements.ToBlock().Accept(validator);

            return errors;
        }
    }
}
