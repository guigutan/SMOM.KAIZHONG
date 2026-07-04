using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Starter
{
    public class VersionInfo
    {
        public string ProductName { get; set; }
        public string Version { get; set; }
        public string ChangeInfo { get; set; }


        const string FILE_NAME = "local.ver";
        public static VersionInfo GetLocalVersion()
        {
            if (File.Exists(FILE_NAME)) 
            {
                string s = File.ReadAllText(FILE_NAME);
                return JsonConvert.DeserializeObject<VersionInfo>(s);
            }
            return new VersionInfo() { ProductName = "", Version = "1.0.0", ChangeInfo = "" };
        }

        public static void SaveLocalVersion(VersionInfo version)
        {
            File.WriteAllText(FILE_NAME, JsonConvert.SerializeObject(version));
        }
    }
}
