using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static void ConvertWAVtoBFWAV(string filepath, string outputFolder)
        {
            string NW4F_WaveConverter = "tools/BFWAVtoWAV/NW4F_WaveConverter.exe";
            Utilities.RunConsoleCommand(NW4F_WaveConverter, $"-o {outputFolder}/{Path.GetFileNameWithoutExtension(filepath)}.bfwav {filepath}", "");
        }

        public static void ConvertFilesToBFWAV(string inputFolder, string outputFolder)
        {
            //Convert WAV to BFWAV
            string[] wavFiles = Directory.GetFiles(inputFolder, "*.wav");
            foreach (var wav in wavFiles)
            {
                ConvertWAVtoBFWAV(wav, outputFolder);
            }

            //Extract BARS
            string[] barsFiles = Directory.GetFiles(inputFolder, "*.bars");
            if (barsFiles.Length > 1)
            {
                throw new System.Exception("Multiple BARS files found in the input folder. Please only use one.");
            }
            else if (barsFiles.Length == 1)
            {
                Uwizard.SARC.extract(barsFiles[0], outputFolder);
            }
        }
    }
}
