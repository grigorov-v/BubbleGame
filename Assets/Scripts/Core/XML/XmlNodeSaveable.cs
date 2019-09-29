using System.Xml;

namespace Core.XML {
	public interface XmlNodeSaveable<T> {
		void Save(XmlElement elem);
	}
}