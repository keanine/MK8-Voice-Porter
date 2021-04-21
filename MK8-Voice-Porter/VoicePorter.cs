﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MK8VoicePorter
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

            //move the friendly temp file to the output with the new name

            //NOTES
            //If there are conflicts (U uses the same sound for some actions) then warn the user
            // For targets that reuse the same sound for multiple actions, ask the user which sound they want to use
        }

        public static void PortToBARS(string targetDriverFile)
        {
            DriverIdentityData[] driverIdentities = DeserializeDriverIdentityData();
            DriverIdentityData targetIdentity = FindTargetDriverIdentity(targetDriverFile, driverIdentities);

            Converter.ConvertFilesToBFWAV(GlobalDirectory.inputFolder, GlobalDirectory.bfwavTempFolder);
            string[] tempFiles = RenametoFriendlyAlias(GlobalDirectory.bfwavTempFolder, GlobalDirectory.friendlyTempFolder, "bfwav", driverIdentities);

            AssignTargetName(tempFiles, GlobalDirectory.finalTempFolder, targetIdentity);

            //Add a condition for unlock and 
            Uwizard.SARC.pack(GlobalDirectory.finalTempFolder, GlobalDirectory.outputFolder + "SNDG_" + targetIdentity.fileName + ".bars");
        }

        public static void PortToBFWAV(string targetDriverFile)
        {
            DriverIdentityData[] driverIdentities = DeserializeDriverIdentityData();
            DriverIdentityData targetIdentity = FindTargetDriverIdentity(targetDriverFile, driverIdentities);

            Converter.ConvertFilesToBFWAV(GlobalDirectory.inputFolder, GlobalDirectory.bfwavTempFolder);
            string[] tempFiles = RenametoFriendlyAlias(GlobalDirectory.bfwavTempFolder, GlobalDirectory.friendlyTempFolder, "bfwav", driverIdentities);

            AssignTargetName(tempFiles, GlobalDirectory.outputFolder, targetIdentity);
        }

        public static void PortToFriendlyBFWAV(string targetDriverFile)
        {
            DriverIdentityData[] driverIdentities = DeserializeDriverIdentityData();
            DriverIdentityData targetIdentity = FindTargetDriverIdentity(targetDriverFile, driverIdentities);

            Converter.ConvertFilesToBFWAV(GlobalDirectory.inputFolder, GlobalDirectory.bfwavTempFolder);
            RenametoFriendlyAlias(GlobalDirectory.bfwavTempFolder, GlobalDirectory.outputFolder, "bfwav", driverIdentities);
        }

        public static void PortToFriendlyWAV(string targetDriverFile)
        {
            DriverIdentityData[] driverIdentities = DeserializeDriverIdentityData();
            DriverIdentityData targetIdentity = FindTargetDriverIdentity(targetDriverFile, driverIdentities);

            FilesToWAV(GlobalDirectory.inputFolder, GlobalDirectory.wavTempFolder);
            RenametoFriendlyAlias(GlobalDirectory.wavTempFolder, GlobalDirectory.outputFolder, "wav", driverIdentities);
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

        public static string[] RenametoFriendlyAlias(string inputFolder, string outputFolder, string extension, DriverIdentityData[] driverIdentities)
        {
            //Get all files in the input folder
            string[] inputFiles = Directory.GetFiles(inputFolder, $"*.{extension}");


            var unusedAliases = AliasData.GetCopyOfVoiceFileAlias();

            //Copy the input file to the temp folder with the friendly name
            List<string> tempFiles = new List<string>();

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
                                tempFiles.Add(destination);
                            }
                        }
                    }
                }
            }

            for (int f = 0; f < inputFiles.Length; f++)
            {
                File.Delete(inputFiles[f]);
            }

            return tempFiles.ToArray();
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

        public static void AssignTargetName(string[] inputFiles, string outputFolder, DriverIdentityData targetIdentity)
        {
            if (targetIdentity != null)
            {
                //Get the U name for the friendly temp file and add name/code
                for (int f = 0; f < inputFiles.Length; f++)
                {
                    string file = Path.GetFileNameWithoutExtension(inputFiles[f]);

                    foreach (var element in targetIdentity.elements)
                    {
                        if (file == element.userFriendlyName && element.uName != null)
                        {
                            string destination = outputFolder + element.uName + ".bfwav";
                            //outputFiles.Add(value);
                            if (!File.Exists(destination))
                            {
                                File.Move(inputFiles[f], destination);
                            }
                        }
                    }
                }
            }
        }
    }
}