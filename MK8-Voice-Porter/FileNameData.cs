using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoicePorter
{
    public class FileNameData
    {
        public string character = "CHARACTER_NAME";
        public FileNameDataElement[] elements;

        [System.Serializable]
        public struct FileNameDataElement
        {
            public string userFriendlyName;
            public string wiiuName;
            public string switchName;
        }
    }
}
