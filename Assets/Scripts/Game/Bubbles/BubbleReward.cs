﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Game.Bubbles {
    public class BubbleReward : MonoBehaviour {
        [SerializeField] string _bubbleTag = string.Empty;

        SpriteRenderer _renderer = null;
        Tween          _tween    = null;

        public string BubbleTag {
            get {
                return _bubbleTag;
            }
        }

        private void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
        }

        void OnDestroy() {
            if ( _tween != null ) {
                _tween.Complete();
                _tween = null;
            }
        }

        public void PlayRewardAnimation(Vector2 endPos, float duration = 0.5f) {
            if ( _tween != null ) {
                return;
            }

            _tween = transform.DOMove(endPos, duration);
            _tween.onComplete += () => {
                Destroy(gameObject);
            };
        }
    }
}