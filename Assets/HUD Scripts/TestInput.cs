using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{

    public FlameMeter flameMeter;
    public Dictionary<string, string> state;

    void Start()
    {
        this.state = new Dictionary<string, string>();
        state.Add("slot1", "empty");
        state.Add("slot2", "empty");
        state.Add("slot3", "empty");
        state.Add("full", "no");
        flameMeter.SetFlameMeterState(state);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            state["slot1"] = "empty";
            state["slot2"] = "empty";
            state["slot3"] = "empty";
            state["full"] = "no";
            flameMeter.SetFlameMeterState(state);
        }
        if (Input.GetKeyDown("1"))
        {
            state["slot1"] = "red";
            state["slot2"] = "empty";
            state["slot3"] = "empty";
            state["full"] = "no";
            flameMeter.SetFlameMeterState(state);
        }
        if (Input.GetKeyDown("2"))
        {
            state["slot1"] = "red";
            state["slot2"] = "red";
            state["slot3"] = "empty";
            state["full"] = "no";
            flameMeter.SetFlameMeterState(state);
        }
        if (Input.GetKeyDown("3"))
        {
            state["slot1"] = "red";
            state["slot2"] = "red";
            state["slot3"] = "red";
            state["full"] = "no";
            flameMeter.SetFlameMeterState(state);
        }
        if (Input.GetKeyDown("4"))
        {
            state["full"] = "yes";
            flameMeter.SetFlameMeterState(state);
        }
    }
}
