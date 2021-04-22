using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoiceTool
{
    class GlobalDirectory
    {
        public static string VGAudioCli = "tools/VGAudioCli/";

        public static string appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/MK8VoiceTool/";

        public static string barsDirectoryU =  appdataFolder + "data/audio/bars/u/";
        public static string barsDirectoryDX = appdataFolder + "data/audio/bars/dx/";

        public static string bfwavDirectoryU = appdataFolder + "data/audio/bfwav/u/";
        public static string bfwavDirectoryDX = appdataFolder + "data/audio/bfwav/dx/";

        public static string wavDirectoryU = appdataFolder + "data/audio/wav/u/";
        public static string wavDirectoryDX = appdataFolder + "data/audio/wav/dx/";

        public static string fileInfoDirectoryU = appdataFolder + "data/file_info/u/";
        public static string fileInfoDirectoryDX = appdataFolder + "data/file_info/dx/";

        public static string inputFolder = "files/input/";
        public static string outputFolder = "files/output/";
        public static string tempFolder = appdataFolder + "temp/";

        public static string wavTempFolder = tempFolder + "wav/";
        public static string bfwavTempFolder = tempFolder + "bfwav/";
        public static string friendlyTempFolder = tempFolder + "friendly/";
        public static string finalTempFolder = tempFolder + "final/";
        public static string paramCheckTempFolder = tempFolder + "param_check/";

        public static string identitiesDirectory = "data/character_identities/";

        public static string driverParamsDirectory = appdataFolder + "data/params/driver/";
        public static string menuParamsDirectory = appdataFolder + "data/params/driver_menu/";
        public static string unlockParamsDirectory = appdataFolder + "data/params/driver_open/";

        public static void RegenerateAllDirectories()
        {
            Directory.CreateDirectory(inputFolder);
            Directory.CreateDirectory(outputFolder);

            Directory.CreateDirectory(identitiesDirectory);

            //RegenerateDevFolders();
            RegenerateTempFolders();
            RegenerateParamFolders();
        }

        public static void RegenerateTempFolders()
        {
            Directory.CreateDirectory(wavTempFolder);
            Directory.CreateDirectory(bfwavTempFolder);
            Directory.CreateDirectory(friendlyTempFolder);
            Directory.CreateDirectory(finalTempFolder);
            Directory.CreateDirectory(paramCheckTempFolder);
        }

        public static void RegenerateParamFolders()
        {
            Directory.CreateDirectory(driverParamsDirectory);
            Directory.CreateDirectory(menuParamsDirectory);
            Directory.CreateDirectory(unlockParamsDirectory);
        }

        public static void RegenerateDevFolders()
        {
            Directory.CreateDirectory(barsDirectoryU);
            Directory.CreateDirectory(barsDirectoryDX);

            Directory.CreateDirectory(bfwavDirectoryU);
            Directory.CreateDirectory(bfwavDirectoryDX);

            Directory.CreateDirectory(wavDirectoryU);
            Directory.CreateDirectory(wavDirectoryDX);

            Directory.CreateDirectory(fileInfoDirectoryU);
            Directory.CreateDirectory(fileInfoDirectoryDX);
        }

        public static void ClearTempFolders()
        {
            Directory.Delete(tempFolder, true);
            RegenerateTempFolders();
        }

        public static void ClearParamFolders()
        {
            foreach (string file in Directory.GetFiles(driverParamsDirectory))
            {
                File.Delete(file);
            }
            foreach (string file in Directory.GetFiles(menuParamsDirectory))
            {
                File.Delete(file);
            }
            foreach (string file in Directory.GetFiles(unlockParamsDirectory))
            {
                File.Delete(file);
            }
            RegenerateParamFolders();
        }
    }
}
