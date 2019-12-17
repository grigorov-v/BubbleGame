using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.XML;

namespace Configs {
    public class ConfigsRegistrator {
        public static Dictionary<string, IConfig> Configs = new Dictionary<string, IConfig> {
            { "level_config", new LevelConfig() }
        };
    }
}