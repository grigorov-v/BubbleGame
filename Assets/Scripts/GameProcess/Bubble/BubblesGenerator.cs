﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

using Configs;
using Controllers;

namespace GameProcess {
    public class BubblesGenerator : MonoBehaviour {
        [SerializeField] Bubble    _bubblePrototype = null;
        [SerializeField] Transform _startPoint      = null;
        [SerializeField] int       _columnCount     = 8;

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
                var position = _startPoint.position;
                var scale = _bubblePrototype.transform.localScale;

                position.x += scale.x * column;
                position.y -= scale.y * line;

                var bubble = Instantiate(_bubblePrototype, position, _bubblePrototype.transform.rotation, _bubblePrototype.transform.parent);
                bubble.UpdateBubbleReward(bubbleInfo.Tag);

                if ( column >= (_columnCount - 1) ) {
                    column = 0;
                    line ++;
                    continue;
                }

                column ++;
            }

            Destroy(_bubblePrototype.gameObject);
        }
    }
}