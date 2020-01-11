using System.Collections;
using System.Collections.Generic;
using System.Xml;

using Core.XML;

namespace Configs {
    public class BubbleInfo: XmlNodeLoadable<BubbleInfo> {
        public string Tag   {get; private set;}
        public int    Count {get; private set;}

        public BubbleInfo Load(XmlNode node) {
            Tag   = node.GetAttrValue("tag", string.Empty);
            Count = node.GetAttrValue("count", 1);
            return this;
        }
    }

    public class LevelInfo: XmlNodeLoadable<LevelInfo> {
        public string           LevelTemplate {get; private set;} 
        public List<BubbleInfo> Bubbles       {get; private set;}

        public LevelInfo Load(XmlNode node) {
            LevelTemplate = node.GetAttrValue("template", string.Empty);
            Bubbles       = node.LoadNodeList("bubbles", "bubble",  index => new BubbleInfo());
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