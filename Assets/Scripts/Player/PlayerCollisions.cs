using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] private GameObject deathEffectPF;
    [SerializeField] private Sprite neutralPosition;

    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
    }

    private bool reachedPortal = false;
    public bool ReachedPortal
    {
        get { return reachedPortal; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == null) { return; }
        if (collision.gameObject.layer == 6)//enemy layer
        {
            isDead = true;
            FindObjectOfType<AudioManager>().PlayDeathSound();
            GameObject deathEffect = Instantiate(deathEffectPF);
            deathEffect.transform.position = transform.position;
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal")//portal
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = neutralPosition;
            reachedPortal = true;
        }
    }
}
