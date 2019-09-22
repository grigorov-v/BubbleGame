using System.Collections.Generic;
using UnityEngine;

namespace GameProcess {
    public class BubbleGunIndicator : MonoBehaviour {
        [SerializeField] Transform       _point                 = null;
        [SerializeField] Transform       _startTransform        = null;
        [SerializeField] float           _distanceBetweenPoints = 0.3f;
        [SerializeField] int             _maxCountPoints        = 10;

        List<Transform> _points = new List<Transform>();
 
        private void Awake() {
            Init();
        }

        void Init() {
            _point.SetParent(null);
            _point.gameObject.SetActive(false);
            
            _points.Add(_point);
            for (int i = 1; i < _maxCountPoints; i++) {
                var newPoint = Instantiate(_point, _point.parent);
                _points.Add(newPoint);
                newPoint.gameObject.SetActive(false);
            }
        }

        public void SetActiveIndicator(bool isActive) {
            gameObject.SetActive(isActive);

            if ( !isActive ) {
                _points.ForEach(point => point.gameObject.SetActive(false));
            }
        }

        void DrawDefaultLine(int maxCountPoints = 7) {
            for (var i = 0; i < _points.Count; i++) {
                if ( i >= maxCountPoints ) {
                    _points[i].gameObject.SetActive(false);
                    continue;
                }

                var pointPosition = _startTransform.position;
                pointPosition += _startTransform.up * _distanceBetweenPoints * i;

                _points[i].position = pointPosition;
                _points[i].up = Vector2.up;
                _points[i].gameObject.SetActive(true);
            }
        }

        void DrawDefaultAndReflectLine(RaycastHit2D raycastHit) {
            var countDefaultPoints = CountPointsBetweenPositions(_startTransform.position, raycastHit.point);
            DrawDefaultLine(countDefaultPoints);
            
            var reflectDir = Vector2.Reflect(transform.up, raycastHit.normal);
            var indexPointReflect = 0;
            for (var i = countDefaultPoints; i < _points.Count; i++) {
                var pointPosition = raycastHit.point;
                pointPosition += reflectDir * _distanceBetweenPoints * indexPointReflect;

                _points[i].position = pointPosition;
                _points[i].up = Vector2.up;
                _points[i].gameObject.SetActive(true);
                
                indexPointReflect ++;
            }
        }

        public bool IsDrawReflectLine(RaycastHit2D raycastHit) {
            var collider = raycastHit.collider;
            if ( !collider ) {
                return false;
            }

            return collider.CompareTag("LeftWall") || collider.CompareTag("RightWall");
        }

        int CountPointsBetweenPositions(Vector2 pos1, Vector2 pos2) {
            var maxCountPoints = Vector2.Distance(pos1, pos2) / _distanceBetweenPoints;
            return Mathf.RoundToInt(maxCountPoints);
        }

        private void Update() {
            var raycastHit = Physics2D.Raycast(transform.position, transform.up);
            var isDrawReflectLine = IsDrawReflectLine(raycastHit);
            if ( !isDrawReflectLine ) {
                DrawDefaultLine();
            } else {
                DrawDefaultAndReflectLine(raycastHit);
            }
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