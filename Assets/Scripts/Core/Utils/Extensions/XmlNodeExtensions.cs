using UnityEngine;

using System;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;

using Core.Time;
using Core.XML;

namespace Core.Extensions {
	public static class XmlNodeExtensions {
		
		// TryGetAttrValue
		
		public static bool TryGetAttrValue(this XmlNode node, string name, out bool result) {
			var attr = node.Attributes[name];
			if ( (attr != null) && bool.TryParse(attr.Value, out result) ) {
				return true;
			}
			result = false;
			return false;
		}
		
		public static bool TryGetAttrValue(this XmlNode node, string name, out int result) {
			var attr = node.Attributes[name];
			if ( (attr != null) && int.TryParse(attr.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) ) {
				return true;
			}
			result = 0;
			return false;
		}
		
		public static bool TryGetAttrValue(this XmlNode node, string name, out uint result) {
			var attr = node.Attributes[name];
			if ( (attr != null) && uint.TryParse(attr.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) ) {
				return true;
			}
			result = 0u;
			return false;
		}
		
		public static bool TryGetAttrValue(this XmlNode node, string name, out long result) {
			var attr = node.Attributes[name];
			if ( (attr != null) && long.TryParse(attr.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) ) {
				return true;
			}
			result = 0;
			return false;
		}
		
		public static bool TryGetAttrValue(this XmlNode node, string name, out ulong result) {
			var attr = node.Attributes[name];
			if ( (attr != null) && ulong.TryParse(attr.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) ) {
				return true;
			}
			result = 0ul;
			return false;
		}
		
		public static bool TryGetAttrValue(this XmlNode node, string name, out float result) {
			var attr = node.Attributes[name];
			if ( (attr != null) && float.TryParse(attr.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) ) {
				return true;
			}
			result = 0.0f;
			return false;
		}

		public static bool TryGetAttrValue(this XmlNode node, string name, out double result) {
			var attr = node.Attributes[name];
			if ( (attr != null) && double.TryParse(attr.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) ) {
				return true;
			}
			result = 0.0;
			return false;
		}
		
		public static bool TryGetAttrValue(this XmlNode node, string name, out string result) {
			var attr = node.Attributes[name];
			if ( attr != null ) {
				result = attr.Value;
				return true;
			}
			result = string.Empty;
			return false;
		}
		
		public static bool TryGetAttrValue(this XmlNode node, string name, out DateTime result) {
			long timestamp;
			if ( node.TryGetAttrValue(name, out timestamp) ) {
				result = TimeUtils.ConvertFromUnixTimestamp(timestamp);
				return true;
			} else {
				var dateStr = string.Empty;
				if ( node.TryGetAttrValue(name, out dateStr) ) {
					if ( DateTime.TryParseExact(dateStr, "yyyy-MM-dd HH-mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ) {
						return true;
					} else if ( DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ) {
						return true;
					}
				}
			}
			result = DateTime.MinValue;
			return false;
		}
		
		// GetAttrValue
		
		public static bool GetAttrValue(this XmlNode node, string name, bool def) {
			bool result;
			return node.TryGetAttrValue(name, out result) ? result : def;
		}
		
		public static int GetAttrValue(this XmlNode node, string name, int def) {
			int result;
			return node.TryGetAttrValue(name, out result) ? result : def;
		}

		public static uint GetAttrValue(this XmlNode node, string name, uint def) {
			uint result;
			return node.TryGetAttrValue(name, out result) ? result : def;
		}
		
		public static long GetAttrValue(this XmlNode node, string name, long def) {
			long result;
			return node.TryGetAttrValue(name, out result) ? result : def;
		}
		
		public static ulong GetAttrValue(this XmlNode node, string name, ulong def) {
			ulong result;
			return node.TryGetAttrValue(name, out result) ? result : def;
		}
		
		public static float GetAttrValue(this XmlNode node, string name, float def) {
			float result;
			return node.TryGetAttrValue(name, out result) ? result : def;
		}
		
		public static string GetAttrValue(this XmlNode node, string name, string def) {
			string result;
			return node.TryGetAttrValue(name, out result) ? result : def;
		}
		
		public static DateTime GetAttrValue(this XmlNode node, string name, DateTime def) {
			DateTime result;
			return node.TryGetAttrValue(name, out result) ? result : def;
		}
		
		// TryChangeAttr
		
		public static bool TryChangeAttr(this XmlNode node, string name, Func<bool, bool> mapFunc) {
			bool result;
			if ( node.TryGetAttrValue(name, out result) ) {
				node.Attributes[name].Value = mapFunc(result).ToString();
				return true;
			}
			return false;
		}

		public static bool TryChangeAttr(this XmlNode node, string name, Func<int, int> mapFunc) {
			int result;
			if ( node.TryGetAttrValue(name, out result) ) {
				node.Attributes[name].Value = mapFunc(result).ToString();
				return true;
			}
			return false;
		}

		public static bool TryChangeAttr(this XmlNode node, string name, Func<string, string> mapFunc) {
			string result;
			if ( node.TryGetAttrValue(name, out result) ) {
				node.Attributes[name].Value = mapFunc(result);
				return true;
			}
			return false;
		}
		
		// LoadNodeList
		
		public static List<int> LoadNodeList(this XmlNode node, string name, string attrName, int attrDef) {
			var result = new List<int>();
			foreach ( XmlNode childNode in node.ChildNodes ) {
				if ( childNode.Name == name ) {
					result.Add(childNode.GetAttrValue(attrName, attrDef));
				}
			}
			return result;
		}

		public static List<int> LoadNodeList(this XmlNode node, string parentName, string name, string attrName, int attrDef) {
			var result = new List<int>();
			foreach ( XmlNode parentNode in node.ChildNodes ) {
				if ( parentNode.Name == parentName ) {
					foreach ( XmlNode childNode in parentNode.ChildNodes ) {
						if ( childNode.Name == name ) {
							result.Add(childNode.GetAttrValue(attrName, attrDef));
						}
					}
					break;
				}
			}
			return result;
		}

		public static List<string> LoadNodeList(this XmlNode node, string name, string attrName) {
			var result = new List<string>();
			foreach ( XmlNode childNode in node.ChildNodes ) {
				if ( childNode.Name == name ) {
					result.Add(childNode.GetNotEmptyStringAttr(attrName));
				}
			}
			return result;
		}

		public static List<T> LoadNodeList<T>(this XmlNode node, string name, Func<int,T> factory) where T : XmlNodeLoadable<T> {
			var result = new List<T>();
			foreach ( XmlNode childNode in node.ChildNodes ) {
				if ( childNode.Name == name ) {
					var val = factory(result.Count);
					if ( val != null ) {
						val.Load(childNode);
						result.Add(val);
					}
				}
			}
			return result;
		}

		public static List<T> LoadNodeList<T>(this XmlNode node, string parentName, string name, Func<int, T> factory) where T : XmlNodeLoadable<T> {
			var result = new List<T>();
			foreach ( XmlNode parentNode in node.ChildNodes ) {
				if ( parentNode.Name == parentName ) {
					foreach ( XmlNode childNode in parentNode.ChildNodes ) {
						if ( childNode.Name == name ) {
							var val = factory(result.Count);
							if ( val != null ) {
								val.Load(childNode);
								result.Add(val);
							}
						}
					}
					break;
				}
			}
			return result;
		}
		
		// LoadNodeRawList
		
		public static List<T> LoadNodeRawList<T>(this XmlNode node, string name, Func<XmlNode,T> factory) {
			var result = new List<T>();
			foreach ( XmlNode childNode in node.ChildNodes ) {
				if ( childNode.Name == name ) {
					var val = factory(childNode);
					if ( val != null ) {
						result.Add(val);
					}
				}
			}
			return result;
		}

		public static List<T> LoadNodeRawList<T>(this XmlNode node, string parentName, string name, Func<XmlNode, T> factory) {
			var result = new List<T>();
			foreach ( XmlNode parentNode in node.ChildNodes ) {
				if ( parentNode.Name == parentName ) {
					foreach ( XmlNode childNode in parentNode.ChildNodes ) {
						if ( childNode.Name == name ) {
							var val = factory(childNode);
							if ( val != null ) {
								result.Add(val);
							}
						}
					}
					break;
				}
			}
			return result;
		}
		
		// LoadNodeDict
		
		public static Dictionary<string, bool> LoadNodeDict(this XmlNode node, string name, bool def) {
			var result = new Dictionary<string, bool>();
			foreach ( XmlNode childNode in node.ChildNodes ) {
				if ( childNode.Name == name ) {
					var key = childNode.GetNotEmptyStringAttr("name");
					var value = childNode.GetAttrValue("value", def);
					result[key] = value;
				}
			}
			return result;
		}

		public static Dictionary<string, bool> LoadNodeDict(this XmlNode node, string parentName, string name, bool def) {
			var result = new Dictionary<string, bool>();
			foreach ( XmlNode parentNode in node.ChildNodes ) {
				if ( parentNode.Name == parentName ) {
					foreach ( XmlNode childNode in parentNode.ChildNodes ) {
						if ( childNode.Name == name ) {
							var key = childNode.GetNotEmptyStringAttr("name");
							var value = childNode.GetAttrValue("value", def);
							result[key] = value;
						}
					}
					break;
				}
			}
			return result;
		}

		public static Dictionary<string, int> LoadNodeDict(this XmlNode node, string name, int def) {
			var result = new Dictionary<string, int>();
			foreach ( XmlNode childNode in node.ChildNodes ) {
				if ( childNode.Name == name ) {
					var key = childNode.GetNotEmptyStringAttr("name");
					var value = childNode.GetAttrValue("value", def);
					result[key] = value;
				}
			}
			return result;
		}

		public static Dictionary<string, int> LoadNodeDict(this XmlNode node, string parentName, string name, int def) {
			var result = new Dictionary<string, int>();
			foreach ( XmlNode parentNode in node.ChildNodes ) {
				if ( parentNode.Name == parentName ) {
					foreach ( XmlNode childNode in parentNode.ChildNodes ) {
						if ( childNode.Name == name ) {
							var key = childNode.GetNotEmptyStringAttr("name");
							var value = childNode.GetAttrValue("value", def);
							result[key] = value;
						}
					}
					break;
				}
			}
			return result;
		}

		public static Dictionary<string, long> LoadNodeDict(this XmlNode node, string name, long def) {
			var result = new Dictionary<string, long>();
			foreach ( XmlNode childNode in node.ChildNodes ) {
				if ( childNode.Name == name ) {
					var key = childNode.GetNotEmptyStringAttr("name");
					var value = childNode.GetAttrValue("value", def);
					result[key] = value;
				}
			}
			return result;
		}
		
		public static Dictionary<string, string> LoadNodeDict(this XmlNode node, string name, string def) {
			var result = new Dictionary<string, string>();
			foreach ( XmlNode childNode in node.ChildNodes ) {
				if ( childNode.Name == name ) {
					var key = childNode.GetNotEmptyStringAttr("name");
					var value = childNode.GetAttrValue("value", def);
					result[key] = value;
				}
			}
			return result;
		}

		public static Dictionary<string, string> LoadNodeDict(this XmlNode node, string parentName, string name, string def) {
			var result = new Dictionary<string, string>();
			foreach ( XmlNode parentNode in node.ChildNodes ) {
				if ( parentNode.Name == parentName ) {
					foreach ( XmlNode childNode in parentNode.ChildNodes ) {
						if ( childNode.Name == name ) {
							var key = childNode.GetNotEmptyStringAttr("name");
							var value = childNode.GetAttrValue("value", def);
							result[key] = value;
						}
					}
				}
			}
			return result;
		}

		public static Dictionary<string, T> LoadNodeDict<T>(this XmlNode node, string name, Func<string, T> factory) where T : XmlNodeLoadable<T> {
			var result = new Dictionary<string, T>();
			foreach ( XmlNode childNode in node.ChildNodes ) {
				if ( childNode.Name == name ) {
					var key = childNode.GetNotEmptyStringAttr("name");
					var value = factory(key);
					value.Load(childNode);
					result[key] = value;
				}
			}
			return result;
		}

		public static Dictionary<string, T> LoadNodeDict<T>(this XmlNode node, string parentName, string name, Func<string, T> factory) where T : XmlNodeLoadable<T> {
			var result = new Dictionary<string, T>();
			foreach ( XmlNode parentNode in node.ChildNodes ) {
				if ( parentNode.Name == parentName ) {
					foreach ( XmlNode childNode in parentNode.ChildNodes ) {
						if ( childNode.Name == name ) {
							var key = childNode.GetNotEmptyStringAttr("name");
							var value = factory(key);
							value.Load(childNode);
							result[key] = value;
						}
					}
					break;
				}
			}
			return result;
		}

		public static Dictionary<string, T> LoadNodeDict<T>(this XmlNode node, string parentName, string name, Func<string, int, T> factory) where T : XmlNodeLoadable<T> {
			var index = 0;
			var result = new Dictionary<string, T>();
			foreach ( XmlNode parentNode in node.ChildNodes ) {
				if ( parentNode.Name == parentName ) {
					foreach ( XmlNode childNode in parentNode.ChildNodes ) {
						if ( childNode.Name == name ) {
							var key = childNode.GetNotEmptyStringAttr("name");
							var value = factory(key, index++);
							value.Load(childNode);
							result[key] = value;
						}
					}
					break;
				}
			}
			return result;
		}
		
		// SelectFirstNode
		// Note: SelectSingleNode can't be used, because XmlNode has such instance method
		
		public static XmlNode SelectFirstNode(this XmlNode parent, string name) {
			if ( parent == null ) {
				return null;
			}
			foreach ( XmlNode childNode in parent.ChildNodes ) {
				if ( childNode.Name == name ) {
					return childNode;
				}
			}
			return null;
		}

		public static XmlNode SelectFirstNode(this XmlNode parent, string parentName, string name) {
			foreach ( XmlNode parentNode in parent.ChildNodes ) {
				if ( parentNode.Name == parentName ) {
					foreach ( XmlNode childNode in parentNode.ChildNodes ) {
						if ( childNode.Name == name ) {
							return childNode;
						}
					}
					break;
				}
			}
			return null;
		}
		
		// LoadFromXPath
		
		public static T LoadFromXPath<T>(this XmlNode parent, string name, T init) where T : XmlNodeLoadable<T> {
			XmlNode node = null;
			foreach ( XmlNode childNode in parent.ChildNodes ) {
				if ( childNode.Name == name ) {
					node = childNode;
					break;
				}
			}
			return ( (init != null) && (node != null) )
				? init.Load(node) : init;
		}

		public static T LoadFromXPathStrict<T>(this XmlNode parent, string xpath, T init) where T : XmlNodeLoadable<T> {
			var node = parent.SelectSingleNode(xpath);
			return ( (init != null) && (node != null) )
				? init.Load(node) : init;
		}
		
		// Other
		
		public static bool HasAttrValue(this XmlNode node, string name) {
			string value = null;
			return node.TryGetAttrValue(name, out value);
		}
		
		public static string GetNotEmptyStringAttr(this XmlNode node, string name) {
			string result;
			if ( node.TryGetAttrValue(name, out result) && !string.IsNullOrEmpty(result) ) {
				return result;
			}
			throw new InvalidOperationException(string.Format("GetNotEmptyStringAttr: string attribute wrong({0})", name));
		}
		
		public static Vector2 CreateVectorFromNode(this XmlNode node) {
			float x = node.GetAttrValue("x", 0);
			float y = node.GetAttrValue("y", 0);
			return new Vector2(x, y);
		}
	}
}
