using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterCustomController : MonoBehaviour
{
 // ========= ACTIONS =================
    // public KeyCode actionKey1 = KeyCode.U;
    // public KeyCode actionKey2 = KeyCode.I;
    // public KeyCode actionKey3 = KeyCode.O;
    // public KeyCode actionKey4 = KeyCode.P;

    // public Action action1; 
    // public Action action2; 
    // public Action action3; 
    // public Action action4; 
    
    // ======== AUDIO ==========
    public AudioSource sfxSource;
    public AudioSource footStepsSource;
    public AudioClip losingItSFX;
    public AudioClip relaxingSFX;
    public AudioClip losingItCompletelySFX;
    public List<AudioClip> footSteps;
    private bool playFootSteps;
    
    // ======== HEALTH ==========
    public float MaxSanity = 5;
    public float Sanity { get; private set; }
    
  
    
    // ================= SOUNDS =======================

    private NPCController npcController;
    private CharacterMover characterMover;

    private void Awake()
    {
        npcController = GetComponent<NPCController>();
        characterMover = GetComponent<CharacterMover>();
    }

    private void Start()
    {
        StartCoroutine(PlayFootSteps());
    }

    private IEnumerator PlayFootSteps()
    {
        while (true)
        {
            if (playFootSteps)
            {
                footStepsSource.PlayOneShot(footSteps[Random.Range(0, footSteps.Count)]);
            }
            yield return new WaitForSeconds(0.4f);
        }
    }
    
    void Update()
    {
        // ============== MOVEMENT ======================
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
                
        Vector2 move = new Vector2(horizontal, vertical);
        playFootSteps = move != Vector2.zero;
        characterMover.Move(move);



        // ============== ACTIONS ======================

        // if (Input.GetKeyDown(actionKey1))
        //     DoAction1();
        // else if (Input.GetKeyDown(actionKey2))
        //     DoAction2();
        // else if (Input.GetKeyDown(actionKey3))
        //     DoAction3();
        // else if (Input.GetKeyDown(actionKey4))
        //     DoAction4();

        /*
        // ======== DIALOGUE ==========
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, 1 << LayerMask.NameToLayer("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }  
            }
        }
        */
        
        // if (Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     if (npcController.allowDebug)
        //     {
        //         BecomeUnplayable();
        //         npcController.SetState(NPCState.Moving);
        //     }
        // }

    }


    
    // public void BecomeUnplayable()
    // {
    //     this.enabled = false;
    //     npcController.enabled = true;
    // }
    


    // protected virtual void DoAction1()
    // {
    //     action1?.Invoke();
    // }
    //
    // protected virtual void DoAction2()
    // {
    //     action2?.Invoke();
    // }
    //
    // protected virtual void DoAction3()
    // {
    //     action3?.Invoke();
    // }
    //
    // protected virtual void DoAction4()
    // {
    //     action4?.Invoke();
    // }
}
