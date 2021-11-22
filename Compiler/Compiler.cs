using Compiler.IO;
using Compiler.Tokenization;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace Compiler
{
    /// <summary>
    /// Compiler for code in a source file
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; }

        /// <summary>
        /// The file reader
        /// </summary>
        public IFileReader Reader { get; }

        /// <summary>
        /// The tokenizer
        /// </summary>
        public Tokenizer Tokenizer { get; }

        /// <summary>
        /// Creates a new compiler
        /// </summary>
        /// <param name="inputFile">The file containing the source code</param>
        /// <param name="binaryOutputFile">The file to write the binary target code to</param>
        /// <param name="textOutputFile">The file to write the text asembly code to</param>
        public Compiler(string inputFile, string binaryOutputFile, string textOutputFile)
        {
            Reporter = new ErrorReporter();
            Reader = new FileReader(inputFile);
            Tokenizer = new Tokenizer(Reader, Reporter);
        }

        /// <summary>
        /// Performs the compilation process
        /// </summary>
        public void Compile()
        {
            // Tokenize
            Write("Tokenising...");
            List<Token> tokens = Tokenizer.GetAllTokens();
            if (Reporter.HasErrors) return;
            WriteLine("Done");

            WriteLine(string.Join("\n", tokens));
        }

        /// <summary>
        /// Writes a message reporting on the success of compilation
        /// </summary>
        private void WriteFinalMessage()
        {
            // You need to fill this in with your own success/error report
        }

        /// <summary>
        /// Compiles the code in a file
        /// </summary>
        /// <param name="args">Should be three arguments - input file (*.tri), binary output file (*.tam), text output file (*.txt)</param>
        public static void Main(string[] args)
        {
            if (args == null || args.Length != 3 || args[0] == null || args[1] == null || args[2] == null)
                WriteLine("ERROR: Must call the program with exactly three arguments - input file (*.tri), binary output file (*.tam), text output file (*.txt)");
            else if (!File.Exists(args[0]))
                WriteLine($"ERROR: The input file \"{Path.GetFullPath(args[0])}\" does not exist");
            else
            {
                string inputFile = args[0];
                string binaryOutputFile = args[1];
                string textOutputFile = args[2];
                Compiler compiler = new Compiler(inputFile, binaryOutputFile, textOutputFile);
                WriteLine("Compiling...");
                compiler.Compile();
                compiler.WriteFinalMessage();
            }
        }
    }
}
