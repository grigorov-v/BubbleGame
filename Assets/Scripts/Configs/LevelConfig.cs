using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

using Core.XML;

namespace Configs {
    public class BubbleInfo: XmlNodeLoadable<BubbleInfo> {
        public int    Line   {get; private set;}
        public int    Column {get; private set;}
        public string Tag    {get; private set;}

        public BubbleInfo Load(XmlNode node) {
            Line   = node.GetAttrValue("line", 0);
            Column = node.GetAttrValue("column", 0);
            Tag    = node.GetAttrValue("tag", string.Empty);
            return this;
        }
    }

    public class LevelInfo: XmlNodeLoadable<LevelInfo> {
        public List<BubbleInfo> Bubbles {get; private set;}

        public LevelInfo Load(XmlNode node) {
            Bubbles = node.LoadNodeList("bubbles", "bubble",  index => new BubbleInfo());
            return this;
        }
    }

    public class LevelConfig: IConfig {
        public List<LevelInfo> Levels {get; private set;}

        public void Load(XmlNode node) {
            Levels = node.LoadNodeList("level",  index => new LevelInfo());
        }
    }
}