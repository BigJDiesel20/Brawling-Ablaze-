using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    CharacterController characterController;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {        
        characterController = GetComponentInParent<CharacterController>();        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Hitbox"))
        {
            AttackDefinition attackDefinition;
            //attackDefinition = other.GetComponent<HitBox>().attackDefinition != null? other.GetComponent<HitBox>().attackDefinition: null;
            if (characterController != null && other.GetComponent<HitBox>().attackDefinition != null)
            {
                attackDefinition = other.GetComponent<HitBox>().attackDefinition;
                characterController.health.SubtractValue(attackDefinition.Damage);
                if (characterController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || characterController.Animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
                {
                    characterController.Animator.SetTrigger(attackDefinition.AttackType);
                }
                Debug.Log(attackDefinition.AttackType + " " + attackDefinition.Damage + " damage");
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
