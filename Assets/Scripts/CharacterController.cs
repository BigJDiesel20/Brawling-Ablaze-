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

    // 
    private bool _isGrounded;
    private bool _apexReached;
    private bool _isMoving;
    private bool _isAttacking;
    
    

    private float previousHeight;
    private float currentHeight;
    private int attack;
    public float viewVelocity;

    // Properties
    public bool IsGrounded { get { return _isGrounded; } }
    public bool ApexReached { get { return _apexReached; } }
    public bool IsMoving { get { return _isMoving; } }
    public bool IsAttacking { get { return _isAttacking; } }
    

    enum AttackType { None, Weak, Medium, Heavy }
    AttackType atackType;

    

    Rigidbody rb;
    CameraContoller cameraContoller;
    private CapsuleCollider bodyCollider;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health.Initialize();
        flame.Initialize();
        cameraContoller = Camera.main.GetComponent<CameraContoller>();
        bodyCollider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Action<int,bool> Attack = (count,input) => { if (input) _isAttacking = true; _isMoving = false; animator.SetBool("IsAttacking", _isAttacking); /*"{ attack++;} attack = attack > 3 ? 0 : attack; animator.SetInteger("Attack", attack); */};
        Attack(1, Input.GetKeyDown(KeyCode.F));

        DetectGroundSurface();
        DetectApex();
        // Input

        Func<bool, float> Jump = (x) => { return x == true ? _isGrounded ? jump : 0 : 0; };
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        movement.Normalize();
        _isMoving = movement != new Vector3(0,rb.velocity.y,0) ? true : false;
        animator.SetBool("IsMoving", _isMoving);
        //float speed = speedLimit /  == 0 ? 1 : rb.velocity.sqrMagnitude;
        viewVelocity = transform.InverseTransformDirection(rb.velocity).z/ speedLimit;


        animator.SetFloat("MovementMultiplyer",  transform.InverseTransformDirection(rb.velocity).z / speedLimit);



        // Converting Input to Camera's Direction
        if (cameraContoller != null)
        {
            movement = cameraContoller.CamDirection.transform.TransformDirection(movement);
        }
        else
        {
            cameraContoller = Camera.main.GetComponent<CameraContoller>();
        }

        if (_isMoving && !IsAttacking)
        {            
            // Rotating Playing towards the Input directions.
            Vector3 movementDirection = Vector3.RotateTowards(transform.forward, movement, 10 * Time.deltaTime, 0.0f);
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                transform.rotation = Quaternion.LookRotation(movementDirection);

            Vector3 movementSpeed;
            movementSpeed.x = Mathf.Abs(rb.velocity.x) > speedLimit ? (speedLimit - Mathf.Abs(rb.velocity.x)) * Mathf.Sign(rb.velocity.x) : movement.x * acceleration * Time.deltaTime; //rb.velocity.x < speedLimit ? movement.x * acceleration  : speedLimit - Mathf.Abs(rb.velocity.x) * Mathf.Sign(rb.velocity.x);
            movementSpeed.y = 0;
            movementSpeed.z = Mathf.Abs(rb.velocity.z) > speedLimit ? (speedLimit - Mathf.Abs(rb.velocity.z)) * Mathf.Sign(rb.velocity.z) : movement.z * acceleration * Time.deltaTime; // rb.velocity.z < speedLimit ? movement.x * acceleration  : speedLimit - Mathf.Abs(rb.velocity.z) * Mathf.Sign(rb.velocity.x);


            rb.AddForce(movementSpeed, ForceMode.VelocityChange);

            rb.AddForce(0, Jump(Input.GetKeyDown(KeyCode.Space)), 0, ForceMode.Impulse);
        }
       

        if (IsAttacking)
        {

        }

        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punching Weak"))
        {
            Debug.Log("Punching Weak");
        }

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

    void DetectGroundSurface()
    {
        bool isGrounded;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, bodyCollider.bounds.extents.y + .01f);
        if (isGrounded) _apexReached = false;
        _isGrounded = isGrounded;
        animator.SetBool("IsGrounded", _isGrounded);
    }

    void DetectApex()
    {
        previousHeight = currentHeight;
        currentHeight = transform.position.y;
        if (currentHeight < previousHeight && !_isGrounded)
        {
            _apexReached = true;
        }
    }

    void Moving()
    {

    }

    //public void AttackCheck()
    //{
    //    animator.SetBool("IsAttacking",_isAttacking);
    //}
    public void AttackReset()
    {
        
        _isAttacking = false;
        animator.SetBool("IsAttacking", _isAttacking);
    }

    //void Attack(bool input)
    //{
    //    if (input)
    //    {
    //        attack++;
    //        attack =  attack > 3 ? 0 : +1f;
    //    }
    //}

    void CheckAnimationState()
    {
        
    }


}
