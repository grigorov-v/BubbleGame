using UnityEngine;
using Game.Bubbles;

namespace Game.Events {
    public struct BubbleCollision {
        public Bubble      Bubble    {get; private set;}
        public Collision2D Collision {get; private set;}

        public BubbleCollision(Bubble bubble, Collision2D collision) {
            Bubble = bubble;
            Collision = collision;
        }
    }

    public struct DeactivateBubble {
        public Bubble Bubble {get; private set;}

        public DeactivateBubble(Bubble bubble) {
            Bubble = bubble;
        }
    }

    public struct DestroyBubble {
        public string BubbleTag {get; private set;}
        
        public DestroyBubble(string tag) {
            BubbleTag = tag;
        }
    }
}