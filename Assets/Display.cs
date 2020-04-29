using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Display
{
    public bool CurrentDisplay;
    public GameObject playerCharacter;
    public GameObject playerModel;
    public GameObject playerModelPefab;
    
    

    public void InstanitatePrefab(Vector3 spawnPoint)
    {
        playerModelPefab = MonoBehaviour.Instantiate(playerModel, spawnPoint, Quaternion.identity);
    }

    public void RemoveModel()
    {
        MonoBehaviour.Destroy(playerModelPefab);
    }
    

    
}
