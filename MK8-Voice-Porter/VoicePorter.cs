using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows;

namespace MK8VoicePorter
{
    public class VoicePorter
    {
        public MainWindow window;

        private string inputFolderName = "input-files";
        private string outputFolderName = "output-files";

        private string[] inputFiles = new string[0];
        private List<string> outputFiles = new List<string>();

        FileNameData[] data = new FileNameData[0];

        public VoicePorter(MainWindow window)
        {
            this.window = window;

            Initialise();
        }

        public void Initialise()
        {
            Directory.CreateDirectory(inputFolderName);
            Directory.CreateDirectory(outputFolderName);


            string[] characterDataFiles = Directory.GetFiles("./data/", "*.json");
            data = new FileNameData[characterDataFiles.Length];

            for (int i = 0; i < characterDataFiles.Length; i++)
            {
                StreamReader reader = new StreamReader(characterDataFiles[i]);
                data[i] = JsonConvert.DeserializeObject<FileNameData>(reader.ReadToEnd());
                reader.Close();
                window.cmb_TargetCharacter.Items.Add(data[i].character);
            }
            window.cmb_TargetCharacter.SelectedIndex = 0;
        }

        public void Port()
        {
            inputFiles = Directory.GetFiles(inputFolderName);
            outputFiles = new List<string>();

            foreach (string file in Directory.GetFiles(outputFolderName))
            {
                File.Delete(file);
            }

            foreach (FileNameData d in data)
            {
                for (int f = 0; f < inputFiles.Length; f++)
                {
                    string file = Path.GetFileNameWithoutExtension(inputFiles[f]);
                    foreach (FileNameData.FileNameDataElement element in d.elements)
                    {
                        if (file == element.switchName || file == element.wiiuName || file == element.userFriendlyName)
                        {
                            string value = outputFolderName + "/" + element.userFriendlyName + ".bfwav";
                            outputFiles.Add(value);
                            File.Copy(inputFiles[f], value);
                        }
                    }
                }
            }

            inputFiles = outputFiles.ToArray();
            outputFiles.Clear();

            for (int f = 0; f < inputFiles.Length; f++)
            {
                string file = Path.GetFileNameWithoutExtension(inputFiles[f]);

                foreach (FileNameData d in data)
                {
                    if (d.character == window.cmb_TargetCharacter.SelectedItem.ToString())
                    {
                        foreach (FileNameData.FileNameDataElement element in d.elements)
                        {
                            if (file == element.userFriendlyName && element.wiiuName != null)
                            {
                                string value = outputFolderName + "/" + element.wiiuName + ".bfwav";
                                outputFiles.Add(value);
                                if (!File.Exists(value))
                                {
                                    File.Move(inputFiles[f], value);
                                }
                            }
                        }
                    }

                }
            }

            //Find any files that didn't find a match for the ported character
            inputFiles = Directory.GetFiles(outputFolderName);
            List<string> errors = new List<string>();
            for (int i = 0; i < inputFiles.Length; i++)
            {
                string file = Path.GetFileNameWithoutExtension(inputFiles[i]);

                foreach (FileNameData.FileNameDataElement element in data[0].elements)
                {
                    if (file == element.userFriendlyName)
                    {
                        Console.WriteLine("Failed to find match for " + file);
                        errors.Add(file);
                        File.Delete(inputFiles[i]);
                    }
                }
            }

            //Show exception
            if (errors.Count > 0)
            {
                string errorText = "Failed to find match for the following files:\n";
                foreach (string error in errors)
                {
                    errorText += error + "\n";
                }
                errorText += "\nThis might be because the character you have ported to reuses clips, so this may not be an error.";
                MessageBox.Show(errorText, "Failed to find matches");
            }

            Process.Start(outputFolderName);
        }
    }

    public struct FileWithChecksum
    {
        public string fullFilePath;
        public string fileName;
        public string checksumMD5;
        
        public FileWithChecksum(string fullFilePath, string fileName, string checksumMD5)
        {
            this.fullFilePath = fullFilePath;
            this.fileName = fileName;
            this.checksumMD5 = checksumMD5;
        }
    }
}
