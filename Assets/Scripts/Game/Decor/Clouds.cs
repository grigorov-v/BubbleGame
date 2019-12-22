using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using NaughtyAttributes;

namespace Game {
    public class Clouds : MonoBehaviour {
        [SerializeField] List<Transform> _clouds = new List<Transform>();
        [SerializeField] Vector2         _size   = Vector2.one;
        [SerializeField] float           _speed  = 10;

        Vector2 LeftTopPoint {
            get {
                var x = transform.position.x - (_size.x / 2);
                var y = transform.position.y + (_size.y / 2);
                return new Vector2(x, y);
            }
        }

        Vector2 RightBottomPoint {
            get {
                var x = transform.position.x + (_size.x / 2);
                var y = transform.position.y - (_size.y / 2);
                return new Vector2(x, y);
            }
        }

        float MinX {
            get {
                return LeftTopPoint.x;
            }
        }

        float MaxX {
            get {
                return RightBottomPoint.x;
            }
        }

        float MinY {
            get {
                return RightBottomPoint.y;
            }
        }

        float MaxY {
            get {
                return LeftTopPoint.y;
            }
        }

        void Update() {
            foreach (var cloud in _clouds) {
                cloud.Translate(Vector2.right * _speed * Time.deltaTime);
                if ( cloud.position.x >= MaxX ) {
                    ResetXPosition(cloud);
                    RandomizeYPosition(cloud);
                }
             }
        }

        void ResetXPosition(Transform cloud) {
            var pos = cloud.position;
            pos.x = MinX;
            cloud.position = pos;
        }

        void RandomizeYPosition(Transform cloud) {
            var pos = cloud.position;
            pos.y = Random.Range(MinY, MaxY);
            cloud.position = pos;
        }

        void RandomizeXPosition(Transform cloud) {
            var pos = cloud.position;
            pos.x = Random.Range(MinX, MaxX);;
            cloud.position = pos;
        }

        void ClampPosition(Transform cloud) {
            var pos = cloud.position;

            pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
            pos.y = Mathf.Clamp(pos.y, MinY, MaxY);
            cloud.position = pos;
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, _size);
            _clouds.ForEach(cloud => ClampPosition(cloud));
        }

        [Button]
        void Randomize() {
            foreach ( var cloud in _clouds ) {
                RandomizeXPosition(cloud);
                RandomizeYPosition(cloud);
            }
        }
    }
}