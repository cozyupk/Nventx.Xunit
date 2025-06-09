using System;
using Xunit;

namespace NventX.xProof.SupportingXunit.SelfHostedTestRuntime
{
    public class ConsoleAndDebugLogger : IRunnerLogger
    {
        public object LockObject { get; } = new();

        private static void WriteLine(string message, ConsoleColor forgroundColor = default)
        {
            if (forgroundColor != default)
            {
                Console.ForegroundColor = forgroundColor;
            }
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
            if (forgroundColor != default)
            {
                Console.ResetColor();
            }
        }
        public void LogError(StackFrameInfo stackFrame, string message)
        {
            WriteLine($"[ERROR] {message}", ConsoleColor.Red);
        }

        public void LogImportantMessage(StackFrameInfo stackFrame, string message)
        {
            WriteLine(message, ConsoleColor.Green);
        }

        public void LogMessage(StackFrameInfo stackFrame, string message)
        {
            WriteLine(message, ConsoleColor.Gray);
        }

        public void LogRaw(string message)
        {
            WriteLine(message); // 色なしそのまま
        }

        public void LogWarning(StackFrameInfo stackFrame, string message)
        {
            WriteLine($"[WARNING] {message}", ConsoleColor.Yellow);
        }
    }
}
