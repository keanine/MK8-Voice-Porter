using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoicePorter
{
    class AudioComparison
    {
        static Tuple<string, string>[] userFriendlyNames =
        {
            Tuple.Create("RANK_TOP_1", "pVO_CHARACTERCODE_GOL_TOP"),
            Tuple.Create("RANK_TOP_2", "pVO_CHARACTERCODE_GOL_TOP2"),
            Tuple.Create("RANK_TOP_END", "pVO_CHARACTERCODE_GOL_TOP_END"),

            Tuple.Create("RANK_GOOD_1", "pVO_CHARACTERCODE_GOL_GOD"),
            Tuple.Create("RANK_GOOD_2", "pVO_CHARACTERCODE_GOL_GOD2"),
            Tuple.Create("RANK_GOOD_END", "pVO_CHARACTERCODE_GOL_GOD_END"),

            Tuple.Create("RANK_BAD_1", "pVO_CHARACTERCODE_GOL_BAD"),
            Tuple.Create("RANK_BAD_2", "pVO_CHARACTERCODE_GOL_BAD2"),
            Tuple.Create("RANK_BAD_END", "pVO_CHARACTERCODE_GOL_BAD_END"),

            Tuple.Create("OVERTAKE_1", "pVO_CHARACTERCODE_OVTAK"),
            Tuple.Create("OVERTAKE_2", "pVO_CHARACTERCODE_OVTAK2"),
            Tuple.Create("OVERTAKE_3", "pVO_CHARACTERCODE_OVTAK3"),
            Tuple.Create("OVERTAKE_4", "pVO_CHARACTERCODE_OVTAK4"),
            Tuple.Create("OVERTAKE_END", "pVO_CHARACTERCODE_OVTAK_END"),

            Tuple.Create("DASH_1", "pVO_CHARACTERCODE_DSH"),
            Tuple.Create("DASH_2", "pVO_CHARACTERCODE_DSH2"),
            Tuple.Create("DASH_3", "pVO_CHARACTERCODE_DSH3"),
            Tuple.Create("DASH_END", "pVO_CHARACTERCODE_DSH_END"),

            Tuple.Create("DASH_S_1", "pVO_CHARACTERCODE_DSH_S"),
            Tuple.Create("DASH_S_2", "pVO_CHARACTERCODE_DSH_S2"),
            Tuple.Create("DASH_S_3", "pVO_CHARACTERCODE_DSH_S3"),
            Tuple.Create("DASH_S_END", "pVO_CHARACTERCODE_DSH_S_END"),

            Tuple.Create("ITEM_PUT", "pVO_CHARACTERCODE_ITM_PUT"),
            Tuple.Create("ITEM_PUT_END", "pVO_CHARACTERCODE_ITM_PUT_END"),

            Tuple.Create("ITEM_GESSO", "pVO_CHARACTERCODE_ITM_GESSO"),
            Tuple.Create("ITEM_GESSO_END", "pVO_CHARACTERCODE_ITM_GESSO_END"),

            Tuple.Create("ITEM_THROW", "pVO_CHARACTERCODE_ITM_TRW"),
            Tuple.Create("ITEM_THROW_END", "pVO_CHARACTERCODE_ITM_TRW_END"),

            Tuple.Create("ITEM_SCES_1", "pVO_CHARACTERCODE_ITM_SCES"),
            Tuple.Create("ITEM_SCES_2", "pVO_CHARACTERCODE_ITM_SCES2"),
            Tuple.Create("ITEM_SCES_END", "pVO_CHARACTERCODE_ITM_SCES_END"),

            Tuple.Create("ITEM_STAR", "pVO_CHARACTERCODE_ITM_STAR"),
            Tuple.Create("ITEM_STAR_END", "pVO_CHARACTERCODE_ITM_STAR_END"),

            Tuple.Create("CMB_LAVA", "pVO_CHARACTERCODE_CMB_LAVA"),
            Tuple.Create("CMB_LAVA_END", "pVO_CHARACTERCODE_CMB_LAVA_END"),

            Tuple.Create("FALL_IN_LAVA", "pVO_CHARACTERCODE_FALL_LAVA"),
            Tuple.Create("FALL_IN_LAVA_END", "pVO_CHARACTERCODE_FALL_LAVA_END"),

            Tuple.Create("CMB_WATER", "pVO_CHARACTERCODE_CMB_WATER"),
            Tuple.Create("CMB_WATER_END", "pVO_CHARACTERCODE_CMB_WATER_END"),

            Tuple.Create("FALL_IN_WATER", "pVO_CHARACTERCODE_FALL_WATER"),
            Tuple.Create("FALL_IN_WATER_END", "pVO_CHARACTERCODE_FALL_WATER_END"),

            Tuple.Create("STRM_1", "pVO_CHARACTERCODE_STRM"),
            Tuple.Create("STRM_2", "pVO_CHARACTERCODE_STRM2"),
            Tuple.Create("STRM_END", "pVO_CHARACTERCODE_STRM_END"),

            Tuple.Create("DAMAGE_S", "pVO_CHARACTERCODE_DMG_S"),
            Tuple.Create("DAMAGE_S_END", "pVO_CHARACTERCODE_DMG_S_END"),

            Tuple.Create("DAMAGE_BOUND", "pVO_CHARACTERCODE_DMG_BOUND"),
            Tuple.Create("DAMAGE_BOUND_END", "pVO_CHARACTERCODE_DMG_BOUND_END"),

            Tuple.Create("DAMAGE_SPIN", "pVO_CHARACTERCODE_DMG_SPN"),
            Tuple.Create("DAMAGE_SPIN_END", "pVO_CHARACTERCODE_DMG_SPN_END"),

            Tuple.Create("DAMAGE_CRASH", "pVO_CHARACTERCODE_DMG_CRSH"),
            Tuple.Create("DAMAGE_CRASH_END", "pVO_CHARACTERCODE_DMG_CRSH_END"),

            Tuple.Create("FALL_SHORT", "pVO_CHARACTERCODE_FALL_SHORT"),
            Tuple.Create("FALL_SHORT_END", "pVO_CHARACTERCODE_FALL_SHORT_END"),

            Tuple.Create("HOVER_DASH_1", "pVO_CHARACTERCODE_HOVER_DASH"),
            Tuple.Create("HOVER_DASH_2", "pVO_CHARACTERCODE_HOVER_DASH2"),
            Tuple.Create("HOVER_DASH_END", "pVO_CHARACTERCODE_HOVER_DASH_END"),

            Tuple.Create("START_HOVER_1", "pVO_CHARACTERCODE_START_HOVER"),
            Tuple.Create("START_HOVER_2", "pVO_CHARACTERCODE_START_HOVER2"),
            Tuple.Create("START_HOVER_END", "pVO_CHARACTERCODE_START_HOVER_END"),

            Tuple.Create("HOVER_CHARGE_1", "pVO_CHARACTERCODE_HOVER_CHARGE"),
            Tuple.Create("HOVER_CHARGE_2", "pVO_CHARACTERCODE_HOVER_CHARGE2"),
            Tuple.Create("HOVER_CHARGE_END", "pVO_CHARACTERCODE_HOVER_CHARGE_END"),

            Tuple.Create("START_GLIDE_1", "pVO_CHARACTERCODE_START_GLIDE"),
            Tuple.Create("START_GLIDE_2", "pVO_CHARACTERCODE_START_GLIDE2"),
            Tuple.Create("START_GLIDE_END", "pVO_CHARACTERCODE_START_GLIDE_END"),

            Tuple.Create("STR_FAIL", "pVO_CHARACTERCODE_STR_FAIL"),
            Tuple.Create("STR_FAIL_END", "pVO_CHARACTERCODE_STR_FAIL_END"),

            Tuple.Create("JP_ACT_1", "pVO_CHARACTERCODE_JP_ACT"),
            Tuple.Create("JP_ACT_2", "pVO_CHARACTERCODE_JP_ACT2"),
            Tuple.Create("JP_ACT_END", "pVO_CHARACTERCODE_JP_ACT_END"),

            Tuple.Create("JP_ACT_SA_1", "pVO_CHARACTERCODE_JP_ACT_SA"),
            Tuple.Create("JP_ACT_SA_2", "pVO_CHARACTERCODE_JP_ACT_SA2"),
            Tuple.Create("JP_ACT_SA_END", "pVO_CHARACTERCODE_JP_ACT_SA_END"),

            Tuple.Create("JP_ACT_SB_1", "pVO_CHARACTERCODE_JP_ACT_SB"),
            Tuple.Create("JP_ACT_SB_2", "pVO_CHARACTERCODE_JP_ACT_SB2"),
            Tuple.Create("JP_ACT_SB_END", "pVO_CHARACTERCODE_JP_ACT_SB_END"),

            Tuple.Create("KT_JPACT", "pSE_CHARACTERCODE_KT_JPACT"),

            Tuple.Create("HORN_OFF", "pSE_HORN_CHARACTERNAME_OFF"),
            Tuple.Create("HORN_ON", "pSE_HORN_CHARACTERNAME_ON"),
            Tuple.Create("HORN_ON_SP", "pSE_HORN_CHARACTERNAME_ON_SP")
        };
        static List<Tuple<string, string>> unusedAliases = new List<Tuple<string, string>>();

        private ChecksumData uChecksumData;

        public string GetMD5(string audioFilePath)
        {
            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "GetMD5.bat";
            p.StartInfo.Arguments = $"{audioFilePath} MD5";
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            // Read the output stream first and then wait.
            p.StandardOutput.ReadLine();
            p.StandardOutput.ReadLine();
            p.StandardOutput.ReadLine();
            string output = p.StandardOutput.ReadLine();
            p.WaitForExit();
            return output;
        }

        public void GenerateUChecksumData(DataGenerationWindow window)
        {
            string[] characterFolders = Directory.GetDirectories("./MK8_WAV_files/");

            foreach (string characterFolder in characterFolders)
            {
                List<string> uFiles = new List<string>();
                List<string> dxFiles = new List<string>();
                uFiles.AddRange(Directory.GetFiles(characterFolder + "/U/"));
                dxFiles.AddRange(Directory.GetFiles(characterFolder + "/DX/"));

                uChecksumData = new ChecksumData();
                uChecksumData.platform = "U";
                uChecksumData.elements = new FileWithChecksum[uFiles.Count];

                //Generate the checksum and store the results in a list of FileWithChecksum
                for (int u = 0; u < uFiles.Count; u++)
                {
                    string uMD5 = GetMD5(uFiles[u]);
                    uChecksumData.elements[u] = new FileWithChecksum(uFiles[u], Path.GetFileNameWithoutExtension(uFiles[u]), uMD5);
                }

                if (!Directory.Exists("data/checksums/"))
                {
                    Directory.CreateDirectory("data/checksums/");
                }

                string jsonString = JsonConvert.SerializeObject(uChecksumData, Formatting.Indented);
                StreamWriter writer = new StreamWriter("data/checksums/" + Path.GetFileNameWithoutExtension(characterFolder) + ".json");
                writer.Write(jsonString);
                writer.Close();
            }
        }

        public void GenerateJSONData(DataGenerationWindow window)
        {
            DateTime startTime = DateTime.Now;

            window.textBlock_generationOutput.Text = "Matching Files...";

            string[] characterFolders = Directory.GetDirectories("./MK8_WAV_files/");

            foreach (string characterFolder in characterFolders)
            {
                StreamReader reader = new StreamReader("data/checksums/" + Path.GetFileNameWithoutExtension(characterFolder) + ".json");
                uChecksumData = JsonConvert.DeserializeObject<ChecksumData>(reader.ReadToEnd());
                reader.Close();

                FileNameData generatedData = new FileNameData();

                List<string> dxFiles = new List<string>();
                dxFiles.AddRange(Directory.GetFiles(characterFolder + "/"));

                string characterCode = string.Empty;
                string characterName = string.Empty;

                //Get the code and name for the current character
                for (int dx = 0; dx < dxFiles.Count; dx++)
                {
                    string file = Path.GetFileNameWithoutExtension(dxFiles[dx]);
                    if (characterCode == string.Empty)
                    {
                        if (file.StartsWith("pVO"))
                        {
                            string[] words = file.Split('_');
                            characterCode = words[1];
                        }
                    }
                    if (characterName == string.Empty)
                    {
                        if (file.StartsWith("pSE_HORN"))
                        {
                            string[] words = file.Split('_');
                            characterName = words[2];
                        }
                    }

                    if (characterName != string.Empty && characterCode != string.Empty)
                    {
                        break;
                    }
                }

                generatedData.character = characterName;
                generatedData.elements = new FileNameData.FileNameDataElement[81];
                int currentElement = 0;

                List<FileWithChecksum> dxFileChecksums = new List<FileWithChecksum>();

                //Generate the checksum and store the results in a list of FileWithChecksum
                for (int dx = 0; dx < dxFiles.Count; dx++)
                {
                    string dxMD5 = GetMD5(dxFiles[dx]);
                    dxFileChecksums.Add(new FileWithChecksum(dxFiles[dx], Path.GetFileNameWithoutExtension(dxFiles[dx]), dxMD5));
                }

                unusedAliases.Clear();
                unusedAliases.AddRange(userFriendlyNames);

                //Compare the checksum of the DX files against the U files and place into the generated data elements
                for (int dx = 0; dx < dxFileChecksums.Count; dx++)
                {
                    for (int u = 0; u < uChecksumData.elements.Length; u++)
                    {
                        if (dxFileChecksums[dx].checksumMD5 == uChecksumData.elements[u].checksumMD5)
                        {
                            generatedData.elements[currentElement].wiiuName = uChecksumData.elements[u].fileName;
                            generatedData.elements[currentElement].switchName = dxFileChecksums[dx].fileName;
                            generatedData.elements[currentElement].userFriendlyName = GetUserFriendlyName(characterCode, characterName, dxFileChecksums[dx].fileName);

                            currentElement++;
                            break;
                        }
                    }
                }

                ////Deal with any stragglers that couldn't find a match
                for (int n = 0; n < unusedAliases.Count; n++)
                {
                    string editedItem2 = unusedAliases[n].Item2.Replace("CHARACTERCODE", characterCode);
                    editedItem2 = editedItem2.Replace("CHARACTERNAME", characterName);

                    generatedData.elements[currentElement].switchName = editedItem2;
                    generatedData.elements[currentElement].userFriendlyName = unusedAliases[n].Item1;
                    currentElement++;
                }

                //Save the generated data into a JSON file
                SaveData(generatedData);

            }

            DateTime finishTime = DateTime.Now;

            window.textBlock_generationOutput.Text = "Completed in " + finishTime.Subtract(startTime).TotalMinutes + " minutes!";
        }

        public static string GetUserFriendlyName(string characterCode, string characterName, string dxName)
        {
            for (int n = 0; n < unusedAliases.Count; n++)
            {
                Tuple<string, string> pair = unusedAliases[n];

                string editedItem2 = pair.Item2.Replace("CHARACTERCODE", characterCode);
                editedItem2 = editedItem2.Replace("CHARACTERNAME", characterName);

                string editedDxName = dxName;
                if (editedItem2 == editedDxName)
                {
                    unusedAliases.RemoveAt(n);
                    return pair.Item1;
                }
            }

            return string.Empty;
        }

        public void SaveData(FileNameData data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            StreamWriter writer = new StreamWriter($"data/generated_{data.character}_data.json");
            writer.Write(json);
            writer.Close();
        }
    }
}
