﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[System.Serializable]
public class AttackDefinition
{
    [SerializeField] private int damage;
    [SerializeField] private string _attackType;
    [SerializeField] private AudioClip _attackSound;

    public int Damage { get { return damage; } }
    public string AttackType
    {
        get
        {


            if (_attackType == "LiteAttack" || _attackType == "MediumAttack" || _attackType == "HeavyAttack" || _attackType == "SpecialAttack")
            {
                return _attackType;
            }
            else
            {
                string name = _attackType;
                _attackType = string.Empty;
                Debug.LogError("The AttackDefinition name " + name + " is an invalid attack Type");
                return _attackType;
            }
        }
    }

    public AudioClip AttackSound { get { return _attackSound; } }
}

    
    

