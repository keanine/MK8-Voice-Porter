using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoiceTool
{
    class FileInfoData
    {
        public List<FileInfoElement> elements = new List<FileInfoElement>();

        public struct FileInfoElement
        {
            public string fileName;
            public string bfwavFileSize;
            public string wavFileSize;
            public string checksumMD5;

            public FileInfoElement(string fileName, string bfwavFileSize, string wavFileSize, string checksumMD5)
            {
                this.fileName = fileName;
                this.bfwavFileSize = bfwavFileSize;
                this.wavFileSize = wavFileSize;
                this.checksumMD5 = checksumMD5;
            }
        }

        public void AddElement(string fileName, string bfwavFileSize, string wavFileSize, string checksumMD5)
        {
            elements.Add(new FileInfoElement(fileName, bfwavFileSize, wavFileSize, checksumMD5));
        }
    }
}
