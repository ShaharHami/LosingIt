using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Kid : Obstacle
{
    public NPCController npcController;
    public SpriteRenderer graphic;
    private bool audioStopped;
    private Tween tween, tween1;
    private void Start()
    {
        bar.fillAmount = 0;
    }

    public override void Annoyed()
    {
        npcController.StartFollowing();
        tween = npcController.target.GetComponent<SpriteRenderer>().DOColor(Color.green, 0.2f).SetLoops(-1, LoopType.Yoyo);
        tween1 = graphic.DOColor(annoyedColor, 0.1f).SetLoops(-1, LoopType.Yoyo);
        audioStopped = false;
        source.clip = annoySfx;
        source.loop = true;
        source.Play();
        base.Annoyed();
    }

    public override void ShowPrompt()
    {
        if (Vector3.Distance(npcController._transform.position, npcController.target.transform.position) < 2f)
        {
            base.ShowPrompt();
        }
    }

    public override void CalmDown()
    {
        HidePrompt();
        if (Vector3.Distance(npcController._transform.position, npcController.target.transform.position) < 2f)
        {
            ShowPrompt();
            bar.fillAmount = calmDownMeter;
            if (!audioStopped)
            {
                source.clip = null;
                source.loop = false;
                source.Stop();
                source.PlayOneShot(calmSfx);
                audioStopped = true;
            }
            base.CalmDown();
        }
    }

    public override void CoolDown()
    {
        tween.Kill();
        tween1.Kill();
        graphic.color = Color.white;
        npcController.target.GetComponent<SpriteRenderer>().color = Color.white;
        npcController.StopFollowing();
        base.CoolDown();
    }
}
