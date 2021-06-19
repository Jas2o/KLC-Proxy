using LibKaseya;
using Newtonsoft.Json;
using nucs.JsonSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCProxy {
    public class Settings : JsonSettings {
        public override string FileName { get; set; } = "KLCProxy-config.json"; //for loading and saving.
        public Settings() { }
        public Settings(string fileName) : base(fileName) { }

        public enum OnLiveConnectAction {
            Default, //Refer to RedirectToAlternative
            UseLiveConnect,
            UseAlternative,
            Prompt
        }

        //--

        //public bool UseMITM { get; set; } = false;
        public LaunchExtra Extra { get; set; } = LaunchExtra.None;
        public bool RedirectToAlternative { get; set; } = false;
        public OnLiveConnectAction OnLiveConnect { get; set; } = OnLiveConnectAction.Default;
        public bool ToastWhenOnline = false;
        public bool AddToSystemTray = false;
        public bool AlwaysOnTop = false;
        public string StartDisplay = "Default";
        public string StartDisplayFallback = "";
        public int StartCorner = 0;
    }
}
