using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FMOD : MonoBehaviour
{

    public AudioClip[] ambiances;
    public AudioSource ambSource0;
    public AudioSource ambSource1;
    public Player player;

    private bool firstSourcePlaying = true;
    public bool playAtStart = true;
    
    public int currentStressLevel = 0;
    public int previousStressLevel = 0;
    public float crossFadeTime = 9;
    
    private void Start()
    {
        if (playAtStart)
        {
            StartPlaying();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            StressDown();
        if (Input.GetKeyDown(KeyCode.Q))
            StressUp();
    }

    public void StartPlaying()
    {
        if (firstSourcePlaying)
        {
            ambSource0.clip = ambiances[currentStressLevel];
            ambSource0.volume = 1;
            ambSource1.volume = 0;
            ambSource0.Play();

        }
        else
        {
            ambSource1.clip = ambiances[currentStressLevel];
            ambSource1.volume = 1;
            ambSource0.volume = 0;
            ambSource1.Play();
        }
    }
    
    public void StressUp()
    {
        if (currentStressLevel == ambiances.Length - 1)
            return;
        
        currentStressLevel++;
        ChangeToStressLevel(currentStressLevel);
    }

    public void StressDown()
    {
        if (currentStressLevel == 0)
            return;
        
        currentStressLevel--;
        ChangeToStressLevel(currentStressLevel);
    }
    public void ChangeToStressLevel(int level)
    {
        if (firstSourcePlaying)
        {
            ambSource1.clip = ambiances[level];
            ambSource1.time = ambSource0.time;
            ambSource1.DOFade(0.7f, crossFadeTime);
            ambSource0.DOFade(0, crossFadeTime).OnComplete(ambSource0.Stop);
            ambSource1.Play();
        }
        else
        {
            ambSource0.clip = ambiances[level];
            ambSource0.time = ambSource1.time;
            ambSource0.DOFade(0.7f, crossFadeTime);
            ambSource1.DOFade(0, crossFadeTime).OnComplete(ambSource1.Stop);
            ambSource0.Play();
        }

        firstSourcePlaying = !firstSourcePlaying;
    }
}
