using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource JumpSound;

    public void PlayDeathSound()
    {
        deathSound.Play();
    }

    public void PlayJumpSound()
    {
        JumpSound.Play();
    }
}
