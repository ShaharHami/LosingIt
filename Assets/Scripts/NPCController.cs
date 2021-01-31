using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public enum NPCState
{
    Moving,
    Working,
    Annoying,
    Idling,
    Follow
}

public class NPCController : MonoBehaviour
{
    private CharacterMover characterMover;

    public NPCState state = NPCState.Idling;
    private CharacterCustomController _characterCustomController;

    public PointsOfInterest[] pointsOfInterest;
    public int currentPointIdx = 0;
    public PointsOfInterest target;
    public Transform followee;
    public float poiDelay;

    internal Transform _transform;
    private Vector2 direction;

    public bool allowDebug = false;
    private Tween lastStretch;

    private void Awake()
    {
        _characterCustomController = GetComponent<CharacterCustomController>();
        _transform = this.transform;
        characterMover = GetComponent<CharacterMover>();
        SetState(NPCState.Idling);
        StartCoroutine(PoiDelay(poiDelay));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (allowDebug)
            {
                BecomePlayable();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (allowDebug)
            {
                StartFollowingPlayerToTarget(target);
            }
        }
    }

    public void BecomePlayable()
    {
        StopAllCoroutines();
        this.enabled = false;
        _characterCustomController.enabled = true;
        GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow = this.transform;
        lastStretch.Kill();
    }

    public void SetState(NPCState state)
    {
        this.state = state;
        switch (state)
        {
            case NPCState.Idling:
                break;
            case NPCState.Moving:
                MoveToNextPoint();
                break;
            case NPCState.Working:
                break;
            case NPCState.Annoying:
                break;
            case NPCState.Follow:
                FollowPlayer();
                break;
        }
    }

    private void OnDone()
    {
        if (state == NPCState.Follow) return;
        SetState(NPCState.Idling);
        characterMover.Move(Vector2.zero);
        StartCoroutine(PoiDelay(poiDelay));
    }

    private IEnumerator PoiDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (state == NPCState.Follow) yield break;
        SetState(NPCState.Moving);
    }

    public void MoveToNextPoint()
    {
        currentPointIdx++;
        if (currentPointIdx == pointsOfInterest.Length)
        {
            currentPointIdx = 0;
        }

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (Vector3.Distance(pointsOfInterest[currentPointIdx].transform.position, _transform.position) >= 0.1f)
        {
            direction = pointsOfInterest[currentPointIdx].transform.position - _transform.position;
            characterMover.Move(direction.normalized);
            yield return null;
        }

        lastStretch = transform.DOMove(pointsOfInterest[currentPointIdx].transform.position, 0.03f).OnComplete(OnDone);
    }

    public void StartFollowingPlayerToTarget(PointsOfInterest target)
    {
        this.target = target;
        SetState(NPCState.Follow);
    }

    public void FollowPlayer()
    {
        followee = GameObject.FindObjectOfType<CinemachineVirtualCamera>().Follow;
        StartCoroutine(FollowToTarget());
    }

    IEnumerator FollowToTarget()
    {
        while (state == NPCState.Follow)
        {
            if (Vector3.Distance(_transform.position, target.transform.position) > 2)
            {
                if (Vector3.Distance(_transform.position, followee.position) > 2)
                {
                    direction = followee.position - _transform.position;
                    characterMover.Move(direction.normalized);
                    yield return null;
                }
                else
                {
                    characterMover.Move(Vector3.zero);
                    yield return null;
                }
            }
            else
            {
                characterMover.Move(Vector3.zero);
                if (Vector3.Distance(_transform.position, target.transform.position) > 0.5f)
                {
                    transform.DOMove(target.transform.position, 0.5f);
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    state = NPCState.Follow;
                    yield return null;
                }
            }
        }
    }

    public void StartFollowing()
    {
        StartFollowingPlayerToTarget(target);
    }

    public void StopFollowing()
    {
        StopAllCoroutines();
        SetState(NPCState.Idling);
        StartCoroutine(PoiDelay(5f));
    }
}