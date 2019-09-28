using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;

using NaughtyAttributes;

public class Config: IConfig {
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
        var attr = node.Attributes.GetNamedItem("layer");
        if ( attr != null ) {
            Layer = int.Parse(attr.Value);
        }

        attr = node.Attributes.GetNamedItem("tag");
        if ( attr != null ) {
            Tag = attr.Value;
        }
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
