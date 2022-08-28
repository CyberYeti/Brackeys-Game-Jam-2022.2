using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    private bool touchingGround = false;
    public bool TouchingGround
    {
        get { return touchingGround; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)//Ground layer
        {
            touchingGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)//Ground layer
        {
            touchingGround = false;
        }
    }
}
