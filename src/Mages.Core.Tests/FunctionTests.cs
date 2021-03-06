﻿namespace Mages.Core.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class FunctionTests
    {
        [Test]
        public void LogicalFunctionsShouldYieldNumericMatrix()
        {
            var result = "isprime([3,4;5,7])".Eval();
            CollectionAssert.AreEquivalent(new Double[,] { { 1.0, 0.0 }, { 1.0, 1.0 } }, (Double[,])result);
        }

        [Test]
        public void LogicalFunctionsShouldYieldBooleanValue()
        {
            var result = "isint(9)".Eval();
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TrigonometricFunctionsShouldYieldNumericVector()
        {
            var result = "sin([0, pi / 4, pi / 2])".Eval();
            CollectionAssert.AreEquivalent(new Double[,] { { 0.0, Math.Sin(Math.PI / 4.0), Math.Sin(Math.PI / 2.0) } }, (Double[,])result);
        }

        [Test]
        public void TrigonometricFunctionsShouldYieldNumericValue()
        {
            var result = "cos(1)".Eval();
            Assert.AreEqual(Math.Cos(1.0), result);
        }

        [Test]
        public void ComparisonFunctionsShouldYieldNumericValue()
        {
            var result = "min(1)".Eval();
            Assert.AreEqual(1.0, result);
        }

        [Test]
        public void ComparisonFunctionsShouldReduceRowVectorToNumericValue()
        {
            var result = "max([1,2,30,4,5])".Eval();
            Assert.AreEqual(30.0, result);
        }

        [Test]
        public void ComparisonFunctionsShouldReduceColumnVectorToNumericValue()
        {
            var result = "min([1;2;3;-4;5])".Eval();
            Assert.AreEqual(-4.0, result);
        }

        [Test]
        public void ComparisonFunctionsShouldReduceMatrixToColumnVector()
        {
            var result = "min([1,2,3;3,4,5])".Eval();
            CollectionAssert.AreEquivalent(new Double[,] { { 1.0 }, { 3.0 } }, (Double[,])result);
        }

        [Test]
        public void ComparisonFunctionsOfEmptyMatrixShouldBeAnEmptyMatrix()
        {
            var result = "sort([])".Eval();
            CollectionAssert.AreEquivalent(new Double[,] { }, (Double[,])result);
        }

        [Test]
        public void CallAnUnknownFunctionShouldResultInNull()
        {
            var result = "footemp()".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CreateMagesFunctionShouldBeClassicallyCallableWithRightTypes()
        {
            var foo = "(x, y) => x * y + y".Eval() as Function;
            var result = foo.Invoke(new Object[] { 2.0, 3.0 });
            Assert.AreEqual(9.0, result);
        }

        [Test]
        public void CreateMagesFunctionShouldNotBeClassicallyCallableWithoutRightTypes()
        {
            var foo = "(x, y) => x * y + y".Eval() as Function;
            var result = foo.Invoke(new Object[] { 2, 3 });
            Assert.IsNull(result);
        }

        [Test]
        public void CreateMagesFunctionShouldBeDirectlyCallableWithRightReturnType()
        {
            var foo = "(x, y) => x * y + y".Eval() as Function;
            var result = foo.Call<Double>(2, 3);
            Assert.AreEqual(9.0, result);
        }

        [Test]
        public void CreateMagesFunctionShouldBeDirectlyCallableWithWrongReturnType()
        {
            var foo = "(x, y) => x * y + y".Eval() as Function;
            var result = foo.Call<Boolean>(2, 3);
            Assert.AreEqual(default(Boolean), result);
        }

        [Test]
        public void CreateMagesFunctionShouldBeDirectlyCallableWithoutType()
        {
            var foo = "(x, y) => x * y + y".Eval() as Function;
            var result = foo.Call(2, 3);
            Assert.AreEqual(9.0, result);
        }

        [Test]
        public void CallStringWithValidIndexYieldsStringWithSingleCharacter()
        {
            var result = "\"test\"(2)".Eval();
            Assert.AreEqual("s", result);
        }

        [Test]
        public void CallStringWithIndexOutOfRangeYieldsNothing()
        {
            var result = "\"test\"(4)".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallStringWithInvalidIndexYieldsNothing()
        {
            var result = "\"test\"(\"1\")".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallStringWithNegativeIndexYieldsNothing()
        {
            var result = "\"test\"(-1)".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallObjectWithValidNameYieldsValue()
        {
            var result = "new { a: 29 }(\"a\")".Eval();
            Assert.AreEqual(29.0, result);
        }

        [Test]
        public void CallObjectWithUnknownNameYieldsNothing()
        {
            var result = "new { a: 29 }(\"b\")".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallObjectWithWithNonStringYieldsValue()
        {
            var result = "new { \"2\": 29 }(2)".Eval();
            Assert.AreEqual(29.0, result);
        }

        [Test]
        public void CallEmptyObjectWithUnknownNameYieldsNothing()
        {
            var result = "new { }(\"Test\")".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithSingleIntegerArgumentYieldsValue()
        {
            var result = "[1,2,3;4,5,6](4)".Eval();
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void CallMatrixWithTwoIntegerArgumentsYieldsValue()
        {
            var result = "[1,2,3;4,5,6](1,1)".Eval();
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void CallMatrixWithSingleOutOfBoundsArgumentYieldsNothing()
        {
            var result = "[1,2,3;4,5,6](9)".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithSecondOutOfBoundsArgumentYieldsNothing()
        {
            var result = "[1,2,3;4,5,6](1,3)".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithFirstOutOfBoundsArgumentYieldsNothing()
        {
            var result = "[1,2,3;4,5,6](3,1)".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithStringArgumentYieldsNothing()
        {
            var result = "[1,2,3;4,5,6](\"0\")".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallMatrixWithBooleanArgumentYieldsNothing()
        {
            var result = "[1,2,3;4,5,6](true)".Eval();
            Assert.IsNull(result);
        }

        [Test]
        public void CallFunctionWithStatementsReturningObject()
        {
            var result = "((x, y) => { var a = x + y; var b = x - y; new { a: a, b: b}; })(2, 3)".Eval();
            var obj = result as IDictionary<String, Object>;
            Assert.IsNotNull(obj);
            Assert.AreEqual(5.0, obj["a"]);
            Assert.AreEqual(-1.0, obj["b"]);
        }

        [Test]
        public void CustomFunctionShouldBeCurried4Times()
        {
            var result = "f = (x,y,z)=>x+y^2+z^3; f()(1)(2)(3)".Eval();
            Assert.AreEqual(32.0, result);
        }

        [Test]
        public void CustomFunctionShouldBeCurriedEqualToOriginal()
        {
            var result = "f = (x,y,z)=>x+y^2+z^3; f() == f".Eval();
            Assert.AreEqual(true, result);
        }

        [Test]
        public void CustomFunctionShouldBeCurried2Times()
        {
            var result = "f = (x,y,z)=>x+y^2+z^3; f(1)(3,3)".Eval();
            Assert.AreEqual(37.0, result);
        }

        [Test]
        public void VariableArgumentsWithImpliedArgsWithoutNaming()
        {
            var result = "f = ()=>length(args); f(1,2,3,\"hi\", true)".Eval();
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void VariableArgumentsWithImpliedArgsDespiteNamedArguments()
        {
            var result = "f = (a,b)=>length(args); f(\"hi\", true)".Eval();
            Assert.AreEqual(2.0, result);
        }

        [Test]
        public void VariableArgumentsAccessWorks()
        {
            var result = "f = ()=>args(2); f(\"hi\", true, 42)".Eval();
            Assert.AreEqual(42.0, result);
        }

        [Test]
        public void VariableArgumentsNotExposedIfArgumentsNamedAccordingly()
        {
            var result = "f = (args)=>length(args); f(1, 2, 3, 4)".Eval();
            Assert.AreEqual(1.0, result);
        }

        [Test]
        public void VariableArgumentsOverwrittenIfLocalVariableExists()
        {
            var result = "f = ()=>{ var args = 1; length(args); }; f(1, 2, 3, 4)".Eval();
            Assert.AreEqual(1.0, result);
        }

        [Test]
        public void EmptyListYieldsZeroEntries()
        {
            var result = "length(list())".Eval();
            Assert.AreEqual(0.0, result);
        }

        [Test]
        public void ListWithFourDifferentEntries()
        {
            var result = "length(list(1, true, [1,2,3], new { }))".Eval();
            Assert.AreEqual(4.0, result);
        }

        [Test]
        public void ListWithOneEntryIndexGetAccessor()
        {
            var result = "list(new { a : 5 })(0).a".Eval();
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void ListWithOneEntryAddNewEntryWithIndexSetAccessor()
        {
            var result = "l = list(false); l(1) = \"foo\"; length(l)".Eval();
            Assert.AreEqual(2.0, result);
        }

        [Test]
        public void TypeOfNothingIsUndefined()
        {
            var result = "type(null)".Eval();
            Assert.AreEqual("Undefined", result);
        }

        [Test]
        public void TypeOfMatrixIsMatrix()
        {
            var result = "type([])".Eval();
            Assert.AreEqual("Matrix", result);
        }

        [Test]
        public void TypeOfDictionaryIsObject()
        {
            var result = "type(new {})".Eval();
            Assert.AreEqual("Object", result);
        }

        [Test]
        public void TypeOfStringIsString()
        {
            var result = "type(\"\")".Eval();
            Assert.AreEqual("String", result);
        }

        [Test]
        public void TypeOfBooleanIsBoolean()
        {
            var result = "type(true)".Eval();
            Assert.AreEqual("Boolean", result);
        }

        [Test]
        public void TypeOfDoubleIsNumber()
        {
            var result = "type(2.3)".Eval();
            Assert.AreEqual("Number", result);
        }

        [Test]
        public void TypeOfDelegateIsFunction()
        {
            var result = "type(() => {})".Eval();
            Assert.AreEqual("Function", result);
        }

        [Test]
        public void TypeIsCurriedForZeroArguments()
        {
            var result = "type() == type".Eval();
            Assert.AreEqual(true, result);
        }

        [Test]
        public void RecursiveObjectShouldNotCrashJson()
        {
            var result = "x = new {}; x.y = x; json(x)".Eval();
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<String>(result);
        }

        [Test]
        public void FunctionWorkWithLexicalCaptures()
        {
            var result = "var f = () => { var a = 5; return () => a; }; var a = 3; var g = f(); g()".Eval();
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void MapFunctionShouldReturnScalar()
        {
            var result = "map(x => x, 3)".Eval();
            Assert.AreEqual(3.0, result);
        }

        [Test]
        public void MapFunctionShouldReturnLengthOfEachValue()
        {
            var result = "map(length, new { a: \"hi\", b: \"foo\", c: \"here\" })".Eval() as IDictionary<String, Object>;

            Assert.IsNotNull(result);
            Assert.AreEqual(2.0, result["a"]);
            Assert.AreEqual(3.0, result["b"]);
            Assert.AreEqual(4.0, result["c"]);
        }

        [Test]
        public void MapFunctionShouldReturnLengthOfEachKey()
        {
            var result = "map((v, k) => length(k), new { eins: \"hi\", two: \"foo\", three: \"here\" })".Eval() as IDictionary<String, Object>;

            Assert.IsNotNull(result);
            Assert.AreEqual(4.0, result["eins"]);
            Assert.AreEqual(3.0, result["two"]);
            Assert.AreEqual(5.0, result["three"]);
        }

        [Test]
        public void MapFunctionShouldConvertMatrixToListObject()
        {
            var result = "map(factorial, [1, 2, 3; 4, 5, 6])".Eval() as IDictionary<String, Object>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1.0, result["0"]);
            Assert.AreEqual(2.0, result["1"]);
            Assert.AreEqual(6.0, result["2"]);
            Assert.AreEqual(24.0, result["3"]);
            Assert.AreEqual(120.0, result["4"]);
            Assert.AreEqual(720.0, result["5"]);
        }
    }
}
