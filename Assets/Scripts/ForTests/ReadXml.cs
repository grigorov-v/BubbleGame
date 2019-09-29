using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using Core.XML;

using NaughtyAttributes;

using Controllers;
using Configs;

public class TestConfig: IConfig {
    public int    Layer = 0;
    public string Tag   = string.Empty;

    public void Load(XmlNode node) {
        Layer = node.GetAttrValue("layer", 0);
        Tag   = node.GetAttrValue("tag", string.Empty);
    }
}

public class ReadXml : MonoBehaviour {
    [Button("REad")]
    private void Read() {
        var config = ConfigsController.Instance.FindConfig<LevelConfig>();

        Debug.LogFormat("Levels count {0}", config.Levels.Count);

        foreach (var level in config.Levels) {
            foreach (var bubble in level.Bubbles) {
                Debug.Log(bubble.Tag);
            }
        }
    }
}
