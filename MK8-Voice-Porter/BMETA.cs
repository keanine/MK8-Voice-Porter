using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;
using VGAudio.Containers.NintendoWare;
using VGAudio.Containers.Wave;

namespace BARSViewer
{
    public class BMETA
    {

        public header hdr = new header();
        public List<byte[]> amtaData = new List<byte[]>();
        public List<byte[]> audioData = new List<byte[]>();
        public List<STRG> strgList = new List<STRG>();
        public List<string> audioIdntr = new List<string>();

        public class header
        {
            public char[] id;
            public UInt32 size;
            public UInt16 BOM;
            public byte unknown1;
            public byte unknown2;
            public UInt32 amtaCount;
            public List<UInt32> hashes = new List<UInt32>();
            public List<UInt32> offsets = new List<UInt32>();
            public List<UInt32> audioOffsets = new List<UInt32>();
            public List<UInt32> amtaOffsets = new List<UInt32>();

            public void parseHeader(BinaryReader br)
            {
                id = br.ReadChars(4);
                if (new string(id) != "BARS") throw new Exception("Invalid file.");
                size = br.ReadUInt32();
                BOM = br.ReadUInt16();
                unknown1 = br.ReadByte();
                unknown2 = br.ReadByte();
                amtaCount = br.ReadUInt32();
                for (int i = 0; i < amtaCount; i++) hashes.Add(br.ReadUInt32());
                UInt32 temp = br.ReadUInt32();
                offsets.Add(temp);
                amtaOffsets.Add(temp);
                while (br.BaseStream.Position < offsets[0])
                {
                    audioOffsets.Add(br.ReadUInt32());
                    amtaOffsets.Add(br.ReadUInt32());
                }
            }
        }

        public class AMTA
        {
            public char[] id;
            public UInt16 BOM;
            public UInt16 unknown1;
            public UInt32 size;
            public UInt32 dataOffset;
            public UInt32 markOffset;
            public UInt32 extOffset;
            public UInt32 strgOffset; // We need this to grab FWAV names

            public void parseAMTA(BinaryReader br)
            {
                id = br.ReadChars(4);
                if (new string(id) != "AMTA") throw new Exception("AMTA chunk doesn't match. Something isn't right here..");
                BOM = br.ReadUInt16();
                unknown1 = br.ReadUInt16();
                size = br.ReadUInt32();
                dataOffset = br.ReadUInt32();
                markOffset = br.ReadUInt32();
                extOffset = br.ReadUInt32();
                strgOffset = br.ReadUInt32();
            }
        }

        public class DATA
        {
            // 32 bit = 4 byte
            // 64 bit = 8 byte
            // 16 bit = 2 byte
            public char[] id;
            public UInt32 size;

            // There's no point in reading the unknowns

            // Floating-point stuff
            public float[] f1;
            public UInt32[] u1;

            public void parseDATA(BinaryReader br)
            {
                id = br.ReadChars(4);
                if (new string(id) != "DATA") throw new Exception("DATA chunk is invalid.");
                size = br.ReadUInt32();
                // Skips the 36 bytes of unknowns
                br.BaseStream.Position += 36;

                f1 = new float[8];
                u1 = new UInt32[8];
                for (int i = 0; i < 8; i++)
                {
                    f1[i] = br.ReadSingle();
                    u1[i] = br.ReadUInt32();
                }
            }
        }

        public class MARK
        {
            public char[] id;
            public UInt32 size;
            public UInt32 entries;
            public List<entry> markEntries = new List<entry>();

            public class entry
            {
                public UInt32 index;
                public UInt32 unk1;
                public UInt32 unk2;
                public UInt32 unk3;
            }

            public void parseMARK(BinaryReader br)
            {
                id = br.ReadChars(4);
                if (new string(id) != "MARK") throw new Exception("MARK chunk is invalid.");
                size = br.ReadUInt32();
                entries = br.ReadUInt32();
                if (entries != 0)
                {
                    for (int i = 0; i < entries; i++)
                    {
                        entry ent = new entry();
                        ent.index = br.ReadUInt32();
                        ent.unk1 = br.ReadUInt32();
                        ent.unk2 = br.ReadUInt32();
                        ent.unk3 = br.ReadUInt32();
                        markEntries.Add(ent);
                    }
                }
            }
        }

        public class EXT_
        {
            public char[] id;
            public UInt32 size;
            public UInt32 entries;
            public List<entry> extEntries = new List<entry>();

            public class entry
            {
                public UInt32 unk1;
                public UInt32 unk2;
            }

            public void parseEXT_(BinaryReader br)
            {
                id = br.ReadChars(4);
                if (new string(id) != "EXT_") throw new Exception("EXT_ chunk is invalid.");
                size = br.ReadUInt32();
                entries = br.ReadUInt32();
                if (entries != 0)
                {
                    for (int i = 0; i < entries; i++)
                    {
                        entry ent = new entry();
                        ent.unk1 = br.ReadUInt32();
                        ent.unk2 = br.ReadUInt32();
                        extEntries.Add(ent);
                    }
                }
            }
        }

        public class STRG
        {
            public char[] id;
            public UInt32 stringSize;
            public char[] fwavName;
            public string name;

            public void parseSTRG(BinaryReader br)
            {
                id = br.ReadChars(4);
                if (new string(id) != "STRG") throw new Exception("STRG chunk is invalid.");
                stringSize = br.ReadUInt32();
                fwavName = br.ReadChars((int)stringSize);
                char[] sep = { (char)0x00 };
                string temp = new string(fwavName);
                string[] temp2 = temp.Split(sep);
                name = temp2[0];
            }
        }

        public void load(string file)
        {
            byte[] f = File.ReadAllBytes(file);
            BinaryReader br = new BinaryReader(new MemoryStream(f, 0, f.Length));
            hdr.parseHeader(br);
            readEntries(br);

            for (int i = 0; i < amtaData.Count; i++)
            {
                f = amtaData[i];
                br = new BinaryReader(new MemoryStream(f, 0, f.Length));
                AMTA amta = new AMTA();
                amta.parseAMTA(br);
                DATA data = new DATA();
                data.parseDATA(br);
                MARK mark = new MARK();
                mark.parseMARK(br);
                EXT_ ext = new EXT_();
                ext.parseEXT_(br);
                STRG strg = new STRG();
                strg.parseSTRG(br);
                strgList.Add(strg);
                br.Close();
            }
        }

        private void readEntries(BinaryReader br)
        {
            for (int i = 0; i < hdr.audioOffsets.Count; i++)
            {
                br.BaseStream.Seek(hdr.amtaOffsets[i], SeekOrigin.Begin);
                headerCheck(br);

                br.BaseStream.Seek(hdr.audioOffsets[i], SeekOrigin.Begin);
                headerCheck(br);
            }
            br.Close();
        }

        private void headerCheck(BinaryReader br)
        {
            uint size;
            char[] temp = br.ReadChars(4);
            string temp2 = new string(temp);
            switch (temp2)
            {
                case "AMTA":
                    br.BaseStream.Seek(0x4, SeekOrigin.Current);
                    size = br.ReadUInt32();
                    br.BaseStream.Seek(-0xC, SeekOrigin.Current);
                    amtaData.Add(br.ReadBytes((int)size));
                    break;
                case "FWAV":
                    br.BaseStream.Seek(0x8, SeekOrigin.Current);
                    size = br.ReadUInt32();
                    br.BaseStream.Seek(-0x10, SeekOrigin.Current);
                    audioData.Add(br.ReadBytes((int)size));
                    audioIdntr.Add(".bfwav");
                    break;
                case "FSTP":
                    br.BaseStream.Seek(0x8, SeekOrigin.Current);
                    size = br.ReadUInt32();
                    br.BaseStream.Seek(-0x10, SeekOrigin.Current);
                    audioData.Add(br.ReadBytes((int)size));
                    audioIdntr.Add(".bfstp");
                    break;
                default: throw new Exception("Unknown chunk: " + temp2);
            }
        }

        public void unpack(string file)
        {
            Directory.CreateDirectory(file);
            for (int i = 0; i < audioData.Count; i++)
            {
                FileStream f = File.Create(file + "/" + strgList[i].name + audioIdntr[i]);
                f.Write(audioData[i], 0, audioData[i].Length);
                f.Close();
            }
        }
        public void unpackWav(string file)
        {
            Directory.CreateDirectory(file);
            for (int i = 0; i < amtaData.Count; i++)
            {
                if (audioIdntr[i] == ".bfwav")
                {
                    FileStream f = File.Create(file + "/" + strgList[i].name + ".wav");
                    BCFstmReader reader = new BCFstmReader();
                    WaveWriter writer = new WaveWriter();
                    VGAudio.Formats.AudioData convertedWav = reader.Read(audioData[i]);
                    writer.WriteToStream(convertedWav, f);
                    f.Close();
                }
            }
        }
    }
}