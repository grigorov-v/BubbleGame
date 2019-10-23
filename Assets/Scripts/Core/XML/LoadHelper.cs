using UnityEngine;

using System.Xml;

namespace Core.XML {
    public class LoadHelper {
        public static void LoadFromResources(IConfig config, string assetPath, string nodeName) {
            var xmlAsset = Resources.Load(assetPath) as TextAsset;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlAsset.text);

            var root = xmlDoc.DocumentElement;
            var node = (nodeName == "root") ? (root as XmlNode) : (root[nodeName] as XmlNode);

            config.Load(node);
        }
    }
}