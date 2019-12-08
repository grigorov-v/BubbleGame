using UnityEngine;
using UnityEngine.EventSystems;

namespace GameProcess.Events {
    public struct PostBubbleCollision {
        public Bubble      Bubble    {get; private set;}
        public Collision2D Collision {get; private set;}

        public PostBubbleCollision(Bubble bubble, Collision2D collision) {
            Bubble    = bubble;
            Collision = collision;
        }
    }

    //================
    //Map drag
    //================
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