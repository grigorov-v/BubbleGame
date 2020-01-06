using System.Collections;
using System.Collections.Generic;
using System.Xml;

using UnityEngine;

using Core.XML;

namespace Configs {
    public class SaveInfo: XmlNodeLoadable<SaveInfo> {
        public string LevelKey {get; private set;}
        
        public SaveInfo Load(XmlNode node) {
            var childNode = node.SelectFirstNode("level_key");
            LevelKey = childNode.GetAttrValue("name", string.Empty);
            return this;
        }
    }

    public class WorldInfo: XmlNodeLoadable<WorldInfo> {
        public string Name                         {get; private set;}
        public string MapSceneName                 {get; private set;}
        public string GameSceneName                {get; private set;}
        public Dictionary<string, string> Configs  {get; private set;}
        public SaveInfo                   SaveInfo {get; private set;}

        public WorldInfo Load(XmlNode node) {
            Name = node.GetAttrValue("Name", string.Empty);
            var firstNode = node.SelectFirstNode("map_scene");
            MapSceneName = (firstNode != null) ? firstNode.GetAttrValue("name", string.Empty) : string.Empty;
            firstNode = node.SelectFirstNode("game_scene");
            GameSceneName = (firstNode != null) ? firstNode.GetAttrValue("name", string.Empty) : string.Empty;
            Configs = node.LoadNodeDict("configs", "config", string.Empty);
            SaveInfo = node.LoadFromXPath("save_info", new SaveInfo());
            return this;
        }
    }

    public class CommonConfig : IConfig {
        public Dictionary<string, WorldInfo> Worlds {get; private set;}

        public void Load(XmlNode node) {
            Worlds = node.LoadNodeDict("worlds", "world", name => new WorldInfo());
        }
    }
}