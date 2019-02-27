using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyGameObjectGizmo : MonoBehaviour
{
    [SerializeField] private float gizmoRadius = 0.1f;
    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
}
