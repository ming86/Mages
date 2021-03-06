﻿namespace Mages.Core.Runtime.Functions
{
    using System;
    using System.Collections.Generic;

    static class Global
    {
        public static readonly IDictionary<String, Object> Mapping = new Dictionary<String, Object>
        {
            { "abs", StandardOperators.Abs },
            { "not", StandardOperators.Not },
            { "type", StandardOperators.Type },
            { "factorial", StandardOperators.Factorial },
            { "positive", StandardOperators.Positive },
            { "negative", StandardOperators.Negative },
            { "transpose", StandardOperators.Transpose },
            { "pow", StandardOperators.Pow },
            { "add", StandardOperators.Add },
            { "and", StandardOperators.And },
            { "equals", StandardOperators.Eq },
            { "notEquals", StandardOperators.Neq },
            { "lessEquals", StandardOperators.Leq },
            { "less", StandardOperators.Lt },
            { "greaterEquals", StandardOperators.Geq },
            { "greater", StandardOperators.Gt },
            { "modulo", StandardOperators.Mod },
            { "multiply", StandardOperators.Mul },
            { "or", StandardOperators.Or },
            { "divide", StandardOperators.RDiv },
            { "subtract", StandardOperators.Sub },
            { "ceil", StandardFunctions.Ceil },
            { "exp", StandardFunctions.Exp },
            { "floor", StandardFunctions.Floor },
            { "round", StandardFunctions.Round },
            { "log", StandardFunctions.Log },
            { "sign", StandardFunctions.Sign },
            { "sqrt", StandardFunctions.Sqrt },
            { "rand", StandardFunctions.Rand },
            { "sin", StandardFunctions.Sin },
            { "cos", StandardFunctions.Cos },
            { "tan", StandardFunctions.Tan },
            { "sinh", StandardFunctions.Sinh },
            { "cosh", StandardFunctions.Cosh },
            { "tanh", StandardFunctions.Tanh },
            { "arcsin", StandardFunctions.ArcSin },
            { "arccos", StandardFunctions.ArcCos },
            { "arctan", StandardFunctions.ArcTan },
            { "isnan", StandardFunctions.IsNaN },
            { "isint", StandardFunctions.IsInt },
            { "isprime", StandardFunctions.IsPrime },
            { "isinfty", StandardFunctions.IsInfty },
            { "min", StandardFunctions.Min },
            { "max", StandardFunctions.Max },
            { "sort", StandardFunctions.Sort },
            { "reverse", StandardFunctions.Reverse },
            { "throw", StandardFunctions.Throw },
            { "catch", StandardFunctions.Catch },
            { "length", StandardFunctions.Length },
            { "sum", StandardFunctions.Sum },
            { "any", StandardFunctions.Any },
            { "all", StandardFunctions.All },
            { "stringify", Stringify.Default },
            { "json", Stringify.Json },
            { "list", StandardFunctions.List },
            { "is", StandardFunctions.Is },
            { "as", StandardFunctions.As },
            { "map", StandardFunctions.Map },
            { "reduce", StandardFunctions.Reduce },
            { "where", StandardFunctions.Where },
            { "concat", StandardFunctions.Concat },
            { "zip", StandardFunctions.Zip },
            { "intersection", StandardFunctions.Intersection },
            { "union", StandardFunctions.Union },
            { "except", StandardFunctions.Except },
        };
    }
}
