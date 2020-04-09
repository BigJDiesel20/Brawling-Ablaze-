using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [Tooltip("The amount of Hit Point the character possesses")] public Stats health;
    [Tooltip("The amount of flames the charactrer possesses")] public Stats flame;

    Rigidbody rb;
    public float speed;
    Vector3 movement;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health.Initialize();
        flame.Initialize();
          
    }

    // Update is called once per frame
    void Update()
    {
        Func<bool, float> Jump = (x) => { return  x == true? 50 : 0;};
        Vector3 movement = new Vector3 (Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        //movement = Camera.main.transform.TransformDirection(movement);
        Vector3 movementDirection = Vector3.RotateTowards(transform.forward, movement, 10 * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(movementDirection);
        
        rb.AddForce(movement * speed * Time.deltaTime, ForceMode.Acceleration);//, Space.World);        
        rb.AddForce(0, Jump(Input.GetKeyDown(KeyCode.Space)),0,ForceMode.Impulse);

        
    }

    public void TriggerEvent()
    {
        health.SubtractValue(5);        
        Debug.Log(health.Value.ToString());

        flame.SubtractValue(1);        
        Debug.Log(flame.Value.ToString());
    }

    public void Died()
    {
        
        Debug.Log("Died");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y,transform.position.z + 2));
        
    }
}
