using System.Xml;

namespace Core.XML {
    public interface ISave: IConfig {
        void Save(XmlNode node);
    }
}