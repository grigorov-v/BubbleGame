using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace GameProcess {
    public class CloudsAnimation : MonoBehaviour {
        [SerializeField] List<Transform> _clouds   = new List<Transform>();
        
        // [Header("Random Y position")]
        // [SerializeField] float           _minYPos  = 0;
        // [SerializeField] float           _maxYPos  = 0;

        [Header("Clamp X position")]
        [SerializeField] float           _minXPos  = 0;
        [SerializeField] float           _maxXPos  = 0;

        [Header("Duration tween position")]
        [SerializeField] float           _speed = 10;

        private void Start() {
            foreach (var cloud in _clouds) {
                InitTween(cloud);
            }
        }

        void InitTween(Transform cloud) {
            cloud.DOLocalMoveX(_maxXPos, _speed * Time.deltaTime)
                .SetEase(Ease.Linear).SetSpeedBased(true)
                .onKill += ()=> {
                    if ( !cloud ) {
                        return;
                    }

                    var startPos = cloud.localPosition;
                    startPos.x = _minXPos;
                    cloud.localPosition = startPos;
                    InitTween(cloud);
                };
        }
    }
}