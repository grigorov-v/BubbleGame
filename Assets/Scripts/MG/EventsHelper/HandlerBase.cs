using System.Collections.Generic;

namespace EventsHelper {
	public abstract class HandlerBase {
		public static bool LogsEnabled { get { return false;       }}
		public static bool AllFireLogs { get { return LogsEnabled; }}

		public List<object> Watchers { get { return _watchers; }}

		protected readonly List<object> _watchers = new List<object>(100);

		public virtual void CleanUp() {}

		public virtual bool FixWatchers() {
			return false;
		}
	}
}