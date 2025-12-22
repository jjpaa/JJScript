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

                IEnumerable<string> lines = File.ReadLines(filePath);
                charCounter = 0;
                foreach (var inputLine in lines)
                {
                    originalLine = inputLine;
                    charCounter++;
                    bool lineDone = false;
                    string l = inputLine;

                    while (!lineDone)
                    {
                        if (l.StartsWith(" ") || l.StartsWith("\t"))
                        {
                            l = l.Substring(1);
                        }
                        if (l.StartsWith("//"))
                        {
                            // This is comment. Skip it
                            break;
                        }
                        if (l.StartsWith("print("))
                        {
                            string afterSplit = l.Split("print(").Last();
                            if (afterSplit.First() == '"')
                            {
                                // string starts find where it ends
                                bool timeToStop = false;

                                foreach (char c in afterSplit)
                                {
                                    if (timeToStop == false)
                                    {
                                        if (c != '"')
                                        {
                                            Console.Write(c);
                                        }
                                        else
                                        {
                                            //This string is done
                                            timeToStop = true;
                                        }
                                    }
                                    else
                                    {
                                        if (c == ')')
                                        {
                                            // All good
                                            lineDone = true;
                                            break;
                                        }
                                        else
                                        {
                                            throw new SyntaxException() { Expected = ")", Was = c.ToString() };
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Not string yet
                                throw new SyntaxException() { Expected = "\"", Was = afterSplit.First().ToString() };
                            }
                        }
                    }

                    line++;
                }
            }
            catch (SyntaxException ex)
            {
                Console.WriteLine($"Print statement, expexting {ex.Expected}, but was {ex.Was} on input row: {line}, char {charCounter}");
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
