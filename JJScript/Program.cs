namespace JJScript
{
    public class SyntaxException : Exception
    {
        public required string Expected;
        public required string Was;
    }

    internal class Program
    {
        public static long line = 1;
        public static string originalLine = "";
        public static long charCounter = 0;
        public static string statementString = "";
        public static bool lineDone = false;
        public static string modifiedLine = "";

        static void Main(string[] args)
        {

            try
            {
                string filePath = "";

                if (args.Length == 0)
                {
                    filePath = "Test.script";
                }
                else
                {
                    filePath = args.First();
                }

                line = 1;

                IterateThroughTheLine(filePath);
            }
            catch (SyntaxException ex)
            {
                Console.WriteLine($"Print statement, expexting {ex.Expected}, but was {ex.Was} on input row: {line}, char {charCounter}");
                Console.WriteLine("");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("");
                Console.WriteLine(ex.Message);
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Program ended");
        }

        static void IterateThroughTheLine(string filePath)
        {
            IEnumerable<string> lines = File.ReadLines(filePath);
            foreach (var inputLine in lines)
            {
                charCounter = 0;
                originalLine = inputLine;
                lineDone = false;
                modifiedLine = inputLine;

                while (!lineDone)
                {
                    charCounter++;
                    // Check escape chars

                    if (modifiedLine == "")
                    {
                        lineDone = true;
                        break;
                    }

                    var firstChar = modifiedLine[0];

                    switch(firstChar)
                    {
                        // Escpace characters
                        case '\\':
                            switch (modifiedLine[1])
                            {
                                case '\\':
                                    // Backslash?
                                    break;
                                case '\t':
                                    // tab
                                    modifiedLine = modifiedLine.Substring(2);
                                    break;
                                case '\n':
                                    // new line
                                    break;
                                case '\"':
                                    // double quete
                                    break;
                                case '\'':
                                    // double quete
                                    break;
                            }
                            break;
                        case ';':
                            HandleStatement();
                            statementString = "";
                            lineDone = true;
                            break;
                        case ' ':
                            modifiedLine = modifiedLine.Substring(1);
                            break;
                        case '/':
                            if (modifiedLine[1] == '/')
                            {
                                // Rest is comment. Skip
                                lineDone = true;
                                break;
                            }
                            throw new SyntaxException() { Expected = "/", Was = modifiedLine[1].ToString() };
                        default:
                            // Put values into statement
                            statementString += firstChar;
                            modifiedLine = modifiedLine.Substring(1);
                            break;
                    }
                }
                line++;
            }
        }

        private static void HandleStatement()
        {
            // This is statement

            if (statementString.StartsWith("print(\""))
            {
                HandlePrint();
            }
            else
            {
                throw new SyntaxException() { Expected = "Valid statement", Was = statementString };
            }
        }

        private static void HandlePrint()
        {
            // Start
            string afterSplit = statementString.Split("print(\"").Last();
            
            // Stuff to print
            var splits = afterSplit.Split("\"");
            
            // Check that ending is right
            Console.WriteLine(splits[0]);
            if(splits[1].First() == ')')
            {
                // All good
            }
            else
            {
                throw new SyntaxException() { Expected = ")", Was = splits[1].First().ToString() };
            }
        }
    }
}
