using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

using Configs;
using Controllers;

namespace Game.Bubbles {
    public class BubblesGenerator : MonoBehaviour {
        [SerializeField] Bubble    _bubblePrototype = null;
        [SerializeField] Transform _startPoint      = null;
        [SerializeField] int       _columnCount     = 9;
        [SerializeField] float     _startOffset     = 0.4f;
 
        private void Start() {
            Generate();
        }
 
        [Button("Generate")]
        void Generate() {
            var levelConfig = ConfigsController.Instance.FindConfig<LevelConfig>();
            var bubbles = levelConfig.Levels[0].Bubbles;

            var column = 0;
            var line = 0;

            foreach (var bubbleInfo in bubbles) {
                for ( int i = 0; i < bubbleInfo.Count; i++ ) {
                    var position = _startPoint.position;
                    var scale = _bubblePrototype.transform.localScale;

                    position.x += scale.x * column;
                    position.y -= scale.y * line;

                    position.x += _startOffset;
                    position.y -= _startOffset;

                    var bubble = CreateNewBubble(_bubblePrototype);
                    bubble.transform.position = position;
                    bubble.UpdateBubbleReward(bubbleInfo.Tag);

                    if ( column >= (_columnCount - 1) ) {
                        column = 0;
                        line ++;
                        continue;
                    }

                    column ++;
                }
            }

            Destroy(_bubblePrototype.gameObject);
        }

        public static Bubble CreateNewBubble(Bubble prototype) {
            var newBubble = Instantiate(prototype, prototype.transform.parent);
            newBubble.name = string.Format("Bubble[{0}]", newBubble.BubbleTag);
            return newBubble;
        }
    }
}