using System.Xml;

namespace Core.XML {
    public interface ISave: XmlNodeLoadable<ISave> {
        void Save(XmlNode node);
    }
}