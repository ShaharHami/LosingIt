using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Obstacle : MonoBehaviour
{
    [HideInInspector] public float calmDownMeter = 0f;
    public float calmDownStep = 0.1f;
    public State state;
    public GameObject prompt;
    public float coolDownTime;
    public float damage;
    public Image bar;
    public AudioSource source;
    public AudioClip annoySfx, calmSfx;
    public Color annoyedColor;
    public GameObject barGO;

    protected ObstacleManager _obstacleManager;
    
    private void Awake()
    {
        _obstacleManager = FindObjectOfType<ObstacleManager>();
        bar.fillAmount = 0;
        // barGO.SetActive(false);
    }

    public enum State
    {
        Idle,
        Calming,
        CoolDown,
        Annoying
    }

    public virtual void Annoyed()
    {
        state = State.Annoying;
        // barGO.SetActive(true);
        calmDownMeter = 1f;
        bar.fillAmount = 1;
    }

    public virtual void Calm()
    {
        state = State.Idle;
        // barGO.SetActive(false);
        calmDownMeter = 0;
    }

    public virtual void CalmDown()
    {
        state = State.Calming;
        calmDownMeter -= calmDownStep * Time.deltaTime;
        bar.fillAmount = calmDownMeter;
        if (calmDownMeter <= 0)
        {
            bar.fillAmount = 0;
            HidePrompt();
            _obstacleManager.canInteract = false;
            CoolDown();
        }
    }

    public virtual void CoolDown()
    {
        StartCoroutine(CoolDownTimed());
    }
    
    private IEnumerator CoolDownTimed()
    {
        state = State.CoolDown;
        yield return new WaitForSeconds(coolDownTime);
        Calm();
    }
    
    public virtual void ShowPrompt()
    {
        prompt.SetActive(true);
    }
    public virtual void HidePrompt()
    {
        prompt.SetActive(false);
    }
}
