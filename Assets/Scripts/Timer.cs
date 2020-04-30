using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timer : MonoBehaviour
{
    public float count;
    private bool _isActive;
    private bool _isReset;
    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public bool IsReset { get { return _isReset; } }


    public  Timer()
    {
        count = 0f;
        _isActive = false;
        _isReset = true;

    }

    public void Counting()
    {

        if (_isActive) { count += Time.deltaTime; _isReset = false; }

    }

    public void ResetCount(bool resetTimer)
    {
        if (resetTimer == true && count != 0) { count = 0; _isReset = true; }

    }


}
