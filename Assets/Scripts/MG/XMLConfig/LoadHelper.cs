using UnityEngine;

using System.Xml;

namespace XmlConfig {
    public class LoadHelper {
        public static void LoadFromResources(IConfig config, string assetPath, string nodeName) {
            var xmlAsset = Resources.Load(assetPath) as TextAsset;
            LoadConfigFromXmlText(config, xmlAsset.text, nodeName);
        }

        static void LoadConfigFromXmlText(IConfig config, string xmlText, string nodeName) {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);

            var root = xmlDoc.DocumentElement;
            var node = (nodeName == "root") ? (root as XmlNode) : (root[nodeName] as XmlNode);

            config.Load(node);
        }
    }
}