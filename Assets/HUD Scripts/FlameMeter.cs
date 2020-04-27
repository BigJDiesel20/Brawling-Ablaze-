using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlameMeter : MonoBehaviour
{
    // player's CharacterController
    public CharacterController player;

    // the flame image positions
    public Image flameSlot1;
    public Image flameSlot2;
    public Image flameSlot3;
    public Image fullFlame;

    // the possible flame images
    public Sprite blackFlame;
    public Sprite blueFlame;
    public Sprite greenFlame;
    public Sprite orangeFlame;
    public Sprite redFlame;

    // dict for flame states
    public Dictionary<string, Sprite> flameDict;

    void Start()
    {
        this.flameDict = new Dictionary<string, Sprite>();
        flameDict.Add("black", blackFlame);
        flameDict.Add("blue", blueFlame);
        flameDict.Add("green", greenFlame);
        flameDict.Add("orange", orangeFlame);
        flameDict.Add("red", redFlame);
        flameSlot1.sprite = flameDict["red"];
        flameSlot2.sprite = flameDict["red"];
        flameSlot3.sprite = flameDict["red"];
    }

    // function to set flame meter state
    public void SetFlameMeterState(Dictionary<string, string> state)
    {
        if (!state.ContainsKey("slot1") | !state.ContainsKey("slot2") | !state.ContainsKey("slot3") | !state.ContainsKey("full"))
        {
            throw new System.ArgumentException("State missing one or more required parameters.");
        }
        // Slot 1
        if (state["slot1"] == "empty")
        {
            flameSlot1.enabled = false;
        }
        else
        {
            flameSlot1.enabled = true;
            flameSlot1.sprite = flameDict[state["slot1"]];
        }
        // Slot 2
        if (state["slot2"] == "empty")
        {
            flameSlot2.enabled = false;
        }
        else
        {
            flameSlot2.enabled = true;
            flameSlot2.sprite = flameDict[state["slot2"]];
        }
        // Slot 3
        if (state["slot3"] == "empty")
        {
            flameSlot3.enabled = false;
        }
        else
        {
            flameSlot3.enabled = true;
            flameSlot3.sprite = flameDict[state["slot3"]];
        }
        // Full Flame
        if (state["full"] == "yes")
        {
            flameSlot1.enabled = false;
            flameSlot2.enabled = false;
            flameSlot3.enabled = false;
            fullFlame.enabled = true;
        }
        else
        {
            fullFlame.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        var flame = player.flame;

        switch (flame.Value)
        {
            case 0:
                flameSlot1.enabled = false;
                flameSlot2.enabled = false;
                flameSlot3.enabled = false;
                fullFlame.enabled = false;
                break;
            case 1:
                flameSlot1.enabled = true;
                flameSlot2.enabled = false;
                flameSlot3.enabled = false;
                fullFlame.enabled = false;
                break;
            case 2:
                flameSlot1.enabled = true;
                flameSlot2.enabled = true;
                flameSlot3.enabled = false;
                fullFlame.enabled = false;
                break;
            case 3:
                flameSlot1.enabled = false;
                flameSlot2.enabled = false;
                flameSlot3.enabled = false;
                fullFlame.enabled = true;
                break;
            default:
                Debug.Log("Flame Value out of bounds!");
                break;
        }
    }


}
