using UnityEngine;

using Core.Events;
using Game.Events;

using DG.Tweening;
using NaughtyAttributes;

namespace Game {
    public class CameraScroll : MonoBehaviour {
        [SerializeField] float _speedScroll     = 0.004f;

        [Header("Clamp camera position")]
        [SerializeField] float _minY            = 0;
        [SerializeField] float _maxY            = 0;

        [Header("Elastic settings")]
        [SerializeField] float _elastic         = 0;
        [SerializeField] float _elasticDuration = 0.5f;

        Tween _tweenElastic = null;

        void Awake() {
            EventManager.Subscribe<MapDrag>(this, OnMapDrag);
            EventManager.Subscribe<EndMapDrag>(this, OnEndMapDrag);
        }

        void OnDestroy() {
            EventManager.Unsubscribe<MapDrag>(OnMapDrag);
            EventManager.Unsubscribe<EndMapDrag>(OnEndMapDrag);
        }

        void KillTweenElastic() {
            if ( _tweenElastic == null ) {
                return;
            }
            
            _tweenElastic.Kill();
        }

        void UpdateElastic() {
            KillTweenElastic();

            var endValue = transform.position.y;
            if ( endValue > _maxY ) {
                endValue = _maxY;
            } else if ( endValue < _minY ) {
                endValue = _minY;
            }

            _tweenElastic = transform.DOMoveY(endValue, _elasticDuration);
        }

        void OnMapDrag(MapDrag e) {            
            var newPosCamera = transform.position;
            newPosCamera.y -= e.EventData.delta.y * _speedScroll;
            
            var maxY = _maxY + _elastic;
            var minY = _minY - _elastic;
            newPosCamera.y = Mathf.Clamp(newPosCamera.y, minY, maxY);
            transform.position = newPosCamera;
        }

        void OnEndMapDrag(EndMapDrag e) {
            UpdateElastic();
        }

        [Button]
        void RememmberMinY() {
            _minY = transform.position.y;
        }

        [Button]
        void RememmberMaxY() {
            _maxY = transform.position.y;
        }
    }
}
