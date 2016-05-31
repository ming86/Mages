﻿namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;

    /// <summary>
    /// Pops one object[] from the stack and exposes a args object.
    /// </summary>
    sealed class ArgsOperation : IOperation
    {
        public static readonly IOperation Instance = new ArgsOperation();
        private static readonly String ArgsIdentifier = "args";

        private ArgsOperation()
        {
        }

        public void Invoke(IExecutionContext context)
        {
            var args = (Object[])context.Pop();

            if (!context.Scope.ContainsKey(ArgsIdentifier))
            {
                var obj = args.ToArrayObject();
                context.Scope.Add(ArgsIdentifier, obj);
            }
        }
    }
}