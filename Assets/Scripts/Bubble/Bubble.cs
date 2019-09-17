﻿using System.Collections.Generic;
using UnityEngine;
using System;

using Core.Events;

namespace GameProcess {
    public class Bubble : MonoBehaviour {
        const string DEACTIVATE_ANIMATION_NAME = "BubbleBurst";
        const string STAY_ANIMATION_NAME       = "Stay";

        public static Dictionary<GameObject, Bubble> CacheBubbles {get; private set;}

        [SerializeField] BubbleTags         _bubbleTag     = BubbleTags.None;
        [SerializeField] List<BubbleReward> _bubbleRewards = new List<BubbleReward>();

        List<Bubble> _connectedBubbles = new List<Bubble>();
        Animator     _animator         = null;
        Action       _collisionAction  = null;
        bool         _init             = false;
      
        public BubbleReward ActiveBubbleReward {get; private set;}
        public Rigidbody2D  Rigidbody          {get; private set;}
        public float        Force              {get; set;}
        public bool         IsDeactivate       {get; set;}
        public bool         BubbleFromGun      {get; set;}

        static void AddToCache(GameObject key, Bubble bubble) {
            if ( CacheBubbles == null ) {
                CacheBubbles = new Dictionary<GameObject, Bubble>();
            }

            if ( CacheBubbles.ContainsKey(key) ) {
                return;
            }

            CacheBubbles.Add(key, bubble);
        }

        static void RemoveToCache(GameObject key) {
            if ( CacheBubbles == null ) {
                return;
            }

            if ( !CacheBubbles.ContainsKey(key) ) {
                return;
            }

            CacheBubbles.Remove(key);
        }

        static Bubble FindToCache(GameObject key) {
            if ( !CacheBubbles.ContainsKey(key) ) {
                return null;
            }

            return CacheBubbles[key];
        }

        public static Bubble GetFreeBubble() {
            foreach (var cacheItem in CacheBubbles) {
                var bubble = cacheItem.Value;
                if ( bubble.gameObject.activeSelf ) {
                    continue;
                }

                return bubble;
            }

            return null;
        }

        public void SetBubbleTag(BubbleTags tag) {
            _bubbleTag = tag;
        }

        public void UpdateBubbleReward() {
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
            
            UpdateBubbleReward();
        }

        private void Awake() {
            Init();
        }

        private void OnDestroy() {
            RemoveToCache(gameObject);
        }

        public void Init() {
            if ( _init ) {
                return;
            }

            AddToCache(gameObject, this);
            _animator = GetComponent<Animator>();

            UpdateBubbleReward();
            Rigidbody = GetComponent<Rigidbody2D>();
            _init = true;
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

        private void OnCollisionEnter2D(Collision2D other) {
            var normale = Vector2.zero;

            if ( other.gameObject.CompareTag("LeftWall") ) {
                normale = other.transform.right;
            } else if ( other.gameObject.CompareTag("RightWall") ) {
                normale = -other.transform.right;
            } else if ( other.gameObject.CompareTag("Bubble") ) {
                Rigidbody.velocity = Vector2.zero;
                Force = 0;
            }
            
            Rigidbody.AddForce(normale * Force, ForceMode2D.Impulse);
            EventManager.Fire(new PostBubbleCollision(this, other));
        }

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

        void PlayDeactivateAnimation() {
            if ( IsDeactivate ) {
                return;
            }

            IsDeactivate = true;
            _animator.Play(DEACTIVATE_ANIMATION_NAME);
        }

        public void PlayStayAnimation() {
            _animator.Play(STAY_ANIMATION_NAME);
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

                if ( bub.IsDeactivate ) {
                    continue;
                }

                count += bub._connectedBubbles.Count;
            }

            return count;
        }

        public void DeactivateBetweenAnimation() {
            if ( !IsDeactivate ) {
                return;
            }

            // gameObject.SetActive(false);
            Destroy(gameObject);
        }

        public static BubbleTags GetRandomBubbleTags() {
            var count = Enum.GetNames(typeof(BubbleTags)).Length;
            var rand = UnityEngine.Random.Range(1, count);
            return (BubbleTags)rand;
        }

        static void RecursiveDeactivateConnectedBubbles(List<Bubble> bubbles) {
            foreach ( var bub in bubbles ) {
                if ( !bub ) {
                    continue;
                }
                
                if ( !bub.gameObject.activeSelf ) {
                    continue;
                }

                if ( bub.IsDeactivate ) {
                    continue;
                }

                bub.PlayDeactivateAnimation();
                RecursiveDeactivateConnectedBubbles(bub._connectedBubbles);
            }
        }
    }
}