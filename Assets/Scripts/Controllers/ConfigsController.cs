using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine;

using Core.Controller;
using Core.XML;

namespace Controllers {
    public struct XmlLoadableInfo {
        public BaseXmlNodeLoadable Config;
        public string AssetPath;
        public string NodeName;

        public void LoadXml() {
            var xmlAsset = Resources.Load(AssetPath) as TextAsset;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlAsset.text);

            var root = xmlDoc.DocumentElement;
            var node = root[NodeName] as XmlNode;

            Config.Load(node);
        }
    }

    public class ConfigsController : BaseController<ConfigsController> {
        List<XmlLoadableInfo> _xmlLoadableInfoList = new List<XmlLoadableInfo>() {
            new XmlLoadableInfo() {
                Config    = new TestConfig(),
                AssetPath = "Configs/LevelConfig",
                NodeName  = "bubble"
            }   
        };

        public override void PostInit() {
            _xmlLoadableInfoList.ForEach(info => info.LoadXml());
        }

        public T FindXmlConfig<T>() where T: class, BaseXmlNodeLoadable {
            foreach (var xmlLoadableInfo in _xmlLoadableInfoList) {
                var check = (xmlLoadableInfo.Config is T);
                if ( !check ) {
                    continue;
                }

                return xmlLoadableInfo.Config as T;
            }

            return null;
        }
    } 
}