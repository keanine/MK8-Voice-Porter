using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MK8VoiceTool
{
    class DriverIdentityGenerator
    {
        public static void GenerateDriverIdentityData()
        {
            string fileInfoDirectoryU = GlobalDirectory.fileInfoDirectoryU;
            string fileInfoDirectoryDX = GlobalDirectory.fileInfoDirectoryDX;

            string[] fileInfoFilepathsU = Directory.GetFiles(fileInfoDirectoryU);

            for (int i = 0; i < fileInfoFilepathsU.Length; i++)
            {
                string driverFile = Path.GetFileName(fileInfoFilepathsU[i]);

                string fileInfoPathU = fileInfoFilepathsU[i];
                string fileInfoPathDX = fileInfoDirectoryDX + driverFile;

                //Deserialise FileInfo
                FileInfoData dataU = JsonConvert.DeserializeObject<FileInfoData>(File.ReadAllText(fileInfoPathU));
                FileInfoData dataDX = JsonConvert.DeserializeObject<FileInfoData>(File.ReadAllText(fileInfoPathDX));

                //Get driverCode
                string driverName = string.Empty;
                string driverCode = string.Empty;
                string bfwavName = Path.GetFileNameWithoutExtension(driverFile);

                foreach (FileInfoData.FileInfoElement elementDX in dataDX.elements)
                {
                    if (driverName == string.Empty && elementDX.fileName.StartsWith("pSE_HORN", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] words = elementDX.fileName.Split('_');
                        driverName = words[2];
                        if (elementDX.fileName.Contains("Mii"))
                        {
                            driverName += "_" + words[3];
                        }
                        continue;
                    }
                    if (driverCode == string.Empty && elementDX.fileName.StartsWith("pVO_", StringComparison.OrdinalIgnoreCase) && !elementDX.fileName.StartsWith("pVO_M_", StringComparison.OrdinalIgnoreCase))
                    {
                        driverCode = elementDX.fileName.Split('_')[1];
                        continue;
                    }
                }

                //Create new identity
                DriverIdentityData identityData = new DriverIdentityData(bfwavName, driverName, driverCode, dataDX.elements.Count);

                //Find matching checksums and add them to the same element in the identity with a user friendly name
                foreach (FileInfoData.FileInfoElement elementU in dataU.elements)
                {
                    foreach (FileInfoData.FileInfoElement elementDX in dataDX.elements)
                    {
                        if (elementU.checksumMD5 == elementDX.checksumMD5)
                        {
                            string friendlyName = AliasData.GetFriendlyVoiceAlias(elementDX.fileName, identityData.driverName, identityData.driverCode);

                            identityData.AddElement(friendlyName, elementU.fileName, elementDX.fileName, elementU.bfwavFileSize, elementU.checksumMD5);
                        }
                    }

                    //Add exception for unlocking, as these files cannot be found in the DX version
                    if (elementU.fileName.StartsWith("pVO_N_"))
                    {
                        string friendlyName = AliasData.GetFriendlyVoiceAlias(elementU.fileName, identityData.driverName, identityData.driverCode);
                        identityData.AddElement(friendlyName, elementU.fileName, elementU.fileName, elementU.bfwavFileSize, elementU.checksumMD5);
                        identityData.voiceclipCount++;
                    }
                }


                if (identityData.elementCount != identityData.voiceclipCount)
                {
                    throw new System.Exception($"{ identityData.driverName }: The number of elements ({ identityData.elementCount }) did not match the number of voice clips ({ identityData.voiceclipCount })");
                }

                //Serialise the final JSON file for use in voice porting!
                string json = JsonConvert.SerializeObject(identityData, Formatting.Indented);
                StreamWriter writer = new StreamWriter($"{GlobalDirectory.identitiesDirectory}{driverFile}");
                writer.Write(json);
                writer.Close();
            }
        }
    }
}
