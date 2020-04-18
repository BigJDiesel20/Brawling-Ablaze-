using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [Tooltip("Current numerical value")][SerializeField] private int _value;
    [Tooltip("Minimum numerical value")] [SerializeField] private int minValue;
    [Tooltip("Maximum numerical value")] [SerializeField] private int maxValue;
    private bool isDepleted = false;
    private bool isInitialize = false;
    



    public int Value
    {
        get
        {
            return _value;
        }

        set
        {
            if (_value != value)
            {


                if (value <= minValue)
                {
                    isDepleted = true;
                    _value = minValue;
                }
                else if (value >= maxValue)
                {
                    _value = maxValue;
                }
                else
                {
                    _value = value;
                }



                //Call UpdateUI Event
            };

            
        }
    }

    public int MaxValue {get{return maxValue;}}
    public bool IsDepleted { get { return isDepleted; } }
    public bool IsInitialize { get { return isInitialize; } }


    public void Initialize()
    {
        
        if (isInitialize == false)
        {
            _value = maxValue;
            isInitialize = true;
        }           
        
    }

    public void AddValue(int additiveValue)
    {
        Value+= additiveValue;
        Debug.Log("Called");
    }

    public void SubtractValue(int subtractiveValue)
    {
        Value -= subtractiveValue;
    }

}
