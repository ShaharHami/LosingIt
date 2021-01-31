using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Getaway : MonoBehaviour
{
    public float sanityBumpRate;
    public GameObject prompt;
    public AudioSource source;
    public AudioClip clip;
    private void OnTriggerStay2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            prompt.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(clip);
                }
                player.IncreaseSanity(sanityBumpRate);
                prompt.SetActive(false);
            }
            else
            {
                source.Stop();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        source.Stop();
        prompt.SetActive(false);
    }
}
