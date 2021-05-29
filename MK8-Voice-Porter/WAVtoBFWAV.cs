using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGAudio.Containers.NintendoWare;
using VGAudio.Containers.Wave;
using static BARSViewer.BMETA;

namespace MK8VoiceTool
{
    class WAVtoBFWAV
    {
        public static void ConvertWAVtoBFWAV(string input, string output)
        {
            byte[] fileAudioData = File.ReadAllBytes(input);
            string fileAudioIdntr = ".wav";
            STRG fileStrg = new STRG();
            fileStrg.name = Path.GetFileNameWithoutExtension(input);

            if (fileAudioIdntr == ".wav")
            {
                FileStream f = File.Create(output);
                WaveReader reader = new WaveReader();
                BCFstmWriter writer = new BCFstmWriter(NwTarget.Cafe);
                VGAudio.Formats.AudioData convertedWav = reader.Read(fileAudioData);
                writer.WriteToStream(convertedWav, f);
                f.Close();
            }
        }

        /*
        public static StreamInfo ReadBfwav(BinaryReader reader, NwVersion version)
        {
            var info = new StreamInfo();
            info.Codec = (NwCodec)reader.ReadByte();
            info.Looping = reader.ReadBoolean();
            reader.BaseStream.Position += 2;
            info.SampleRate = reader.ReadInt32();
            info.LoopStart = reader.ReadInt32();
            info.SampleCount = reader.ReadInt32();

            if (Common.IncludeUnalignedLoopWave(version))
            {
                info.LoopStartUnaligned = reader.ReadInt32();
            }
            else
            {
                reader.BaseStream.Position += 4;
            }

            //Peek at the number of entries in the reference table
            info.ChannelCount = reader.ReadInt32();
            reader.BaseStream.Position -= 4;
            return info;
        }
        */
    }
}
