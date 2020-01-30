using UnityEngine;

using System;
using System.Collections.Generic;

namespace EventsHelper {
	sealed class Handler<T> : HandlerBase {
		List<Action<T>> _actions  = new List<Action<T>>(100);
		List<Action<T>> _removed  = new List<Action<T>>(100);

		public void Subscribe(object watcher, Action<T> action) {
			if ( _removed.Contains(action) ) {
				_removed.Remove(action);
			}
			if ( !_actions.Contains(action) ) {
				_actions.Add(action);
				_watchers.Add(watcher);
			} else if ( LogsEnabled ) {
				//Log.TraceWarningFormat(LogTag.Event, "{0} tries to subscribe to {1} again.", watcher, action);
			}
		}

		public void Unsubscribe(Action<T> action) {
			SafeUnsubscribe(action);
		}

		void SafeUnsubscribe(Action<T> action) {
			var index = _actions.IndexOf(action);
			SafeUnsubscribe(index);
			if ( (index < 0) && LogsEnabled ) {
				//Log.TraceWarningFormat(LogTag.Event, "Trying to unsubscribe action {0} without watcher.", action);
			}
		}

		void SafeUnsubscribe(int index) {
			if ( index >= 0 ) {
				_removed.Add(_actions[index]);
			}
		}

		void FullUnsubscribe(int index) {
			if ( index >= 0 ) {
				_actions.RemoveAt(index);
				if ( index < _watchers.Count ) {
					_watchers.RemoveAt(index);
				}
			} 
		}

		void FullUnsubscribe(Action<T> action) {
			var index = _actions.IndexOf(action);
			FullUnsubscribe(index);
		}

		public void Fire(T arg) {
			for ( var i = 0; i < _actions.Count; i++ ) {
				var current = _actions[i];
				if ( !_removed.Contains(current) ) {
					try {
						current.Invoke(arg);
					} catch {
						//Log.TraceException(new EventCallbackException(typeof(T), e));
					}
				}
			}
			CleanUp();
			if ( AllFireLogs ) {
				//Log.TraceFormat(LogTag.Event, "[{0}] fired (Listeners: {1})", typeof(T).Name, _watchers.Count);
			}
		}

		public override void CleanUp() {
			foreach ( var item in _removed ) {
				FullUnsubscribe(item);
			}
			_removed.Clear();
		}

		public override bool FixWatchers() {
			CleanUp();
			var count = 0;
			for ( var i = 0; i < _watchers.Count; i++ ) {
				var watcher = _watchers[i];
				if ( watcher is MonoBehaviour ) {
					var comp = watcher as MonoBehaviour;
					if ( !comp ) {
						SafeUnsubscribe(i);
						count++;
					}
				} 
			}
			if ( count > 0 ) {
				CleanUp();
			}
			if ( (count > 0) && LogsEnabled ) {
				//Log.TraceErrorFormat(LogTag.Event, "{0} destroyed scripts subscribed to event {1}.", count, typeof(T));
			}
			return (count == 0);
		}
	}
}