using System.Xml;

namespace XmlConfig {
	public interface IConfig {
		void Load(XmlNode node);
	}
}

