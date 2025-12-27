namespace JJScript
{
    public class SyntaxException : Exception
    {
        public required string Expected;
        public required string Was;
    }
    public class DataException : Exception
    {
        public required string ExtraMessage;
    }

    public class Program
    {
        public static long line = 1;
        public static string originalLine = "";
        public static long charCounter = 0;
        public static string statementString = "";
        public static bool lineDone = false;
        public static bool trimString = true;
        public static string modifiedLine = "";

        public static Dictionary<string,string> StringsDictionary = new Dictionary<string,string>();

        static void Main(string[] args)
        {
            //Console.WriteLine("Printing arguments");

            //foreach (var item in args)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine("Stopped print arguments");

            Run(args);
        }

        public static void Run(string[] args)
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

                ProcessInput(filePath);
            }
            catch (SyntaxException ex)
            {
                Console.WriteLine($"Print statement, expexting {ex.Expected}, but was {ex.Was} on input row: {line}, char {charCounter}");
                Console.WriteLine("");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("");
                Console.WriteLine(ex.Message);
                Console.WriteLine("");
                Console.WriteLine("Statement string was: " + statementString);
            }
            catch (DataException ex)
            {
                Console.WriteLine($"Data exception. {ex.ExtraMessage} on input row: {line}, char {charCounter}");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Statement string was: " + statementString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Statement string was: " + statementString);
            }

            Console.WriteLine("Program ended");
        }

        static void ProcessInput(string filePathOrInput)
        {
            IEnumerable<string> lines = new List<string>();

            // Check if input is actually file or just inline string
            if (File.Exists(filePathOrInput))
            {
                lines = File.ReadLines(filePathOrInput);
            }
            //else
            //{
            //    //Console.WriteLine("Inline execution");
            //    var l = new List<string>();
            //    l.Add(filePathOrInput);
            //    lines = l;
            //}

            //IterateThroughTheLine
            foreach (var inputLine in lines)
            {
                //Console.WriteLine("Input line: " + inputLine);

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

                    if (modifiedLine.StartsWith("string"))
                    {
                        trimString = false;
                    }

                    var firstChar = modifiedLine[0];

                    switch (firstChar)
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
                                    // This is inside of print statement. Do not print
                                    if (!trimString)
                                    {
                                        statementString += firstChar;
                                        statementString += modifiedLine[1];
                                    }
                                    modifiedLine = modifiedLine.Substring(2);
                                    break;
                                case '\n':
                                    // new line
                                    break;
                                case '\"':
                                    //trimString = !trimString;
                                    break;
                                case '\'':
                                    // apostrophe
                                    break;
                            }
                            break;
                        case ';':
                            HandleStatement();
                            statementString = "";
                            lineDone = true;
                            break;
                        case '"':
                            trimString = !trimString;
                            statementString += firstChar;
                            modifiedLine = modifiedLine.Substring(1);
                            break;
                        case ' ':

                            if (!trimString)
                            {
                                // This is inside of print statement. Do not print
                                statementString += firstChar;
                            }
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

            if (statementString.StartsWith("printl("))
            {
                HandlePrintLn();
            }
            else if (statementString.StartsWith("string "))
            {
                HandleString();
            }
            else
            {
                throw new SyntaxException() { Expected = "Valid statement", Was = statementString };
            }
        }

        private static void HandleString()
        {
            try
            {
                // Start
                string afterSplit = statementString.Split("string ").Last();

                // Seek the name part
                var splits = afterSplit.Split(" = ");
                string stringName = splits.First();
                string TheActualText = splits.Last();

                // Check if syntax is correct
                var temp = TheActualText.Trim('"','\\');

                var stringItselfAndStatementEnding = temp.Split('"');
                var stringItself = stringItselfAndStatementEnding.First();

                try
                {
                    // All good
                    StringsDictionary[stringName] = stringItself;
                }
                catch (Exception)
                {
                    throw new DataException() { ExtraMessage = "String Dictionary failed" };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void HandlePrintLn()
        {
            string afterSplit = statementString.Split("printl(").Last();

            // Inline string
            if(afterSplit.StartsWith("\""))
            {
                var splitsStrings = afterSplit.Split("\"");

                if (splitsStrings.Last() == ")")
                {
                    // All good
                    Console.WriteLine(splitsStrings[1]);
                }
                else
                {
                    throw new SyntaxException() { Expected = ")", Was = splitsStrings.Last()};
                }
            }

            // Variable string
            else
            {
                // Try to find string from strings and then print it.
                var stringName = afterSplit.Split(")").First();
                if (StringsDictionary.ContainsKey(stringName))
                {
                    StringsDictionary.TryGetValue(stringName, out var value);
                    Console.WriteLine(value);
                }
                else
                {
                    throw new DataException() { ExtraMessage = $"<{stringName} does not exists in known strings>"};
                }
            }
        }
    }
}
