using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace GameProcess {
    public enum BubbleTags {
        None,
        Lemon,
        PurpleLollipop,
        Bomb,
        Heart,
        PinkLollipop,
        BlueLollipop,
        WhiteLollipop,
        GreenLollipop,
        Star,
        RedСandy,
        ОrangeСandy,
        СherryСandy,
        BluePop,
        Lightning,
        StripedLollipop,
        Cake,
        Clock,
        CaramelLollipop
    }

    public class BubbleReward : MonoBehaviour {
        const float FREEZE_ALPHA = 0.7f;

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

        public void SetFreezeColor() {
            _renderer.color = new Color(1, 1, 1, FREEZE_ALPHA);
        }

        public void SetDefaultColor() {
            _renderer.color = new Color(1, 1, 1, 1);
        }

        public void PlayRefwardAnimation(Vector2 endPos, float duration = 0.5f) {
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