using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public float sanity;
    public float baseErosionRate;
    public Image sanityBar;
    public TextMeshProUGUI sanityLevel;
    public SpriteRenderer faceRenderer;
    public Sprite[] moods;
    public FMOD fmod;
    public RuntimeAnimatorController dadController, momController;
    public Animator animator;
    private int stressLevel = 0;

    private void Awake()
    {
        animator.runtimeAnimatorController = PersistantData.Instance.brando ? dadController : momController;
        SetBGM();
    }

    public void ErodeSanity(List<Obstacle> obstacles)
    {
        sanity -= ErosionAmount(obstacles) * baseErosionRate * Time.deltaTime;
        UpdateUI();
        SetBGM();
        if (sanity <= 0)
        {
            sanity = 0;
            gameManager.GameOver();
        }
    }

    private void SetBGM()
    {
        if (sanity >= 90 && stressLevel != 0)
        {
            stressLevel = 0;
            fmod.ChangeToStressLevel(stressLevel);
        }
        else if (sanity < 90 && sanity >= 75 && stressLevel != 1)
        {
            stressLevel = 1;
            fmod.ChangeToStressLevel(stressLevel);
        }
        else if (sanity < 75 && sanity >= 50 && stressLevel != 2)
        {
            stressLevel = 2;
            fmod.ChangeToStressLevel(stressLevel);
        }
        else if (sanity < 50 && stressLevel != 3)
        {
            stressLevel = 3;
            fmod.ChangeToStressLevel(stressLevel);
        }
    }

    private float ErosionAmount(List<Obstacle> obstacles)
    {
        return obstacles.Sum(obstacle => obstacle.damage);
    }
    
    public void IncreaseSanity(float rate)
    {
        if (sanity <= 100)
        {
            sanity += rate * Time.deltaTime;
            UpdateUI();
            SetBGM();
        }
    }

    private void UpdateUI()
    {
        sanityBar.fillAmount = sanity / 100;
        sanityLevel.text = $"Sanity {sanity:F1}/100";
    }

    public void changeMood(int moodIdx)
    {
        if (moodIdx < 0 || moodIdx >= moods.Length)
            faceRenderer.gameObject.SetActive(false);
        else
        {
            faceRenderer.gameObject.SetActive(true);
            faceRenderer.sprite = moods[moodIdx];
        }
    }
}
