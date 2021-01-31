using System;
using UnityEngine;

public class Office : MonoBehaviour
{
    public GameObject prompt;
    public GameManager gameManager;
    public AudioSource source;
    public AudioClip clip;
    public float completionRate;
    private ObstacleManager _obstacleManager;

    private void Awake()
    {
        _obstacleManager = FindObjectOfType<ObstacleManager>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null) return;
        prompt.SetActive(true);
        if (Input.GetKey(KeyCode.E))
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(clip);
            }
            gameManager.AdvanceMission(completionRate);
            prompt.SetActive(false);
        }
        else
        {
            source.Stop();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>() == null) return;
        source.Stop();
        prompt.SetActive(false);
    }
}
