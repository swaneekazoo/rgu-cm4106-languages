using System.Collections.Generic;
using static System.Console;

namespace Compiler.IO
{
    /// <summary>
    /// An object for reporting errors in the compilation process
    /// </summary>
    public class ErrorReporter
    {
        /// <summary>
        /// The number of errors encountered so far
        /// </summary>
        public int ErrorCount { get; private set; } = 0;

        /// <summary>
        /// Whether or not any errors have been encounter
        /// </summary>
        public bool HasErrors { get { return ErrorCount > 0; } }

        /// <summary>
        /// The errors encountered so far
        /// </summary>
        public Dictionary<Position, string> Errors { get; private set; } = new Dictionary<Position, string>();

        /// <summary>
        /// Reports an error
        /// </summary>
        /// <param name="message">The message to display</param>
        public void ReportError(Position position, string message)
        {
            ErrorCount += 1;
            Errors.Add(position, message);
            WriteLine($"ERROR: {message}");
        }

        public string FinalReport()
        {
            string finalReport = "";
            foreach (KeyValuePair<Position, string> error in Errors)
            {
                finalReport += $"{error.Key}: {error.Value}\n";
            }
            return finalReport;
        }
    }
}
