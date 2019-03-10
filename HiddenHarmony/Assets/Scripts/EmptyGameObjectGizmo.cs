using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmptyGameObjectGizmo : MonoBehaviour
{
    [SerializeField] private float gizmoRadius = 0.1f;
    [SerializeField] private Color gizmoColor = Color.blue;
    void OnDrawGizmos(){
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
}
