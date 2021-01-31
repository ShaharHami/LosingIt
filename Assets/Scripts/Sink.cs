using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Sink : Obstacle
{
    public SpriteRenderer graphic;
    public Sprite dirty, clean;
    private bool audioStopped;
    private Tween tween;
    public override void Annoyed()
    {
        audioStopped = false;
        graphic.sprite = dirty;
        tween = graphic.DOColor(annoyedColor, 0.1f).SetLoops(-1, LoopType.Yoyo);
        source.clip = annoySfx;
        source.loop = true;
        source.Play();
        base.Annoyed();
    }

    public override void CalmDown()
    {
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

    public override void CoolDown()
    {
        tween.Kill();
        graphic.sprite = clean;
        graphic.color = Color.white;
        base.CoolDown();
    }
}
