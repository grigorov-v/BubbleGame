using UnityEngine;
using UnityEngine.EventSystems;

using Core.Events;
using Game.Events;

namespace Game.Bubbles {
    public class MapDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public void OnBeginDrag(PointerEventData eventData) {
            EventManager.Fire(new BeginMapDrag(eventData));
        }

        public void OnDrag(PointerEventData eventData) {
            EventManager.Fire(new MapDrag(eventData));
        }

        public void OnEndDrag(PointerEventData eventData) {
            EventManager.Fire(new EndMapDrag(eventData));
        }
    }
}