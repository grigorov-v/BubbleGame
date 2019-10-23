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
        List<XmlLoadableInfo> _xmlLoadableInfoList = new List<XmlLoadableInfo>() {
            new XmlLoadableInfo() {
                Config    = new LevelConfig(),
                AssetPath = "Configs/{SceneName}_LevelConfig",
                NodeName  = "root"
            }   
        };

        public override void PostInit() {
            foreach (var loadableInfo in _xmlLoadableInfoList) {
                var config = loadableInfo.Config;
                var assetPath = FilterAssetPath(loadableInfo.AssetPath);
                var nodeName = loadableInfo.NodeName;
                LoadHelper.LoadFromResources(config, assetPath, nodeName);
            }
        }

        string FilterAssetPath(string path) {
            if ( path.IndexOf("{SceneName}") != -1 ) {
                var sceneName = SceneManager.GetActiveScene().name;
                sceneName = sceneName.Replace("_map", "");
                path = path.Replace("{SceneName}", sceneName);
            }

            return path;
        }

        public T FindConfig<T>() where T: class, IConfig {
            foreach (var xmlLoadableInfo in _xmlLoadableInfoList) {
                var check = (xmlLoadableInfo.Config is T);
                if ( !check ) {
                    continue;
                }

                return xmlLoadableInfo.Config as T;
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