using System.Collections.Generic;
using UnityEngine;

namespace GameProcess {
    public class Bubble : MonoBehaviour {
        const string DEACTIVATE_ANIMATION_NAME = "BubbleBurst";

        static Dictionary<GameObject, Bubble> _cache = new Dictionary<GameObject, Bubble>();

        public bool   BubbleFromGun = false;

        [SerializeField] BubbleTags         _bubbleTag    = BubbleTags.None;
        [SerializeField] List<BubbleReward> _bubbleRewards = new List<BubbleReward>();

        List<Bubble> _connectedBubbles = new List<Bubble>();
        Animator     _animator         = null;
        bool         _isDeactivate     = false;

        public BubbleReward ActiveBubbleReward {get; private set;}

        static void AddToCache(GameObject key, Bubble bubble) {
            if ( _cache.ContainsKey(key) ) {
                return;
            }

            _cache.Add(key, bubble);
        }

        static void RemoveToCache(GameObject key) {
            if ( !_cache.ContainsKey(key) ) {
                return;
            }

            _cache.Remove(key);
        }

        static Bubble FindToCache(GameObject key) {
            if ( !_cache.ContainsKey(key) ) {
                return null;
            }

            return _cache[key];
        }

        void SetBubbleReward() {
            foreach (var bubbleReward in _bubbleRewards) {
                var isActive = (bubbleReward.BubbleTag == _bubbleTag);
                bubbleReward.gameObject.SetActive(isActive);

                if ( isActive ) {
                    ActiveBubbleReward = bubbleReward;
                }
            }
        }

        private void OnValidate() {
            if ( Application.isPlaying ) {
                return;
            }
            
            SetBubbleReward();
        }

        private void Awake() {
            AddToCache(gameObject, this);
            _animator = GetComponent<Animator>();

            SetBubbleReward();
        }

        private void OnDestroy() {
            RemoveToCache(gameObject);
        }

        public bool CheckConnectedBubble(Bubble bubble) {
            return _connectedBubbles.IndexOf(bubble) != -1;
        }

        public bool CheckAllConnectedBubble(Bubble bubble) {
            if ( CheckConnectedBubble(bubble) ) {
                return true;
            }

            foreach ( var curBubble in _connectedBubbles ) {
                if ( curBubble.CheckConnectedBubble(bubble) ) {
                    return true;
                }
            }

            return false;
        }

        void TryAddConnectedBubble(Bubble bubble) {
            if ( !bubble ) {
                return;
            }

            if ( (bubble._bubbleTag != _bubbleTag) || (bubble == this) ) {
                return;
            }

            if ( CheckAllConnectedBubble(bubble) ) {
                return;
            }

            _connectedBubbles.Add(bubble);
        }

        void TryRemoveConnectedBubble(Bubble bubble) {
            if ( !bubble ) {
                return;
            }

            if ( !CheckConnectedBubble(bubble) ) {
                return;
            }

            _connectedBubbles.Remove(bubble);
        }

        private void OnTriggerStay2D(Collider2D other) {
            var bubble = FindToCache(other.gameObject);
            TryAddConnectedBubble(bubble);

            if ( BubbleFromGun ) {
                TryDeactivateAllConnectedBubbles();
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var bubble = FindToCache(other.gameObject);
            TryRemoveConnectedBubble(bubble);
        }

        [ContextMenu("DestroyAll")]
        public void TryDeactivateAllConnectedBubbles() {
            var count = GetCountConnectedBubbles();
            if ( count < 3 ) {
                return;
            }

            RecursiveDeactivateConnectedBubbles(_connectedBubbles);

            if ( _connectedBubbles.Count > 0 ) {
                PlayDeactivateAnimation();
            }
        }

        [ContextMenu("PlayBurstAnimation")]
        void PlayDeactivateAnimation() {
            if ( _isDeactivate ) {
                return;
            }

            _isDeactivate = true;
            _animator.Play(DEACTIVATE_ANIMATION_NAME);
        }

        void PlayReward() {
            if ( !ActiveBubbleReward ) {
                return;
            }

            ActiveBubbleReward.transform.SetParent(null);
            ActiveBubbleReward.SetDefaultColor();
            ActiveBubbleReward.gameObject.AddComponent<Rigidbody2D>();
        }

        int GetCountConnectedBubbles() {
            var count = _connectedBubbles.Count;
            foreach ( var bub in _connectedBubbles ) {
                if ( !bub.gameObject.activeSelf ) {
                    continue;
                }

                if ( bub._isDeactivate ) {
                    continue;
                }

                count += bub._connectedBubbles.Count;
            }

            return count;
        }

        public void DeactivateBetweenAnimation() {
            if ( !_isDeactivate ) {
                return;
            }

            PlayReward();

            gameObject.SetActive(false);
        }

        static void RecursiveDeactivateConnectedBubbles(List<Bubble> bubbles) {
            foreach ( var bub in bubbles ) {
                if ( !bub.gameObject.activeSelf ) {
                    continue;
                }

                if ( bub._isDeactivate ) {
                    continue;
                }

                bub.PlayDeactivateAnimation();
                RecursiveDeactivateConnectedBubbles(bub._connectedBubbles);
            }
        }
    }
}