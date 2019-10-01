using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

using Configs;
using Controllers;

namespace GameProcess {
    public class BubblesGenerator : MonoBehaviour {
        [SerializeField] Bubble    _bubble          = null;
        [SerializeField] Transform _startPoint      = null;
        [SerializeField] int       _horizontalCount = 8;

        private void Start() {
            Generate();
        }
 
        [Button("Generate")]
        void Generate() {
            var levelConfig = ConfigsController.Instance.FindConfig<LevelConfig>();
            var bubbles = levelConfig.Levels[0].Bubbles;

            foreach (var bubbleInfo in bubbles) {
                var position = _startPoint.position;
                var scale = _bubble.transform.localScale;

                position.x += scale.x * bubbleInfo.Column;
                position.y -= scale.y * bubbleInfo.Line;

                var bubble = Instantiate(_bubble, position, _bubble.transform.rotation, _bubble.transform.parent);
                bubble.SetBubbleTag(bubbleInfo.Tag);
                bubble.UpdateBubbleReward();
            }
        }
    }
}