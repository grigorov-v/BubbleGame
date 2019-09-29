using System;
using System.Xml;

namespace Core.XML {
    public static class XmlHelper {
        public static string GetAttributeValue(this XmlNode node, string name, string def) {
            var attr = node.Attributes.GetNamedItem(name);
            if ( attr == null ) {
                return def;
            }

            return attr.Value;
        }

        public static int GetAttributeValue(this XmlNode node, string name, int def) {
            var value = node.GetAttributeValue(name, null);
            if ( String.IsNullOrEmpty(value) ) {
                return def;
            }

            return int.Parse(value);
        }

        public static float GetAttributeValue(this XmlNode node, string name, float def) {
            var value = node.GetAttributeValue(name, null);
            if ( String.IsNullOrEmpty(value) ) {
                return def;
            }

            return float.Parse(value);
        }
    }
}