using UnityEngine;

using System;
using System.Collections.Generic;

namespace EventsHelper {
	/// <summary>
	/// Data class to represent event info in editor for debug
	/// </summary>
	[Serializable]
	public sealed class EventData {
		public string              Name          = string.Empty;
		public Type                Type          = null;
		public List<MonoBehaviour> MonoWatchers  = new List<MonoBehaviour>(100);
		public List<string>        OtherWatchers = new List<string>(100);

		public EventData(Type type) {
			Type = type;
			Name = type.ToString();
		}
	}
}