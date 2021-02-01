using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI timeText;
    private float timer;
    private float step;
    private bool stopCycle;

    private float gameHour, gameMinute;

    private void Start()
    {
        step = cycleStep;
        gameHour = gameTime / 48;
        gameMinute = gameHour / 60;
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
            timeText.text = 0 + ":" + 0;
        }
        else
        {
            timer += Time.deltaTime;
            var pq = (60 - (timer % gameHour) / gameMinute);
            var t = pq > 10 ? pq.ToString("F0") : 0.ToString();
            if (pq >= 50)
            {
                t = 50.ToString();
            }
            var tf = t.Substring(0, 1);
            timeText.text = (47.5f - (timer / gameHour)).ToString("F0") + ":" + tf;
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
