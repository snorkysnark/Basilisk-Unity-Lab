using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource)), SelectionBase]
public class Button : MonoBehaviour //, IInteractible
{
    const string ANIMATION_TRIGGER = "Push";
    
    [SerializeField] Powerable target = null;

    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        animator.SetTrigger(ANIMATION_TRIGGER);
        audioSource.Play();
        if(target != null) target.Power();
    }
}
