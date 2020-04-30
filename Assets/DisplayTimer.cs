using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DisplayTimer : MonoBehaviour
{

    public TextMeshProUGUI TimerText;
    public Timer Timer;
    public float MaxTime;
    public bool TimerDone;

    // Start is called before the first frame update
    void Start()
    {
        TimerDone = false;
        float count = Timer.count;
        float time = MaxTime - count;
        TimerText.text = string.Format("{0}",  Math.Floor(time));
    }

    // Update is called once per frame
    void Update()
    {
        float count = Timer.count;
        if (count > MaxTime)
        {
            TimerDone = true;
        }
        else
        {
            float time = MaxTime - count;
            TimerText.text = string.Format("{0}", Math.Floor(time));
        } 
    }
}
