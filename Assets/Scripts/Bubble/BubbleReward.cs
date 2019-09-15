using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField] BubbleTags _bubbleTag = BubbleTags.None;

        SpriteRenderer _renderer = null;

        public BubbleTags BubbleTag {
            get {
                return _bubbleTag;
            }
        }

        private void Awake() {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void SetFreezeColor() {
            _renderer.color = new Color(1, 1, 1, FREEZE_ALPHA);
        }

        public void SetDefaultColor() {
            _renderer.color = new Color(1, 1, 1, 1);
        }
    }
}