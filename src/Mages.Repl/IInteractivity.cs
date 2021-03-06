﻿namespace Mages.Repl
{
    using System;

    interface IInteractivity
    {
        Boolean IsPromptShown { get; set; }

        String Read();

        void WritePrompt();

        void Write(String output);

        void Info(Object result);

        void Error(String message);
    }
}
