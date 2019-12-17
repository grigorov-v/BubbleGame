using System.Collections.Generic;
using System.Xml;

using UnityEngine;
using UnityEngine.SceneManagement;

using Core.Controller;
using Core.XML;

using Configs;

namespace Controllers {
    struct XmlLoadableInfo {
        public IConfig Config;
        public string AssetPath;
        public string NodeName;
    }

    public class ConfigsController : BaseController<ConfigsController> {
        const string PATH_COMMON_CONFIG = "Configs/CommonConfig";

        CommonConfig _commonConfig = new CommonConfig();

        public override void PostInit() {
            LoadHelper.LoadFromResources(_commonConfig, PATH_COMMON_CONFIG, "root");

            var worldInfo = _commonConfig.Worlds["CandyWorld"]; // Будет браться из сохранений
            foreach (var pair in ConfigsRegistrator.Configs) {
                var name = pair.Key;
                var path = worldInfo.Configs.ContainsKey(name) ? worldInfo.Configs[name] : string.Empty;

                if ( string.IsNullOrEmpty(path) ) {
                    continue;
                }

                var config = pair.Value;
                LoadHelper.LoadFromResources(config, path, "root");
            }
        }

        public T FindConfig<T>() where T: class, IConfig {
            foreach (var pair in ConfigsRegistrator.Configs) {
                var config = pair.Value;

                var check = (config is T);
                if ( !check ) {
                    continue;
                }

                return config as T;
            }

            return null;
        }

        public LevelInfo GetLevelInfo(int levelIndex) {
            var levelConfig = FindConfig<LevelConfig>();
            if ( levelConfig == null ) {
                return null;
            }

            var levels = levelConfig.Levels;
            if ( (levels == null) || (levelIndex > levels.Count - 1) ) {
                return null;
            }

            return levels[levelIndex];
        }
    } 
}