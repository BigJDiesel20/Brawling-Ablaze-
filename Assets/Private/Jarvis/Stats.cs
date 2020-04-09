using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats 
{

    private int _value;

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
                _value = value;
                //Call UpdateUI Event
            };

            
        }
    }
    
}
