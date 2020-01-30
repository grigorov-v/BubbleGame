using System.Xml;

namespace XmlConfig {
	public interface XmlNodeLoadable<T> {
		T Load(XmlNode node);
	}
}
