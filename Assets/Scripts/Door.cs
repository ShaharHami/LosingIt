using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject opendoor;
    public GameObject closeddoor;

    public bool startOpen = false;
    private bool isOpen;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        if (startOpen)
            OpenDoor();
        else
        {
            CloseDoor();
        }

    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 1f && !isOpen)
        {
            OpenDoor();
        }
    }

    public void Interact()
    {
        if(isOpen)
            CloseDoor();
        else
        {
            OpenDoor();
        }
    }
    public void CloseDoor()
    {
        opendoor.SetActive(false);
        closeddoor.SetActive(true);
        isOpen = false;
    }
    public void OpenDoor()
    {
        opendoor.SetActive(true);
        closeddoor.SetActive(false);
        isOpen = true;
        StartCoroutine(AutoClose());
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(3f);
        while (Vector2.Distance(transform.position, player.transform.position) < 3f)
        {
            yield return new WaitForSeconds(0.5f);
        }
        CloseDoor();
        
    }
}
