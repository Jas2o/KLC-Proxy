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

        //--

        public bool UseMITM { get; set; } = false;
        public bool RedirectToAlternative { get; set; } = false;
        [JsonIgnore] public bool ForceLiveConnect { get; set; } = false;
    }
}
