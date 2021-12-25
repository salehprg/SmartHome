using System.Diagnostics;

namespace smarthome.Helper
{
    public static class ShellHelper
    {
        public static string Run(string filename , string args , bool wait = true)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            string result = "";

            if(wait)
                result = process.StandardOutput.ReadToEnd();

            return result;
        }
    }
}