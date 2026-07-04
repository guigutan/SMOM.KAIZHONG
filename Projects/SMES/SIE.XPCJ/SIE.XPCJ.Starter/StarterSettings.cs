using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Starter
{
    public class StarterSettings
    {
        public string ExePath { get; set; }
        public string ExeName { get; set; }
        public string VersionUrl { get; set; }
        public string FileUrl { get; set; }

        static StarterSettings _Instance = null;
        public static StarterSettings Instance
        {
            get {
                if (_Instance == null)
                {
                    string text = File.ReadAllText("StarterSettings.json");
                    _Instance = JsonConvert.DeserializeObject<StarterSettings>(text);

                    AppSettings.AppPath = _Instance.ExePath;

                    if (!_Instance.VersionUrl.StartsWith("http://") && !_Instance.VersionUrl.StartsWith("https://"))
                        _Instance.VersionUrl = AppSettings.Instance.AttachUrl.EndsWith("/") ? AppSettings.Instance.AttachUrl + _Instance.VersionUrl : AppSettings.Instance.AttachUrl + "/" + _Instance.VersionUrl;

                    if (!_Instance.FileUrl.StartsWith("http://") && !_Instance.FileUrl.StartsWith("https://"))
                        _Instance.FileUrl = AppSettings.Instance.AttachUrl.EndsWith("/") ? AppSettings.Instance.AttachUrl + _Instance.FileUrl : AppSettings.Instance.AttachUrl + "/" + _Instance.FileUrl;

                }
                return _Instance;
            }
        }
    }
}
