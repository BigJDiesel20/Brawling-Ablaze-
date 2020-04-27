using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    private static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)this;
        }
        
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }
        

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
