using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Events;

using Game.Events;
using Controllers;

using DG.Tweening;

namespace Game.Bubbles {
    public class BubblesGun : MonoBehaviour {
        [SerializeField] float              _force          = 10;
        [SerializeField] Transform          _bubblesCenter  = null;
        [SerializeField] Transform          _body           = null;
        [SerializeField] float              _lerpMove       = 10f;
        [SerializeField] Bubble             _lastBubble     = null;
        [SerializeField] BubbleGunIndicator _indicator      = null;
        [SerializeField] float              _reloadDuration = 0.5f;

        Queue<string> _allBubbleTagsForGun = new Queue<string>();
        Tween         _tweenReload         = null;
        bool          _canShot             = true;

        void Start() {
            var levelInfo = LevelController.Instance.LevelInfo;

            foreach ( var bubbleForGun in levelInfo.Bubbles ) {
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
                
            EventManager.Subscribe<BubbleCollision>(this, OnBubbleCollision);
        }

        void OnDestroy() {
            KillTween();
            EventManager.Unsubscribe<BubbleCollision>(OnBubbleCollision);
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
                _canShot = false;
            }

            var isActiveIndicator = Input.GetMouseButton(0);
            _indicator.SetActiveIndicator(isActiveIndicator);
        }

        void Shot() {
            if ( !_canShot || !_lastBubble ) {
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
            if ( String.IsNullOrEmpty(newTag) ) {
                _lastBubble = null;
                return;
            }

            _lastBubble = Bubble.CreateNewBubble(_lastBubble);
            _lastBubble.Init()
                .SetParent(_bubblesCenter)
                .SetLocalPosition(Vector2.zero)
                .PhysicsSimulated(false)
                .SetForce(Vector2.zero, 0)
                .UpdateBubbleReward(newTag);

            KillTween();
            var scaleTemp = _lastBubble.transform.localScale;
            _lastBubble.transform.localScale = Vector3.zero;
            _tweenReload = _lastBubble.transform.DOScale(scaleTemp, _reloadDuration)
                .SetEase(Ease.Linear).OnComplete(() => _canShot = true);
        }

        string GetNewBubbleTag() {
            if ( (_allBubbleTagsForGun == null) || (_allBubbleTagsForGun.Count == 0) ) {
                return null;
            }

            return _allBubbleTagsForGun.Dequeue();
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

        void KillTween() {
            if ( _tweenReload == null ) {
                return;
            }

            _tweenReload.Kill();
        }

        void OnBubbleCollision(BubbleCollision e) {
            if ( e.Bubble != _lastBubble ) {
                return;
            }

            ReloadGun();
        }
    }
}