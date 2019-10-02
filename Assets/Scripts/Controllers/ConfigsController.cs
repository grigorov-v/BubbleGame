using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine;
using UnityEngine.SceneManagement;

using Core.Controller;
using Core.XML;

using Configs;

namespace Controllers {
    public struct XmlLoadableInfo {
        public IConfig Config;
        public string AssetPath;
        public string NodeName;

        public string InitAssetPath() {
            var path = AssetPath;

            if ( path.IndexOf("{SceneName}") != -1 ) {
                var sceneName = SceneManager.GetActiveScene().name;
                path = path.Replace("{SceneName}", sceneName);
            }

            return path;
        }

        public void LoadXml() {
            AssetPath = InitAssetPath();
            var xmlAsset = Resources.Load(AssetPath) as TextAsset;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlAsset.text);

            var root = xmlDoc.DocumentElement;
            var node = (NodeName == "root") ? (root as XmlNode) : (root[NodeName] as XmlNode);

            Config.Load(node);
        }
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
            _xmlLoadableInfoList.ForEach(info => info.LoadXml());
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