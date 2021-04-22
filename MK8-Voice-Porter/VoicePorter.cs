using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MK8VoiceTool
{
    public enum VoiceFileFormat { BARS, BFWAV, BFWAV_Friendly, WAV_Friendly }

    class VoicePorter
    {
        public static void Port(string targetDriverFile, VoiceFileFormat format)
        {
            foreach (string file in Directory.GetFiles(GlobalDirectory.outputFolder))
            {
                File.Delete(file);
            }

            GlobalDirectory.ClearTempFolders();

            switch (format)
            {
                case VoiceFileFormat.BARS:
                    PortToBARS(targetDriverFile);
                    break;
                case VoiceFileFormat.BFWAV:
                    PortToBFWAV(targetDriverFile);
                    break;
                case VoiceFileFormat.BFWAV_Friendly:
                    PortToFriendlyBFWAV(targetDriverFile);
                    break;
                case VoiceFileFormat.WAV_Friendly:
                    PortToFriendlyWAV(targetDriverFile);
                    break;
                default:
                    PortToBARS(targetDriverFile);
                    break;
            }
            GlobalDirectory.ClearTempFolders();

            //NOTES
            //If there are conflicts (U uses the same sound for some actions) then warn the user
            // For targets that reuse the same sound for multiple actions, ask the user which sound they want to use
        }

        public static void PortToBARS(string targetDriverFile)
        {
            DriverIdentityData[] driverIdentities = DeserializeDriverIdentityData();
            DriverIdentityData targetIdentity = FindTargetDriverIdentity(targetDriverFile, driverIdentities);

            Converter.ConvertFilesToBFWAV(GlobalDirectory.inputFolder, GlobalDirectory.bfwavTempFolder);
            RenameToFriendlyAlias(GlobalDirectory.bfwavTempFolder, GlobalDirectory.friendlyTempFolder, "bfwav", driverIdentities);

            //if contains menu, pack that and delete
            string menuFilepath = GlobalDirectory.friendlyTempFolder + "SELECT_DRIVER.bfwav";
            if (File.Exists(menuFilepath))
            {
                AssignTargetNameToFile(menuFilepath, GlobalDirectory.finalTempFolder, targetIdentity);

                string menuParam = GlobalDirectory.menuParamsDirectory + targetIdentity.fileName + "_param.bin";
                File.Copy(menuParam, GlobalDirectory.finalTempFolder + "_param.bin");

                if (Directory.GetFiles(GlobalDirectory.finalTempFolder).Length > 1)
                    Uwizard.SARC.pack(GlobalDirectory.finalTempFolder, GlobalDirectory.outputFolder + "SNDG_M_" + targetIdentity.fileName + ".bars", 0x020);
                else
                    System.Windows.MessageBox.Show($"There were no compatible menu files for {targetIdentity.fileName}");

                foreach (string file in Directory.GetFiles(GlobalDirectory.finalTempFolder))
                {
                    File.Delete(file);
                }
            }

            //if contains unlock, pack that and delete
            string unlockFilepath = GlobalDirectory.friendlyTempFolder + "UNLOCK_DRIVER.bfwav";
            if (File.Exists(unlockFilepath))
            {
                AssignTargetNameToFile(unlockFilepath, GlobalDirectory.finalTempFolder, targetIdentity);

                string menuParam = GlobalDirectory.menuParamsDirectory + targetIdentity.fileName + "_param.bin";
                File.Copy(menuParam, GlobalDirectory.finalTempFolder + "_param.bin");

                if (Directory.GetFiles(GlobalDirectory.finalTempFolder).Length > 1)
                    Uwizard.SARC.pack(GlobalDirectory.finalTempFolder, GlobalDirectory.outputFolder + "SNDG_N_" + targetIdentity.fileName + ".bars", 0x020);
                else
                    System.Windows.MessageBox.Show($"There were no compatible unlock files for {targetIdentity.fileName}");

                foreach (string file in Directory.GetFiles(GlobalDirectory.finalTempFolder))
                {
                    File.Delete(file);
                }
            }

            //pack everything else

            AssignTargetName(GlobalDirectory.friendlyTempFolder, GlobalDirectory.finalTempFolder, targetIdentity);

            string param = GlobalDirectory.driverParamsDirectory + targetIdentity.fileName + "_param.bin";
            File.Copy(param, GlobalDirectory.finalTempFolder + "_param.bin");

            //Add a condition for unlock and menu
            if (Directory.GetFiles(GlobalDirectory.finalTempFolder).Length > 1)
                Uwizard.SARC.pack(GlobalDirectory.finalTempFolder, GlobalDirectory.outputFolder + "SNDG_" + targetIdentity.fileName + ".bars", 0x020);
            else
                System.Windows.MessageBox.Show($"There were no compatible driver files for {targetIdentity.fileName}");
        }

        public static void PortToBFWAV(string targetDriverFile)
        {
            DriverIdentityData[] driverIdentities = DeserializeDriverIdentityData();
            DriverIdentityData targetIdentity = FindTargetDriverIdentity(targetDriverFile, driverIdentities);

            Converter.ConvertFilesToBFWAV(GlobalDirectory.inputFolder, GlobalDirectory.bfwavTempFolder);
            RenameToFriendlyAlias(GlobalDirectory.bfwavTempFolder, GlobalDirectory.friendlyTempFolder, "bfwav", driverIdentities);

            AssignTargetName(GlobalDirectory.friendlyTempFolder, GlobalDirectory.outputFolder, targetIdentity);
        }

        public static void PortToFriendlyBFWAV(string targetDriverFile)
        {
            DriverIdentityData[] driverIdentities = DeserializeDriverIdentityData();

            Converter.ConvertFilesToBFWAV(GlobalDirectory.inputFolder, GlobalDirectory.bfwavTempFolder);
            RenameToFriendlyAlias(GlobalDirectory.bfwavTempFolder, GlobalDirectory.outputFolder, "bfwav", driverIdentities);
        }

        public static void PortToFriendlyWAV(string targetDriverFile)
        {
            DriverIdentityData[] driverIdentities = DeserializeDriverIdentityData();

            FilesToWAV(GlobalDirectory.inputFolder, GlobalDirectory.wavTempFolder);
            RenameToFriendlyAlias(GlobalDirectory.wavTempFolder, GlobalDirectory.outputFolder, "wav", driverIdentities);
        }

        public static void FilesToWAV(string inputFolder, string outputFolder)
        {
            //Extract BARS
            string[] barsFiles = Directory.GetFiles(inputFolder, "*.bars");
            if (barsFiles.Length > 1)
            {
                throw new System.Exception("Multiple BARS files found in the input folder. Please only use one.");
            }
            else if (barsFiles.Length == 1)
            {
                Uwizard.SARC.extract(barsFiles[0], GlobalDirectory.bfwavTempFolder);
            }

            //Convert BFWAV to WAV
            Converter.ConvertBFWAVtoWAV(inputFolder, outputFolder);
            Converter.ConvertBFWAVtoWAV(GlobalDirectory.bfwavTempFolder, outputFolder);
        }

        public static void RenameToFriendlyAlias(string inputFolder, string outputFolder, string extension, DriverIdentityData[] driverIdentities)
        {
            //Get all files in the input folder
            string[] inputFiles = Directory.GetFiles(inputFolder, $"*.{extension}");


            var unusedAliases = AliasData.GetCopyOfVoiceFileAlias();

            //Copy the input file to the temp folder with the friendly name
            foreach (DriverIdentityData driverIdentity in driverIdentities)
            {
                for (int f = 0; f < inputFiles.Length; f++)
                {
                    string file = Path.GetFileNameWithoutExtension(inputFiles[f]);
                    foreach (var element in driverIdentity.elements)
                    {
                        if ((file == element.dxName || file == element.uName || file == element.userFriendlyName) && file != "SE_SILENT_TRG.b.32.dspadpcm")
                        {
                            string destination = outputFolder + element.userFriendlyName + $".{extension}";

                            if (!File.Exists(destination))
                            {
                                File.Copy(inputFiles[f], destination);
                            }
                        }
                    }
                }
            }

            for (int f = 0; f < inputFiles.Length; f++)
            {
                File.Delete(inputFiles[f]);
            }
        }

        public static DriverIdentityData[] DeserializeDriverIdentityData()
        {
            string[] driverIdentityFiles = Directory.GetFiles(GlobalDirectory.identitiesDirectory);
            DriverIdentityData[] driverIdentities = new DriverIdentityData[driverIdentityFiles.Length];

            for (int i = 0; i < driverIdentityFiles.Length; i++)
            {
                StreamReader reader = new StreamReader(driverIdentityFiles[i]);
                driverIdentities[i] = JsonConvert.DeserializeObject<DriverIdentityData>(reader.ReadToEnd());
                reader.Close();
            }
            return driverIdentities;
        }

        public static DriverIdentityData FindTargetDriverIdentity(string targetDriverFile, DriverIdentityData[] driverIdentities)
        {
            string driverName = string.Empty;
            string driverCode = string.Empty;
            DriverIdentityData targetIdentity = null;

            //Get driver name and code
            foreach (DriverIdentityData driverIdentity in driverIdentities)
            {
                if (driverIdentity.fileName.Equals(targetDriverFile, StringComparison.OrdinalIgnoreCase))
                {
                    targetIdentity = driverIdentity;
                    break;
                }
            }
            return targetIdentity;
        }

        public static DriverIdentityData FindTargetDriverIdentity(string targetDriverFile)
        {
            DriverIdentityData[] driverIdentities = DeserializeDriverIdentityData();
            return FindTargetDriverIdentity(targetDriverFile, driverIdentities);
        }

        public static void AssignTargetName(string inputFolder, string outputFolder, DriverIdentityData targetIdentity)
        {
            if (targetIdentity != null)
            {
                foreach (var inputFile in Directory.GetFiles(inputFolder))
                {
                    AssignTargetNameToFile(inputFile, outputFolder, targetIdentity);
                }
            }
        }

        public static void AssignTargetNameToFile(string inputFile, string outputFolder, DriverIdentityData targetIdentity)
        {
            //Get the U name for the friendly temp file and add name/code
            if (targetIdentity != null)
            {
                    string file = Path.GetFileNameWithoutExtension(inputFile);

                    foreach (var element in targetIdentity.elements)
                    {
                        if (file == element.userFriendlyName && element.uName != null)
                        {
                            string destination = outputFolder + element.uName + ".bfwav";
                            //outputFiles.Add(value);
                            if (!File.Exists(destination))
                            {
                                File.Move(inputFile, destination);
                            }
                        }
                    }
            }
        }
    }
}
