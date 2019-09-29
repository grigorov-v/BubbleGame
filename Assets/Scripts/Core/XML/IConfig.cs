using System.Xml;

namespace Core.XML {
	public interface IConfig {
		void Load(XmlNode node);
	}
}

