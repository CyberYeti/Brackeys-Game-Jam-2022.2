/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DoorSettings : MonoBehaviour
{
    [SerializeField] private GameObject doorBottomImg;
    [SerializeField] private GameObject doorTopImg;
    [SerializeField] private GameObject doorImg;
    [SerializeField] private GameObject triggerImg;

    [SerializeField] private int seperatingDistance = 2;
    [SerializeField] private Color doorColor = Color.white;

    private void Update()
    {
        if (EditorApplication.isPlaying) return;

        DoorController doorController = GetComponent<DoorController>();
        if (doorController.DefualtOpen == true)
            doorImg.transform.localScale = Vector3.zero;
        else
            doorController.CloseDoor();

        //Set color
        doorBottomImg.GetComponent<SpriteRenderer>().color = doorColor;
        doorTopImg.GetComponent<SpriteRenderer>().color = doorColor;
        doorImg.GetComponent<SpriteRenderer>().color = doorColor;
        triggerImg.GetComponent<SpriteRenderer>().color = doorColor;

        //Set distance
        doorTopImg.transform.localPosition = new Vector3(0, seperatingDistance-1, 0);
    }
}
*/