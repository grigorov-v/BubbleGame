using GameProcess;
using UnityEngine;

namespace Core.Events {
    public struct PostBubbleCollision {
        public Bubble      Bubble    {get; private set;}
        public Collision2D Collision {get; private set;}

        public PostBubbleCollision(Bubble bubble, Collision2D collision) {
            Bubble    = bubble;
            Collision = collision;
        }
    }
}