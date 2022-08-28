using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform topMarker;
    [SerializeField] private Transform bottomMarker;
    [SerializeField] private Transform doorImg;

    [SerializeField] private Trigger trigger;

    [SerializeField] private bool defualtOpen = false;
    [SerializeField] private float speed = 3;
    [Range(0,1)] private float openValue = 0;
    private bool isOpen = false;

    public bool DefualtOpen
    {
        get { return defualtOpen; }
    }

    public void CloseDoor()
    {
        float doorHeight = Mathf.Abs(topMarker.localPosition.y - bottomMarker.localPosition.y);
        doorImg.localPosition = Vector3.Lerp(bottomMarker.localPosition, (topMarker.localPosition + bottomMarker.localPosition) / 2, 1);
        doorImg.localScale = new Vector3(1, doorHeight * 1, 1);
    }

    private void Update()
    {
        if (trigger == null) { print("No Trigger Assigned to " + gameObject.name); return; }

        if (trigger.Triggered)
            isOpen = !defualtOpen;
        else
            isOpen = defualtOpen;

        if (isOpen)
        {
            openValue = Mathf.Clamp(openValue-(speed*Time.deltaTime), 0, 1);
        }
        else
        {
            openValue = Mathf.Clamp(openValue + (speed * Time.deltaTime), 0, 1);
        }
        
        //Update Door Value
        float doorHeight = Mathf.Abs(topMarker.localPosition.y - bottomMarker.localPosition.y);
        doorImg.localPosition = Vector3.Lerp(bottomMarker.localPosition, (topMarker.localPosition + bottomMarker.localPosition) /2, openValue);
        doorImg.localScale = new Vector3(1, doorHeight * openValue, 1);
    }
}
