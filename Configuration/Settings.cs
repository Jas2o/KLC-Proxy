using LibKaseya;
using Newtonsoft.Json;
using nucs.JsonSettings;

namespace KLC_Proxy {
    public class Settings : JsonSettings {
        public override string FileName { get; set; } = "KLC-Proxy-config.json"; //for loading and saving.
        public Settings() { }
        public Settings(string fileName) : base(fileName) { }

        public enum OnLiveConnectAction {
            Default, //Refer to RedirectToAlternative
            UseLiveConnect,
            UseAlternative,
            Prompt
        }

        //--

        public LaunchExtra Extra { get; set; } = LaunchExtra.None;
        public bool RedirectToAlternative { get; set; } = false;
        public OnLiveConnectAction OnLiveConnect { get; set; } = OnLiveConnectAction.Default;
        public OnLiveConnectAction OnOneClick { get; set; } = OnLiveConnectAction.Default;
        public OnLiveConnectAction OnNativeRDP { get; set; } = OnLiveConnectAction.Default;
        public bool OverrideRCSharedtoLC { get; set; } = false;
        //public bool OverrideAltCanary { get; set; } = false;

        public bool OverrideAltCharm { get; set; } = false;

        public bool ToastWhenOnline = false;
        public bool AddToSystemTray = false;
        public bool AlwaysOnTop = false;
        public string StartDisplay = "Default";
        public string StartDisplayFallback = "";
        public int StartCorner = 0;
        public bool ShowAgentsVSA = false;

        [JsonIgnore] public ConfigureHandler.ProxyState ProxyState = ConfigureHandler.ProxyState.Disabled;
        [JsonIgnore] public bool RedirectDebug = false;
    }
}
