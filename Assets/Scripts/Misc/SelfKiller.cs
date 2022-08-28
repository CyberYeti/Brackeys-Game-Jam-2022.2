using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfKiller : MonoBehaviour
{
    [SerializeField] private float killDelay = 0;

    private float startTime = 0;
    private void Awake()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time-startTime > killDelay)
            Destroy(gameObject);
    }
}
