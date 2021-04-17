using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoicePorter
{
    class ChecksumData
    {
        public FileWithChecksum[] elements;
        public string platform;
    }

    public struct FileWithChecksum
    {
        public string fullFilePath;
        public string fileName;
        public string checksumMD5;

        public FileWithChecksum(string fullFilePath, string fileName, string checksumMD5)
        {
            this.fullFilePath = fullFilePath;
            this.fileName = fileName;
            this.checksumMD5 = checksumMD5;
        }
    }
}
