using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This file draws a line from transform 1 to transform 2

public class GizmoLink : MonoBehaviour
{
    [SerializeField] private Color gizmoColor = Color.blue;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;

    void OnDrawGizmos(){
        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(point1.position, point2.position);
    }
}
