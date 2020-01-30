using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XmlConfig;

namespace Configs {
    public class ConfigsRegistrator {
        public static Dictionary<string, IConfig> Configs = new Dictionary<string, IConfig> {
            { "level_config", new LevelConfig() }
        };
    }
}