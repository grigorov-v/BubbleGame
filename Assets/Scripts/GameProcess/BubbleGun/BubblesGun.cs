using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Events;

using GameProcess.Events;
using Controllers;

namespace GameProcess {
    public class BubblesGun : MonoBehaviour {
        [SerializeField] float              _force         = 10;
        [SerializeField] Transform          _bubblesCenter = null;
        [SerializeField] Transform          _body          = null;
        [SerializeField] float              _lerpMove      = 10f;
        [SerializeField] Bubble             _lastBubble    = null;
        [SerializeField] BubbleGunIndicator _indicator     = null;

        Queue<string> _allBubbleTagsForGun = new Queue<string>();
        int           _lastGenerateCount   = 0;

        void Start() {
            EventManager.Subscribe<PostBubbleCollision>(this, OnPostBubbleCollision);

            var levelInfo = ConfigsController.Instance.GetLevelInfo(0);

            foreach ( var bubbleForGun in levelInfo.BubblesForGun ) {
                for ( int i = 0; i < bubbleForGun.Count; i++ ) {
                    _allBubbleTagsForGun.Enqueue(bubbleForGun.Tag);
                }
            }

            var newTag = GetNewBubbleTag();
            if ( String.IsNullOrEmpty(newTag) ) {
                return;
            }

            _lastBubble.UpdateBubbleReward(newTag)
                .PhysicsSimulated(false);
                
            _lastGenerateCount = levelInfo.LastGenerateCount;
        }

        void OnDestroy() {
            EventManager.Unsubscribe<PostBubbleCollision>(OnPostBubbleCollision);
        }

        void Update() {
            if ( Input.GetMouseButton(0) ) {
                var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                if ( _body.position.y >= worldMousePos.y ) {
                    return;
                }

                var dir = _body.position - worldMousePos;
                dir = -(Vector2)dir;
                _body.up = Vector2.Lerp(_body.up, dir, _lerpMove * Time.deltaTime);
            }

            if ( Input.GetMouseButtonUp(0) ) {
                Shot();
            }

            var isActiveIndicator = Input.GetMouseButton(0);
            _indicator.SetActiveIndicator(isActiveIndicator);
        }

        void Shot() {
            if ( !_lastBubble ) {
                return;
            }

            ResetAllGunFlags();
            
            _lastBubble.SetParent(null)
                .PhysicsSimulated(true)
                .SetGunFlag(true)
                .SetForce(_bubblesCenter.up, _force);
        }

        void ReloadGun() {
            var newTag = GetNewBubbleTag();
            newTag = String.IsNullOrEmpty(newTag) ? GetNewBubbleTagFromScene() : newTag;

            if ( String.IsNullOrEmpty(newTag) ) {
                _lastBubble = null;
                return;
            }

            _lastBubble = BubblesGenerator.CreateNewBubble(_lastBubble);
            _lastBubble.Init()
                .SetParent(_bubblesCenter)
                .SetLocalPosition(Vector2.zero)
                .PhysicsSimulated(false)
                .SetForce(Vector2.zero, 0)
                .UpdateBubbleReward(newTag);
        }

        string GetNewBubbleTag() {
            if ( (_allBubbleTagsForGun == null) || (_allBubbleTagsForGun.Count == 0) ) {
                return null;
            }

            return _allBubbleTagsForGun.Dequeue();
        }

        string GetNewBubbleTagFromScene() {
            if ( _lastGenerateCount == 0 ) {
                return null;
            }

            if ( Bubble.Cache.Count == 0 ) {
                return null;
            }

            _lastGenerateCount --;
            var bubbleTags = new List<string>();
            foreach (var bubbleFromCache in Bubble.Cache) {
                if ( bubbleFromCache.Value != _lastBubble ) {
                    bubbleTags.Add(bubbleFromCache.Value.BubbleTag);
                }
            }

            if ( bubbleTags.Count == 0 ) {
                return null;
            }

            var rand = UnityEngine.Random.Range(0, bubbleTags.Count);
            return bubbleTags[rand];
        }

        void OnPostBubbleCollision(PostBubbleCollision e) {
            if ( e.Bubble != _lastBubble ) {
                return;
            }

            ReloadGun();
        }

        void ResetAllGunFlags() {
            foreach (var item in Bubble.Cache) {
                var bubble = item.Value;
                if ( bubble == _lastBubble ) {
                    continue;
                }

                bubble.SetGunFlag(false);
            }
        }
    }
}