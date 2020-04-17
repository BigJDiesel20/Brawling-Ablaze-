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
    private bool _isAttemptingToMove;
    private bool nextAttack;
    private int _attackState;
    private bool _isSpecialActive;
    


    private float previousHeight;
    private float currentHeight;
    [SerializeField]
    public Vector3 viewVelocity;

    // Properties
    public bool IsGrounded { get { return _isGrounded; } }
    public bool ApexReached { get { return _apexReached; } }
    public bool IsMoving { get { return _isMoving; } }
    public bool IsAttacking { get { return _isAttacking; } }
    public bool IsAttemptingToMove { get { return _isAttemptingToMove; } }
    public int AttackState { get { return _attackState; } set {_attackState = value; animator.SetInteger("AttackState", _attackState); } }
    public bool IsSpecialActive {get{return _isSpecialActive = flame.Value == flame.MaxValue && special.isExecuted == false ?  true : false;  }}
    public float SpecialSpeed { get { return Mathf.Clamp(transform.InverseTransformDirection(rb.velocity).z,1, 2); } }
    
    

    



    enum AttackType { None, Weak, Medium, Heavy }
    AttackType atackType;

    enum AnimationState { Idle = 0,}

    

    Rigidbody rb;
    CameraContoller cameraContoller;
    private CapsuleCollider bodyCollider;
    private Animator animator;

    public Execute special;
    

    [System.Serializable]
    public class Execute
    {
        public bool isExecuted;

        Execute()
        {
            isExecuted = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health.Initialize();
        flame.Initialize();
        _attackState = 0;        
        cameraContoller = Camera.main.GetComponent<CameraContoller>();
        bodyCollider = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();

        
        
    }

    // Update is called once per frame
    void Update()
    {

        

        DetectGroundSurface();
        DetectApex();
        // Input
                
        Func<bool, float> Jump = (x) => { return x == true ? _isGrounded ? jump : 0 : 0; };
        Vector3 inputMovementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputMovementDirection.Normalize();
        _isAttemptingToMove = inputMovementDirection != Vector3.zero;
        _isMoving = inputMovementDirection != new Vector3(0,rb.velocity.y,0) ? true : false;
        animator.SetBool("IsAttemptingToMove", _isAttemptingToMove);
        animator.SetBool("IsMoving", _isMoving);
        //float speed = speedLimit /  == 0 ? 1 : rb.velocity.sqrMagnitude;
        viewVelocity = transform.InverseTransformDirection(rb.velocity);


        animator.SetFloat("MovementMultiplyer",  transform.InverseTransformDirection(rb.velocity).z / speedLimit);

        ConvertInputDirToCameraDir(ref inputMovementDirection); // Converting Input to Camera's Direction
        
        AnimatiorStateBehaviour(inputMovementDirection,Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Jump(Input.GetKeyDown(KeyCode.Space))); // Set Code Behaviour based on Animator State

        


    }

    void ConvertInputDirToCameraDir(ref Vector3 inputMovementDirection)
    {

        if (cameraContoller != null)
        {
            inputMovementDirection = cameraContoller.CamDirection.transform.TransformDirection(inputMovementDirection);
        }
        else
        {
            cameraContoller = Camera.main.GetComponent<CameraContoller>();
            
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

   void UpdateAttackStateLitePunch(int currentAttackState)
    {
       
        //Debug.Log();
        if (AttackState <= 1)
        {
            this.AttackState = 0;            
        }        

    }
    void UpdateAttackStateMediumKick(int currentAttackState)
    {
        
        //Debug.Log();
        if (AttackState >= 2)
        {
            this.AttackState = 0;
        }

    }

    void UpdateAttackStateHeavyKick(int currentAttackState)
    {
        
        //Debug.Log();
        if (AttackState < 0)
        {
            this.AttackState = 0;
        }

    }
    
    void AnimatiorStateBehaviour(Vector3 inputMovementDirection, float horizontal, float vertical, float jump)
    {
        Action<int, bool> Attack = (nextAttack, input) => { if (input) { _isAttacking = true; _isMoving = false; animator.SetBool("IsAttacking", _isAttacking); AttackState++; AttackState = AttackState > nextAttack ? nextAttack : AttackState; } };
        Action<bool> Special = (x) => { if (x == true) { Debug.Log(IsSpecialActive); animator.SetBool("IsSpecialActive", IsSpecialActive); AttackState = 0; special.isExecuted = false; StartCoroutine(SpecialTimer()); }; };
        Action ResetTime = () => { _isAttacking = _isAttacking == true ? false : _isAttacking; animator.SetBool("IsAttacking", _isAttacking); };
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Attack(1, Input.GetKeyDown(KeyCode.F));
            IdlePlayerMovement(jump); // Movement allowed while player is idle
            Special(Input.GetKeyDown(KeyCode.G));

            
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
        {

            Attack(1, Input.GetKeyDown(KeyCode.F));
            RunningPlayerMovement(inputMovementDirection, horizontal, vertical, jump); // Movement allowed while player is running
            Special(Input.GetKeyDown(KeyCode.G));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Lite Punch"))
        {


            Attack(2, Input.GetKeyDown(KeyCode.F));
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Movement allowed while player is weak punching
            Special(Input.GetKeyDown(KeyCode.G));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Medium Kick"))
        {

            Attack(4, Input.GetKeyDown(KeyCode.F));
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Movement allowed while player is medium kicking
            Special(Input.GetKeyDown(KeyCode.G));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Heavy Kick"))
        {

            Attack(10, Input.GetKeyDown(KeyCode.F));
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Movement allowed while player is heavy kicking
            Special(Input.GetKeyDown(KeyCode.G));
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hurricane Kick"))
        {
            RunningPlayerMovement(inputMovementDirection, horizontal, vertical, jump); // Movement allowed while player is using Special
            Debug.Log("Hurricane Kick");
            animator.SetFloat("SpecialSpeed", SpecialSpeed);
        }
       
    }







    
    void RunningPlayerMovement(Vector3 inputMovementDirection, float horizontal, float vertical, float jump)
    {
        // Rotating Playing towards the Input directions.
        Vector3 movementDirection = Vector3.RotateTowards(transform.forward, inputMovementDirection, 10 * Time.deltaTime, 0.0f); // Rotates the characters forward towards the direction of the Input movement;
        if (horizontal != 0 || vertical != 0)
            transform.rotation = Quaternion.LookRotation(movementDirection);

        Vector3 movementSpeed;
        movementSpeed.x = Mathf.Abs(rb.velocity.x) > speedLimit ? (speedLimit - Mathf.Abs(rb.velocity.x)) * Mathf.Sign(rb.velocity.x) : inputMovementDirection.x * acceleration * Time.deltaTime; //rb.velocity.x < speedLimit ? movement.x * acceleration  : speedLimit - Mathf.Abs(rb.velocity.x) * Mathf.Sign(rb.velocity.x);
        movementSpeed.y = 0;
        movementSpeed.z = Mathf.Abs(rb.velocity.z) > speedLimit ? (speedLimit - Mathf.Abs(rb.velocity.z)) * Mathf.Sign(rb.velocity.z) : inputMovementDirection.z * acceleration * Time.deltaTime; // rb.velocity.z < speedLimit ? movement.x * acceleration  : speedLimit - Mathf.Abs(rb.velocity.z) * Mathf.Sign(rb.velocity.x);


        rb.AddForce(movementSpeed, ForceMode.VelocityChange);

        rb.AddForce(0, jump, 0, ForceMode.Impulse);
    }

    void IdlePlayerMovement(float jump)
    {
        rb.AddForce(0, jump, 0, ForceMode.Impulse);
    }


    public IEnumerator SpecialTimer()
    {

        for (int i = flame.Value; i > 0; i--)
        {        
            yield return new WaitForSeconds(2);
            flame.SubtractValue(1);
            Debug.Log(flame.MaxValue);
        }

        animator.SetBool("IsSpecialActive", IsSpecialActive);
        special.isExecuted = false;
    }

    

}
