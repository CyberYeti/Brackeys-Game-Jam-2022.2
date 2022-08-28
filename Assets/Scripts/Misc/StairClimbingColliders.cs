using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairClimbingColliders : MonoBehaviour
{
    [SerializeField] private GroundCollider bottomCollider;
    [SerializeField] private GroundCollider topCollider;

    public bool CanClimbStairs
    {
        get { return bottomCollider.TouchingGround && !topCollider.TouchingGround; }
    }
}
