/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GridSnap : MonoBehaviour
{
    [SerializeField] float xOffset = 0.5f;
    [SerializeField] float yOffset = 0.5f;

    private void Update()
    {
        if (EditorApplication.isPlaying) return;

        float xPos = Mathf.RoundToInt(transform.position.x - xOffset) + xOffset;
        float yPos = Mathf.RoundToInt(transform.position.y - yOffset) + yOffset;
        float zPos = 0;

        transform.position = new Vector3(xPos, yPos, zPos);
    }
}
*/