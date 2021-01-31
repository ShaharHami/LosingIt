using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class DayNightLight : MonoBehaviour
{
    public GameManager gameManager;
    public Light2D globalLight;
    public Color dayColor;
    public Color nightColor;
    public float dayIntensity;
    public float nightIntensity;
    public float dayTime, nightTime;
    public float cycleStep;
    public float gameTime;
    public Image timeBar;
    private float timer;
    private float step;
    private bool stopCycle;

    private void Start()
    {
        step = cycleStep;
    }

    private void Update()
    {
        if (!stopCycle)
        {
            ChangeIntensity(step);
            globalLight.color = Color.Lerp(nightColor, dayColor, globalLight.intensity);
            if (globalLight.intensity >= dayIntensity)
            {
                globalLight.intensity = dayIntensity;
                StartCoroutine(PeriodDelay(dayTime, -cycleStep));
            }
            else if (globalLight.intensity <= nightIntensity)
            {
                globalLight.intensity = nightIntensity;
                StartCoroutine(PeriodDelay(nightTime, cycleStep));
            }
        }
        
        if (timer >= gameTime)
        {
            gameManager.GameOver();
        }
        else
        {
            timer += Time.deltaTime;
            timeBar.fillAmount = 1 - Map(timer, 0, gameTime, 0, 1, true);
        }
    }

    public float Map(float x, float in_min, float in_max, float out_min, float out_max, bool clamp = false)
    {
        if (clamp) x = Math.Max(in_min, Math.Min(x, in_max));
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
    
    private IEnumerator PeriodDelay(float duration, float i)
    {
        stopCycle = true;
        yield return new WaitForSeconds(duration);
        stopCycle = false;
        step = i;
    }
    
    private void ChangeIntensity(float changeRate)
    {
        globalLight.intensity += changeRate * Time.deltaTime;
    }
}
