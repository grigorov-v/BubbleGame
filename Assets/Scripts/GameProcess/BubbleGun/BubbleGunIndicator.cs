using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProcess {
    public class BubbleGunIndicator : MonoBehaviour {
        public void SetActiveIndicator(bool isActive) {
            gameObject.SetActive(isActive);
        }

        // private Vector2 CalculatePosition(float elapsedTime, Vector2 startPosition, Rigidbody2D rb) {
        //     var GRAVITY = Physics2D.gravity * rb.gravityScale;
        //     var INITIAL_POSITION = startPosition;
        //     var LAUNCH_VELOCITY = rb.velocity;
        //     return GRAVITY * elapsedTime * elapsedTime * 0.5f +
        //        LAUNCH_VELOCITY * elapsedTime + INITIAL_POSITION;
        // }
    }
}