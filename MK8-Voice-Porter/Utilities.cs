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

        public static string RunConsoleCommand(string exe, string arguments, bool createNoWindow = true)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.CreateNoWindow = createNoWindow;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(exe);
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }
    }
}
