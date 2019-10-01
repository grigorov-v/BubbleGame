using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Events;

namespace GameProcess {
    public class BubblesGun : MonoBehaviour {
        [SerializeField] float              _force         = 10;
        [SerializeField] Transform          _bubblesCenter = null;
        [SerializeField] Transform          _body          = null;
        [SerializeField] float              _lerpMove      = 10f;
        [SerializeField] Bubble             _lastBubble    = null;
        [SerializeField] BubbleGunIndicator _indicator     = null;

        private void Start() {
            EventManager.Subscribe<PostBubbleCollision>(this, OnPostBubbleCollision);
        }

        private void OnDestroy() {
            EventManager.Unsubscribe<PostBubbleCollision>(OnPostBubbleCollision);
        }

        private void Update() {
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
            _lastBubble.Rigidbody.simulated = true;
            _lastBubble.Rigidbody.gravityScale = 0;
            _lastBubble.Rigidbody.angularDrag = 0;

            _lastBubble.Force = _force;
            _lastBubble.BubbleFromGun = true;
            _lastBubble.transform.SetParent(null);

            _lastBubble.Rigidbody.AddForce(_bubblesCenter.up * _force, ForceMode2D.Impulse);
        }

        void ReloadBubble() {
            _lastBubble = _lastBubble.CopyBubble();//Когда будет ограниченное кол-во, брать из пула

            _lastBubble.Init();
            _lastBubble.transform.SetParent(_bubblesCenter);
            _lastBubble.transform.localPosition = Vector2.zero;
            _lastBubble.Rigidbody.simulated = false;
            _lastBubble.Force = 0;
            _lastBubble.RandomUpdateBubbleReward();
        }

        void OnPostBubbleCollision(PostBubbleCollision e) {
            if ( e.Bubble != _lastBubble ) {
                return;
            }

            ReloadBubble();
        }
    }
}