using System;
using System.Threading;      // Needed for Thread.Sleep
using Spectre.Console;       // Spectre.Console library for fancy console output

namespace Test
{
    // This static class contains our simple progress loader
    internal static class ProgressDemo
    {
        // Run method executes the progress bar
        public static void Run()
        {
            // Create and start a progress display
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    // Define two tasks with descriptive labels
                    var task1 = ctx.AddTask("[green]Reticulating splines[/]");  // Task 1
                    var task2 = ctx.AddTask("[green]Folding space[/]");         // Task 2

                    // Loop until all tasks are finished
                    while (!ctx.IsFinished)
                    {
                        // Increment the tasks by a small amount
                        task1.Increment(0.7);   // Increase task1 progress
                        task2.Increment(0.5);   // Increase task2 progress

                        // Pause briefly to make the progress visible
                        Thread.Sleep(8);
                    }
                });
        }
    }
}
