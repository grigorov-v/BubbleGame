using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProcess {
    public class BubblesGun : MonoBehaviour {
        [SerializeField] float     _force         = 10;
        [SerializeField] Transform _bubblesCenter = null;
        [SerializeField] Bubble    _bubble        = null;
        [SerializeField] Transform _body          = null;

        [SerializeField] float     _lerpMove      = 10f;

        private void Update() {
            if ( Input.GetMouseButton(0) ) {
                var worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                if ( _body.position.y >= worldMousePos.y ) {
                    return;
                }

                var dir = _body.position - worldMousePos;
                dir = -(Vector2)dir;
                //_body.up = dir;
                _body.up = Vector2.Lerp(_body.up, dir, _lerpMove * Time.deltaTime);
            }

            if ( Input.GetMouseButtonUp(0) ) {
                Shot();
            }
        }

        void Shot() {
            _bubble.Rigidbody.simulated = true;
            _bubble.Force = _force;
            _bubble.IsDeactivate = false;
            _bubble.BubbleFromGun = true;
            _bubble.Rigidbody.AddForce(_bubblesCenter.up * _force, ForceMode2D.Impulse);
        }
    }
}