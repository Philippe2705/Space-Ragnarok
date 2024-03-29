﻿using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    void OnDrawGizmos()
    {
        DrawArrow.ForGizmo(transform.position, transform.right);
#if UNITY_EDITOR
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, 0.05f);
#endif
    }
}
