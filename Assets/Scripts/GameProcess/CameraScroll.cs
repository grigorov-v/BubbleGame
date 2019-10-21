using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProcess {
    public class CameraScroll : MonoBehaviour {
        [SerializeField] float _speedScroll = 10f;
        [SerializeField] float _minY        = 0;
        [SerializeField] float _maxY        = 0;

        float _startYPosCamera = 0;
        float _startYPosMouse  = 0;

        void LateUpdate() {
            if ( Input.GetMouseButtonDown(0) ) {
                _startYPosCamera = transform.position.y;
                _startYPosMouse = Input.mousePosition.y;
            }

            if ( Input.GetMouseButton(0) ) {
                var curYMouse = Input.mousePosition.y;
                var yOffset = _startYPosMouse - curYMouse;

                var newPosCamera = transform.position;
                newPosCamera.y = _startYPosCamera + yOffset * _speedScroll;
                newPosCamera.y = Mathf.Clamp(newPosCamera.y, _minY, _maxY);
                transform.position = newPosCamera;
            }
        }
    }
}
