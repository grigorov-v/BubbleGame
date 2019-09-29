using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using Core.XML;

using NaughtyAttributes;

using Controllers;

public class TestConfig: BaseXmlNodeLoadable {
    public int    Layer = 0;
    public string Tag   = string.Empty;

    public void Load(XmlNode node) {
        Layer = node.GetAttrValue("layer", 0);
        Tag   = node.GetAttrValue("tag", string.Empty);
    }
}

public class ReadXml : MonoBehaviour {
    private void Start() {
        var config = ConfigsController.Instance.FindXmlConfig<TestConfig>();
        Debug.Log(config.Tag);
    }
}
