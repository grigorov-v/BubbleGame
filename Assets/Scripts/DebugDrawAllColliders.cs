using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class DebugDrawAllColliders : MonoBehaviour {
    [SerializeField] Color _colorLines = Color.white;

    List<BoxCollider2D>    _boxColliders    = new List<BoxCollider2D>();
    List<CircleCollider2D> _circleColliders = new List<CircleCollider2D>();

    private void OnDrawGizmos() {
       
        _boxColliders = FindObjectsOfType<BoxCollider2D>().ToList();
        _circleColliders = FindObjectsOfType<CircleCollider2D>().ToList();
        

        Gizmos.color = _colorLines;
        foreach ( var collider in _boxColliders ) {
            var center = (Vector2)collider.transform.position + collider.offset;
            var size = collider.size;
            var diff = Vector2.one - (Vector2)collider.transform.localScale;
            size -= diff;
            Gizmos.DrawWireCube(center, size);
        }

        // foreach ( var collider in _circleColliders ) {
        //     var center = (Vector2)collider.transform.position + collider.offset;
        //     var radius = collider.radius;
        //     Gizmos.DrawWireSphere(center, radius);
        // }
    }
}
