using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private TriggerModes triggerMode;
    [SerializeField] private SpriteRenderer graphics;
    [SerializeField] private Sprite offTrigger;
    [SerializeField] private Sprite onTrigger;

    private bool triggered = false;

    public bool Triggered
    {
        get { return triggered; }
    }

    private void Update()
    {
        if (triggered)
            graphics.sprite = onTrigger;
        else
            graphics.sprite = offTrigger;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            triggered = !triggered;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (triggerMode == TriggerModes.Lever) { return; }
        if (collision.gameObject.tag == "Player")
        {
            triggered = false;
        }
    }
}

public enum TriggerModes
{
    Button,
    Lever
}

