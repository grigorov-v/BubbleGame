using System.Xml;

namespace Core.Configs {
    public interface IConfigElement {
        string AssetPath {get;}
        string NodeName  {get;}

        void Load(XmlNode node);
    }
}