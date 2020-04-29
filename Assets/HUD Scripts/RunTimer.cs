using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTimer : MonoBehaviour
{

    public Timer Timer;

    void Start()
    {
        Timer.IsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            Debug.Log("RESET TIMER!!!");
            Timer.ResetCount(true);
        }
        else
        {
            Timer.Counting();
        }
    }
}
