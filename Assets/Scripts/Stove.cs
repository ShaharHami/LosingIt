using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Stove : Obstacle
{
    public SpriteRenderer graphic1;
    public SpriteRenderer graphic2;
    private bool audioStopped;
    private Tween tween, tween1;
    public override void Annoyed()
    {
        audioStopped = false;
        tween = graphic1.DOColor(annoyedColor, 0.1f).SetLoops(-1, LoopType.Yoyo);
        tween1 = graphic2.DOColor(annoyedColor, 0.1f).SetLoops(-1, LoopType.Yoyo);
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
        tween1.Kill();
        graphic1.color = Color.white;
        graphic2.color = Color.white;
        base.CoolDown();
    }
}
