using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using Cafe;
//using Library.CommonFormats;
using BARSViewer; // For extracting Switch .bars
using Uwizard; // For extracting WiiU .bars

namespace MK8VoiceTool
{
    class Converter
    {
        public static void ExtractAllUBARS()
        {
            string barsDirectory = GlobalDirectory.barsDirectoryU;
            string bfwavDirectory = GlobalDirectory.bfwavDirectoryU;
            string wavDirectory = GlobalDirectory.wavDirectoryU;

            Directory.CreateDirectory(barsDirectory);
            string[] barsFilepaths = Directory.GetFiles(barsDirectory, "*.bars");

            foreach (string barsFilepath in barsFilepaths)
            {
                string barsFile = Path.GetFileNameWithoutExtension(barsFilepath);
                string characterName = string.Empty;

                if (Utilities.StringStartsWithAny(barsFile, /*"MenuDriver_", */"SNDG_M_"))
                {
                    characterName = barsFile.Remove(0, 7);
                }
                else if (Utilities.StringStartsWithAny(barsFile, /*"OpenDriver_", */"SNDG_N_"))
                {
                    characterName = barsFile.Remove(0, 7);
                }
                else if (Utilities.StringStartsWithAny(barsFile, /*"Driver_", */"SNDG_"))
                {
                    characterName = barsFile.Remove(0, 5);
                }

                Directory.CreateDirectory(bfwavDirectory + characterName);
                SARC.extract(barsFilepath, bfwavDirectory + characterName);
            }
            ConvertBFWAVtoWAVForGeneration(bfwavDirectory, wavDirectory);
        }

        public static void ExtractDXBARS()
        {
            Utilities.progressValue = 0;
            Utilities.progressDesc = "Extracting BARS...";

            string barsDirectory = GlobalDirectory.barsDirectoryDX;
            string bfwavDirectory = GlobalDirectory.bfwavDirectoryDX;
            string wavDirectory = GlobalDirectory.wavDirectoryDX;

            Directory.CreateDirectory(barsDirectory);
            string[] barsFilepaths = Directory.GetFiles(barsDirectory, "*.bars");

            foreach (string barsFilepath in barsFilepaths)
            {
                string barsFile = Path.GetFileNameWithoutExtension(barsFilepath);
                string characterName = string.Empty;

                if (Utilities.StringStartsWithAny(barsFile, "MenuDriver_"))
                {
                    characterName = barsFile.Remove(0, 11);
                }
                else if (Utilities.StringStartsWithAny(barsFile, "Driver_"))
                {
                    characterName = barsFile.Remove(0, 7);
                }

                Directory.CreateDirectory(bfwavDirectory + characterName);
                
                BMETA Bmta = new BMETA();
                Bmta.load(barsFilepath);
                Bmta.unpack(bfwavDirectory + characterName);
                Bmta.unpackWav(wavDirectory + characterName);

                Utilities.progressValue++;
            }
            Utilities.progressDesc = "Extraction Complete! Please close this window.";
        }

        public static void ConvertBFWAVtoWAVForGeneration(string bfwavDirectory, string wavDirectory)
        {
            string[] driverFolders = Directory.GetDirectories(bfwavDirectory);

            foreach (string driverFolder in driverFolders)
            {
                string driverName = Path.GetFileName(driverFolder);
                string outputFolder = GlobalDirectory.VGAudioCli + "output/";

                string argument = $"-b -i {driverFolder} -o {outputFolder} --out-format wav";

                string exe = GlobalDirectory.VGAudioCli + "VGAudioCli.exe";
                Utilities.RunConsoleCommand(exe, argument, "");

                string[] wavFiles = Directory.GetFiles(outputFolder, "*.wav");
                foreach (string wavFile in wavFiles)
                {
                    string destination = $"{wavDirectory}{driverName}/{Path.GetFileName(wavFile)}";
                    Directory.CreateDirectory($"{wavDirectory}{driverName}/");
                    if (File.Exists(destination))
                    {
                        File.Delete(destination);
                    }
                    File.Move(wavFile, destination);
                }
            }
        }

        public static void ConvertBFWAVtoWAV(string bfwavDirectory, string wavDirectory)
        {
            string argument = $"-b -i {bfwavDirectory} -o {wavDirectory} --out-format wav";

            string exe = GlobalDirectory.VGAudioCli + "VGAudioCli.exe";
            Utilities.RunConsoleCommand(exe, argument, "");
        }

        public static bool ConvertWAVtoBFWAV(string filepath, string outputFolder, bool outputMessage = true)
        {
            string NW4F_WaveConverter = "tools/BFWAV to WAV/NW4F_WaveConverter.exe";
            if (File.Exists(NW4F_WaveConverter))
            {
                Utilities.RunConsoleCommand(NW4F_WaveConverter, $"-o {outputFolder}/{Path.GetFileNameWithoutExtension(filepath)}.bfwav {filepath}", "");
                return true;
            }
            else if (outputMessage)
            {
                MessageBox.Show("Converting from WAV to BFWAV has been disabled while I find a new solution for it.");
            }
            return false;
        }

        public static void ConvertFilesToBFWAV(string inputFolder, string outputFolder)
        {
            bool successful = false;
            //Convert WAV to BFWAV
            string[] wavFiles = Directory.GetFiles(inputFolder, "*.wav");
            if (wavFiles.Length > 0)
            {
                foreach (var wav in wavFiles)
                {
                    if (ConvertWAVtoBFWAV(wav, outputFolder, false))
                    {
                        successful = true;
                    }
                }

                if (!successful)
                {
                    MessageBox.Show("Converting from WAV to BFWAV has been disabled while I find a new solution for it.");
                }
            }

            //Extract BARS
            string[] barsFiles = Directory.GetFiles(inputFolder, "*.bars");
            if (barsFiles.Length > 1)
            {
                int driverParamCount = 0;
                int menuParamCount = 0;
                int unlockParamCount = 0;

                //for each BARS, cycle through stored _param files until a match is found. If another BARS has the same type (driver/menu/unlock) then fail.
                foreach (string barsFile in barsFiles)
                {
                    Uwizard.SARC.extract(barsFile, GlobalDirectory.paramCheckTempFolder);

                    string extractedParamFile = Directory.GetFiles(GlobalDirectory.paramCheckTempFolder, "*.bin")[0];
                    byte[] extractedParam = File.ReadAllBytes(extractedParamFile);

                    if (driverParamCount <= 1 && FindMatchingParams(extractedParamFile, GlobalDirectory.driverParamsDirectory, ref driverParamCount))
                    {
                        Utilities.ClearDirectory(GlobalDirectory.paramCheckTempFolder);
                        if (driverParamCount > 1)
                        {
                            MessageBox.Show($"Please only use one driver BARS file at a time. Only {Path.GetFileName(barsFile)} will be used");
                            continue;
                        }

                        Uwizard.SARC.extract(barsFile, outputFolder);
                        continue;
                    }
                    if (menuParamCount <= 1 && FindMatchingParams(extractedParamFile, GlobalDirectory.menuParamsDirectory, ref menuParamCount))
                    {
                        Utilities.ClearDirectory(GlobalDirectory.paramCheckTempFolder);
                        if (menuParamCount > 1)
                        {
                            MessageBox.Show("Please only use one menu BARS file at a time. Only {Path.GetFileName(barsFile)} will be used");
                            continue;
                        }

                        Uwizard.SARC.extract(barsFile, outputFolder);
                        continue;
                    }
                    if (unlockParamCount <= 1 && FindMatchingParams(extractedParamFile, GlobalDirectory.unlockParamsDirectory, ref unlockParamCount))
                    {
                        Utilities.ClearDirectory(GlobalDirectory.paramCheckTempFolder);
                        if (unlockParamCount > 1)
                        {
                            MessageBox.Show("Please only use one unlock BARS file at a time. Only {Path.GetFileName(barsFile)} will be used");
                            continue;
                        }

                        Uwizard.SARC.extract(barsFile, outputFolder);
                        continue;
                    }
                }

                //int sndgCount = 1;
                //if (Directory.GetFiles(inputFolder, "SNDG_M_*.bars").Length > 1)
                //{
                //    sndgCount++;
                //    throw new System.Exception("Please only use one of each type of BARS at a time");
                //}
                //if (Directory.GetFiles(inputFolder, "SNDG_B_*.bars").Length > 1)
                //{
                //    sndgCount++;
                //    throw new System.Exception("Please only use one of each type of BARS at a time");
                //}
                //if (Directory.GetFiles(inputFolder, "SNDG_*.bars").Length > sndgCount)
                //{
                //    throw new System.Exception("Please only use one of each type of BARS at a time");
                //}
            }
            else if (barsFiles.Length == 1)
            {
                Uwizard.SARC.extract(barsFiles[0], outputFolder);
            }

            string[] bfwavFiles = Directory.GetFiles(inputFolder, "*.bfwav");
            foreach (string bfwavFile in bfwavFiles)
            {
                File.Copy(bfwavFile, outputFolder + Path.GetFileName(bfwavFile));
            }
        }

        static bool FindMatchingParams(string extractedParamFile, string paramsFolder, ref int count)
        {
            foreach (string paramFile in Directory.GetFiles(paramsFolder))
            {
                //byte[] param = File.ReadAllBytes(paramFile);
                if (FileEquals(extractedParamFile, paramFile))
                {
                    count++;
                    return true;
                }
            }
            return false;
        }

        static bool FileEquals(string path1, string path2)
        {
            byte[] file1 = File.ReadAllBytes(path1);
            byte[] file2 = File.ReadAllBytes(path2);
            if (file1.Length == file2.Length)
            {
                for (int i = 0; i < file1.Length; i++)
                {
                    if (file1[i] != file2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
