using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProcess {
    public class BubblesGun : MonoBehaviour {
        public float Force = 100;

        [SerializeField] Transform _bubblesCenter = null;
        [SerializeField] Bubble    _bubble        = null;

        void Shot() {
            _bubble.Rigidbody.simulated = true;
            _bubble.Rigidbody.AddForce(_bubblesCenter.up * Force, ForceMode2D.Impulse);
        }

        private void Update() {
            if ( Input.GetKeyDown(KeyCode.Space) ) {
                Shot();
            }
        }
    }
}