using System.Xml;

namespace Core.XML {
	public interface XmlNodeLoadable<T> {
		T Load(XmlNode node);
	}
}
