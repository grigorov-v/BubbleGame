using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections.Generic;

namespace Core.Events {
	/// <summary>
	/// Helper class to clean up unwanted subscribers on scene load and debug active subscriptions
	/// </summary>
	public sealed class EventHelper : MonoBehaviour {
		// To debug:
		// Adds SubscribeToLog calls inside SubscribeToLogged method and observe invocations in log
		// Also, you can see all subscriptions in editor:
		// - Find [EventHelper] while application is running
		// - Click 'Fill' in context menu of Event Helper (Script)
		// - List of all events with subscribers appears below
		
		public bool AutoFill { get { return false; }}

		public List<EventData> Events = new List<EventData>(100);

		Dictionary<Type, string> _typeCache    = new Dictionary<Type, string>();
		float                    _cleanupTimer = 0;

		void Awake() {
			DontDestroyOnLoad(gameObject);
			SubscribeToLogged();
		}

		void OnEnable() {
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnDisable() {
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			EventManager.Instance.CheckHandlersOnLoad();
		}

		void SubscribeToLogged() {}

		// ReSharper disable once UnusedMember.Local
		void SubscribeToLog<T>() where T:struct {
			EventManager.Subscribe<T>(this, OnLog);
		}

		void OnLog<T>(T ev) where T:struct {
			
		}

		void Update() {
			TryCleanUp();
			if(AutoFill) {
				Fill();
			}
		}

		[ContextMenu("CheckEventHandlers")]
		public void CheckEventHandlers() {
			var handlers = EventManager.Instance.Handlers;
			foreach ( var handler in handlers ) {
				if ( handler.Value.Watchers.Count > 0 ) {
					Debug.Log(handler.Key);
					foreach ( var watcher in handler.Value.Watchers ) {
						Debug.Log(handler.Key + " => " + watcher.GetType());
					}
				}
			}
		}

		[ContextMenu("ClearEventHandlers")]
		public void ClearEventHandlers() {
			EventManager.Instance.Handlers.Clear();
		}

		void TryCleanUp() {
			if ( _cleanupTimer > EventManager.CleanUpInterval ) {
				EventManager.Instance.CleanUp();
				_cleanupTimer = 0;
			} else {
				_cleanupTimer += Time.deltaTime;
			}
		}

		[ContextMenu("Fill")]
		public void Fill() {
			var handlers = EventManager.Instance.Handlers;
			foreach ( var pair in handlers ) {
				var eventData = GetEventData(pair.Key);
				if ( eventData == null ) {
					eventData = new EventData(pair.Key);
					Events.Add(eventData);
				}
				FillEvent(pair.Value, eventData);
			}
		}

		void FillEvent(HandlerBase handler, EventData data) {
			data.MonoWatchers.Clear();
			data.OtherWatchers.Clear();
			foreach ( var item in handler.Watchers) {
				if ( item is MonoBehaviour ) {
					data.MonoWatchers.Add(item as MonoBehaviour);
				} else {
					data.OtherWatchers.Add((item != null) ? GetTypeNameFromCache(item.GetType()) : "null");
				}
			}
		}

		EventData GetEventData(Type type) {
			foreach ( var ev in Events ) {
				if ( ev.Type == type ) {
					return ev;
				}
			}
			return null;
		}

		string GetTypeNameFromCache(Type type) {
			var typeName = string.Empty;
			if ( !_typeCache.TryGetValue(type, out typeName) ) {
				typeName = type.ToString();
				_typeCache.Add(type, typeName);
			}
			return typeName;
		}
	}
}