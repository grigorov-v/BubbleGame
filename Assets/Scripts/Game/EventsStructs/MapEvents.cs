using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Events {
    public struct BeginMapDrag {
        public PointerEventData EventData {get; private set;}

        public BeginMapDrag(PointerEventData eventData) {
            EventData = eventData;
        }
    }

    public struct MapDrag {
        public PointerEventData EventData {get; private set;}

        public MapDrag(PointerEventData eventData) {
            EventData = eventData;
        }
    }

    public struct EndMapDrag {
        public PointerEventData EventData {get; private set;}

        public EndMapDrag(PointerEventData eventData) {
            EventData = eventData;
        }
    }
}