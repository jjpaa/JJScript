using System.Diagnostics;

namespace SciptTests
{
    public class PrintLnTests
    {
        [Fact]
        public void PrintlnTest1()
        {
            // Executable location
            string path = @"..\..\..\..\JJScript\bin\Debug\net10.0\JJScript.exe";

            var psi = new ProcessStartInfo
            {
                FileName = path,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string inputFile = "TestScriptInput.script";
            var pathToFile = Path.GetFullPath(inputFile);
            psi.ArgumentList.Add(pathToFile);

            using var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            string shouldReturn = "Hello World!\r\n" + "Program ended\r\n";
            Assert.True(shouldReturn == output);
            
        }

        [Fact]
        public void PrintlnTest2()
        {
            // Executable location
            string path = @"..\..\..\..\JJScript\bin\Debug\net10.0\JJScript.exe";

            var psi = new ProcessStartInfo
            {
                FileName = path,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string inputFile = "TestScriptInput2.script";
            var pathToFile = Path.GetFullPath(inputFile);
            psi.ArgumentList.Add(pathToFile);

            using var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            string shouldReturn =
@"Hello World
Hello World2
hello
Hello
Hell o
 Hel l o 
Program ended
";
            Assert.True(shouldReturn == output);
        }
    }
}
