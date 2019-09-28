using System.Xml;

public interface IConfig {
    string AssetPath {get;}
    string NodeName  {get;}

    void Load(XmlNode node);
}