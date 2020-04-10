using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("The amount of Hit Point the character possesses")] public Stats health;
    [Tooltip("The amount of flames the charactrer possesses")] public Stats flame;
    [Tooltip("Rate at which the chracter's speed increases")] [SerializeField] private float acceleration;
    [Tooltip("Max speed the character can travel")] [SerializeField] private float speedLimit;
    [Tooltip("jump impluse value")] [SerializeField] private float jump;

    
    enum AttackType { None,Weak, Medium, Heavy }
    AttackType atackType;

   

    
    Rigidbody rb;
    CameraContoller cameraContoller;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health.Initialize();
        flame.Initialize();
        cameraContoller = Camera.main.GetComponent<CameraContoller>();
        
    }

    // Update is called once per frame
    void Update()
    {


        // Input
        Func<bool, float> Jump = (x) => { return x == true ? jump : 0; };
        Vector3 movement = new Vector3 (Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));
        movement.Normalize();

       
        
        
        // Converting Input to Camera's Direction
        movement = cameraContoller.CamDirection.transform.TransformDirection(movement);

        // Rotating Playing towards the Input directions.
        Vector3 movementDirection = Vector3.RotateTowards(transform.forward, movement, 10 * Time.deltaTime, 0.0f);
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            transform.rotation = Quaternion.LookRotation(movementDirection);

        Vector3 movementSpeed;
        movementSpeed.x = Mathf.Abs(rb.velocity.x) > speedLimit ? (speedLimit - Mathf.Abs(rb.velocity.x)) * Mathf.Sign(rb.velocity.x) : movement.x * acceleration * Time.deltaTime; //rb.velocity.x < speedLimit ? movement.x * acceleration  : speedLimit - Mathf.Abs(rb.velocity.x) * Mathf.Sign(rb.velocity.x);
        movementSpeed.y = 0;
        movementSpeed.z = Mathf.Abs(rb.velocity.z) > speedLimit ? (speedLimit - Mathf.Abs(rb.velocity.z)) * Mathf.Sign(rb.velocity.z) : movement.z * acceleration * Time.deltaTime; // rb.velocity.z < speedLimit ? movement.x * acceleration  : speedLimit - Mathf.Abs(rb.velocity.z) * Mathf.Sign(rb.velocity.x);
        
        
        rb.AddForce(movementSpeed , ForceMode.VelocityChange);
        
        rb.AddForce(0, Jump(Input.GetKeyDown(KeyCode.Space)),0,ForceMode.Impulse);
        
        

        
    }

    //Temp Code
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
