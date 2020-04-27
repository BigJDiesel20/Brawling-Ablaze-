using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // player's CharacterController
    public CharacterController player;
    // slider instance
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    void Start()
    {
        SetMaxHealth(player.health.MaxValue);
    }

    void Update()
    {
        SetHealth(player.health.Value);
    }
}
