using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoicePorter
{
    class Utilities
    {
        public static int progressValue = 0;
        public static string progressDesc = string.Empty;

        public static string RunConsoleCommand(string exe, string arguments, string workingDirectory)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        public static bool StringStartsWithAny(string str, params string[] values)
        {
            foreach (string value in values)
            {
                if (str.StartsWith(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;

        }
    }
}
