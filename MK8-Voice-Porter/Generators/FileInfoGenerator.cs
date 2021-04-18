using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoicePorter
{
    class FileInfoGenerator
    {
        public static void GenerateFileInfo(string wavDirectory, string bfwavDirectory, string fileInfoDirectory)
        {
            string[] wavDriverFolders = Directory.GetDirectories(wavDirectory);

            foreach (string wavDriverFolder in wavDriverFolders)
            {
                string driverName = Path.GetFileName(wavDriverFolder);
                Utilities.progressDesc = $"Generating {driverName} file info...";

                FileInfoData fileInfoData = new FileInfoData();

                string[] wavFiles = Directory.GetFiles(wavDriverFolder);

                foreach (string wavFile in wavFiles)
                {
                    string consoleOutput = Utilities.RunConsoleCommand("GetMD5.bat", $"{wavFile} MD5");
                    string[] stringSeparators = new string[] { "\r\n" };
                    string checksum = consoleOutput.Split(stringSeparators, StringSplitOptions.None)[3];

                    string wavFileName = Path.GetFileNameWithoutExtension(wavFile);

                    long wavFileSize = new FileInfo(wavFile).Length;
                    long bfwavFileSize = new FileInfo(Path.Combine(bfwavDirectory, driverName, wavFileName + ".bfwav")).Length;

                    fileInfoData.AddElement(Path.GetFileNameWithoutExtension(wavFileName), bfwavFileSize.ToString(), wavFileSize.ToString(), checksum);
                }

                string jsonText = JsonConvert.SerializeObject(fileInfoData, Formatting.Indented);
                StreamWriter writer = new StreamWriter(Path.Combine(fileInfoDirectory, driverName + ".json"));
                writer.Write(jsonText);
                writer.Close();
                Utilities.progressValue++;
            }

            Utilities.progressDesc = $"Generation completed successfully!";

            //fileInfoData.elements.Add(new FileInfoData.FileInfoElement("","",""));


        }
    }
}
