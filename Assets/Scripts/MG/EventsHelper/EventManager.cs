using UnityEngine;

using System;
using System.Collections.Generic;

using Singleton;

namespace EventsHelper {
	/// <summary>
	/// Event system implementation
	/// </summary>
	public sealed class EventManager : SingleInstance<EventManager> {
		public const float CleanUpInterval = 10.0f;

		public Dictionary<Type, HandlerBase> Handlers {
			get {
				return _handlers;
			}
		}

		readonly Dictionary<Type, HandlerBase> _handlers = new Dictionary<Type, HandlerBase>(100);

		public EventManager() {
			AddHelper();
		}
		
		public void CheckHandlersOnLoad() {
			foreach ( var handler in _handlers ) {
				handler.Value.FixWatchers();
			}
		}

		public void CleanUp() {
			foreach ( var handler in _handlers ) {
				handler.Value.CleanUp();
			}
		}
		
		void Sub<T>(object watcher, Action<T> action) {
			var tHandler = GetOrCreateHandler<T>();
			if ( tHandler != null ) {
				tHandler.Subscribe(watcher, action);
			}
		}

		void Unsub<T>(Action<T> action) {
			HandlerBase handler;
			if ( _handlers.TryGetValue(typeof(T), out handler) ) {
				var tHandler = handler as Handler<T>;
				if ( tHandler != null ) {
					tHandler.Unsubscribe(action);
				}
			}
		}

		void FireEvent<T>(T args) {
			var tHandler = GetOrCreateHandler<T>();
			if ( tHandler != null ) {
				tHandler.Fire(args);
			}
		}

		Handler<T> GetOrCreateHandler<T>() {
			HandlerBase handler;
			if ( !_handlers.TryGetValue(typeof(T), out handler) ) {
				handler = new Handler<T>();
				_handlers.Add(typeof(T), handler);
			}
			return handler as Handler<T>;
		}

		bool HasWatchersDirect<T>() where T : struct {
			HandlerBase container;
			if ( _handlers.TryGetValue(typeof(T), out container) ) {
				return (container.Watchers.Count > 0);
			}
			return false;
		}

		void AddHelper() {
			var go = new GameObject("[EventHelper]");
			go.AddComponent<EventHelper>();
		}
		
		public static void Subscribe<T>(object watcher, Action<T> action) where T:struct {
			Instance.Sub(watcher, action);
		}

		public static void Unsubscribe<T>(Action<T> action) where T : struct {
			if ( _instance != null ) {
				Instance.Unsub(action);
			}
		}

		public static void Fire<T>(T args) where T : struct {
			Instance.FireEvent(args);
		}

		public static bool HasWatchers<T>() where T : struct {
			return Instance.HasWatchersDirect<T>();
		}
	}
}