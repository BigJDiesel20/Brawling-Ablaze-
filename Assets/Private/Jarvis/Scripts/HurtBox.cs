﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HurtBox : MonoBehaviour
{
    CharacterController characterController;
    Animator animator;
    AudioSource audioSource;
    public AudioClip DeathScreamAudio;
    
    // Start is called before the first frame update
    void Start()
    {        
        characterController = GetComponentInParent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Hitbox") && other.transform.root.GetComponent<CharacterController>().playerID != characterController.playerID)
        {
            AttackDefinition attackDefinition;
            //attackDefinition = other.GetComponent<HitBox>().attackDefinition != null? other.GetComponent<HitBox>().attackDefinition: null;
            if (characterController != null && other.GetComponent<HitBox>().attackDefinition != null)
            {
                attackDefinition = other.GetComponent<HitBox>().attackDefinition;
                characterController.health.SubtractValue(attackDefinition.Damage);
                audioSource.clip = attackDefinition.AttackSound;
                audioSource.volume = 0.005f;
                audioSource.Play();
                if (characterController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || characterController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    characterController.Animator.SetTrigger(attackDefinition.AttackType);
                    if (characterController.health.IsDepleted)
                    {
                        characterController.Animator.SetTrigger("Defeated");
                        audioSource.clip = DeathScreamAudio;
                        audioSource.volume = 0.005f;
                        audioSource.Play();
                        this.GetComponent<Collider>().enabled = false;
                        
                    }
                }
                Debug.Log(attackDefinition.AttackType + " " + attackDefinition.Damage + " damage");
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
