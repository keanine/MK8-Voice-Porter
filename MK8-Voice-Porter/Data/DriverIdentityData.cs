using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoiceTool
{
    class DriverIdentityData
    {
        public string fileName;
        public string driverName;
        public string driverCode;
        public int voiceclipCount;
        public int elementCount;
        public List<DriverIdentityElement> elements;

        public DriverIdentityData(string fileName, string driverName, string driverCode, int voiceclipCount)
        {
            this.fileName = fileName;
            this.driverName = driverName;
            this.driverCode = driverCode;
            this.voiceclipCount = voiceclipCount;
            elements = new List<DriverIdentityElement>();
        }

        public struct DriverIdentityElement
        {
            public string userFriendlyName;
            public string uName;
            public string dxName;
            public string bfwavFileSize;
            public string checksumMD5;

            public DriverIdentityElement(string userFriendlyName, string uName, string dxName, string bfwavFileSize, string checksumMD5)
            {
                this.userFriendlyName = userFriendlyName;
                this.uName = uName;
                this.dxName = dxName;
                this.bfwavFileSize = bfwavFileSize;
                this.checksumMD5 = checksumMD5;
            }

            public bool IsMatch(string filename)
            {
                return filename == uName || filename == uName || filename == uName;
            }
        }

        public void AddElement(string userFriendlyName, string uName, string dxName, string bfwavFileSize, string checksumMD5)
        {
            elements.Add(new DriverIdentityElement(userFriendlyName, uName, dxName, bfwavFileSize, checksumMD5));

            elementCount++;
        }
    }
}
