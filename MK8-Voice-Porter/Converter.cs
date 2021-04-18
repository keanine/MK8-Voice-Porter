using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoicePorter
{
    class Converter
    {
        public static void ConvertBFWAVtoWAV(string bfwavDirectory, string loopingAudioConverterRoot)
        {
            string[] driverFolders = Directory.GetDirectories(bfwavDirectory);

            foreach (string driverFolder in driverFolders)
            {
                string driverName = Path.GetFileName(driverFolder);

                string[] bfwavFiles = Directory.GetFiles(driverFolder, "*.bfwav");
                string argument = /*"--auto " + */Path.GetFullPath("./LAC_Settings.xml");

                if (bfwavFiles.Length > 0)
                {
                    foreach (string bfwavFile in bfwavFiles)
                    {
                        argument += " " + Path.GetFullPath(bfwavFile);
                    }

                    string commandOutput = Utilities.RunConsoleCommand(loopingAudioConverterRoot + "LoopingAudioConverter.exe", argument);

                    string[] wavFiles = Directory.GetFiles(loopingAudioConverterRoot + "output/", "*.wav");
                    foreach (string wavFile in wavFiles)
                    {
                        string destination = $"{Strings.wavDirectoryU}{driverName}/{Path.GetFileName(wavFile)}";
                        if (File.Exists(destination))
                        {
                            File.Delete(destination);
                        }
                        File.Move(wavFile, destination);
                    }
                }
            }
        }
    }
}
