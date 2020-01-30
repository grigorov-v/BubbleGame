using System.Collections;
using System.Collections.Generic;
using System.Xml;

using XmlConfig;
using Core.Extensions;

using LevelValues;
using Utils;

namespace Configs {
    public class BubbleInfo: XmlNodeLoadable<BubbleInfo> {
        public string Tag   { get; private set; }
        public int    Count { get; private set; }

        public BubbleInfo Load(XmlNode node) {
            Tag   = node.GetAttrValue("tag", string.Empty);
            Count = node.GetAttrValue("count", 1);
            return this;
        }
    }

    public class LevelInfo: XmlNodeLoadable<LevelInfo> {
        public string            LevelTemplate { get; private set; } 
        public List<BubbleInfo>  Bubbles       { get; private set; }
        public List<LevelTarget> Targets       { get; private set; }
        public List<BubbleInfo>  IgnoreBubbles { get; private set; }

        public LevelInfo Load(XmlNode node) {
            LevelTemplate = node.GetAttrValue("template", string.Empty);
            Bubbles       = node.LoadNodeList("bubbles", "bubble",  index => new BubbleInfo());
            Targets       = FindTargets(node);
            IgnoreBubbles = FindIgnoreBubbles(node);
            return this;
        }

        List<LevelTarget> FindTargets(XmlNode node) {
            var childNode = node.SelectFirstNode("targets");
            var strTargets = childNode.LoadNodeList("target", "name");
            var targets = new List<LevelTarget>();
            foreach ( var strTarget in strTargets ) {
                var target = StringToEnumConverter.LevelTargetFromString(strTarget);
                targets.Add(target);
            }

            return targets;
        }

        List<BubbleInfo> FindIgnoreBubbles(XmlNode node) {
            var childNode = node.SelectFirstNode("targets");
            var targetNodes = childNode.ChildNodes;
            var ignoreBubbles = new List<BubbleInfo>();
            foreach ( XmlNode curNode in targetNodes ) {
                if ( curNode.GetAttrValue("name", string.Empty) == "IgnoreBubble" ) {
                    ignoreBubbles = curNode.LoadNodeList("bubble",  index => new BubbleInfo());
                }
            }

            return ignoreBubbles;
        }
    }

    public class LevelConfig: IConfig {
        public List<LevelInfo> Levels { get; private set; }

        public void Load(XmlNode node) {
            Levels = node.LoadNodeList("level",  index => new LevelInfo());
        }
    }
}