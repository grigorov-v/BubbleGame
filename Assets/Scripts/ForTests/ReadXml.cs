using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using Core.XML;
using Core.Configs;

using NaughtyAttributes;

public class Config: IConfigElement {
    public string AssetPath {
        get {
            return "Configs/LevelConfig";
        }
    }
    public string NodeName {
        get {
            return "bubble";
        }
    }

    public int    Layer = 0;
    public string Tag   = string.Empty;

    public void Load(XmlNode node) {
        Layer = node.GetAttrValue("layer", 0);
        Tag   = node.GetAttrValue("tag", string.Empty);
    }
}

public class ReadXml : MonoBehaviour {
    [Button("Read")]
    void Read() {
        var config = new Config();
        var xmlAsset = Resources.Load(config.AssetPath) as TextAsset;

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlAsset.text);

        var xRoot = xmlDoc.DocumentElement;
        var xnode = xRoot[config.NodeName] as XmlNode;

        config.Load(xnode);

        print(config.Layer);
        print(config.Tag);
    }
}
