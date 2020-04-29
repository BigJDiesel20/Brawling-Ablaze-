using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int playerID;
    CustomInput playerInput;
    public CharacterController opponentController;
    public AudioSource audioSource;
    public AudioClip audioClip;

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
    public bool _inAttackRange;
    private bool _isBlocking;



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
    public int AttackState { get { return _attackState; } set { _attackState = value; _animator.SetInteger("AttackState", _attackState); } }
    public bool IsSpecialActive { get { return _isSpecialActive = flame.Value == flame.MaxValue && special.isExecuted == false ? true : false; } }
    public float SpecialSpeed { get { return Mathf.Clamp(transform.InverseTransformDirection(rb.velocity).z, 1, 2); } }
    public bool InAttackRange { get { return _inAttackRange; } set { _inAttackRange = value; } }
    public bool IsBlocking { get { return _isBlocking; } set { _isBlocking = value; } }


    // Hurt & Hitboxes
    public Collider hurtbox;
    public Collider liteAttackHitbox;
    public Collider mediumAttackHitbox;
    public Collider heavyAttackHitbox;
    public Collider specialAttackHitbox;
    public Collider playerCollisionBox;
    public Collider opponentCollisionBox;
    public Collider[] enviornments;
    public Collider[] playerHitboxes;
    public Collider[] opponentHitboxes;
    public float horizontalMovement;
    public float verticalMovement;

    public bool isopponentHitboxesSet;



    enum AttackType { None, Weak, Medium, Heavy }
    AttackType atackType;

    enum AnimationState { Idle = 0, }



    Rigidbody rb;
    CameraContoller cameraContoller;
    private Collider bodyCollider;
    private Animator _animator;

    public Execute special;

    public Animator Animator { get { return _animator; } }


    [System.Serializable]
    public class Execute
    {
        public bool isExecuted;

        Execute()
        {
            isExecuted = false;
        }
    }

    public delegate void OnComplete();
    OnComplete onInitializedComplete;
    bool isInitialized = false;
    bool isSetup = false;

    [System.Serializable]
    public class Timer
    {
        public float count;
        private bool _isActive;
        private bool _isReset;
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
        public bool IsReset { get { return _isReset; }}


        public Timer()
        {
            count = 0f;
            _isActive = false;
            _isReset = true;

        }

        public void Counting()
        {
            
            if (_isActive) { count += Time.deltaTime; _isReset = false; }

        }

        public void ResetCount(bool resetTimer)
        {       
             if (resetTimer == true && count != 0) { count = 0; _isReset = true; }
                
        }

        
    }

    Timer delayActionTimer;
    public float viewTimer;
    public bool viewActive;
    public bool viewReset;
    
    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        IsBlocking = false;
        delayActionTimer = new Timer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInitialized == false) { Initialized(); return; }
        if (isSetup == false) { Setup(); return; }
        playerInput.ChangeController(playerID, this);
        _isBlocking = playerInput.GetKey(1);
        _animator.SetBool("Block", _isBlocking);
        delayActionTimer.Counting();
        viewTimer = delayActionTimer.count;
        viewActive = delayActionTimer.IsActive;
        viewReset = delayActionTimer.IsReset;




        //if (playerInput == null) playerInput = new CustomInput(playerID);
        //opponentController = GameManager.Instance.playerTwo;


        DetectGroundSurface();
        DetectApex();
        // Input
                
        Func<bool, float> Jump = (x) => { return x == true ? _isGrounded ? jump : 0 : 0; };
        Vector3 inputMovementDirection = new Vector3(playerInput.GetAxisRaw("Horizontal"), 0, playerInput.GetAxisRaw("Vertical"));
        inputMovementDirection.Normalize();
        _isAttemptingToMove = inputMovementDirection != Vector3.zero;
        _isMoving = inputMovementDirection != new Vector3(0,rb.velocity.y,0) ? true : false;
        _animator.SetBool("IsAttemptingToMove", _isAttemptingToMove);
        _animator.SetBool("IsMoving", _isMoving);
        horizontalMovement =transform.InverseTransformDirection(inputMovementDirection).x;
        verticalMovement = transform.InverseTransformDirection(inputMovementDirection).z;
        //_animator.SetFloat("HorizontalMovement", horizontalMovement);
        //_animator.SetFloat("VerticalMovement", verticalMovement);
        //float speed = speedLimit /  == 0 ? 1 : rb.velocity.sqrMagnitude;
        viewVelocity = transform.InverseTransformDirection(rb.velocity);
        Debug.Log(playerInput.GetAxisRaw("Horizontal"));
        
        
        
        _animator.SetFloat("MovementMultiplyer", inputMovementDirection.sqrMagnitude);//Mathf.Clamp(transform.InverseTransformDirection(rb.velocity).z / speedLimit,.1f, transform.InverseTransformDirection(rb.velocity).z));

        ConvertInputDirToCameraDir(ref inputMovementDirection); // Converting Input to Camera's Direction
        HitBoxActivation();
        AnimatiorStateBehaviour(inputMovementDirection,playerInput.GetAxisRaw("Horizontal"), playerInput.GetAxisRaw("Vertical"), Jump(playerInput.GetKeyDown(0))); // Set Code Behaviour based on Animator State

        


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

        isGrounded = Physics.Raycast(transform.position, Vector3.down, /*bodyCollider.bounds.extents.y +*/ .1f);
        if (isGrounded) _apexReached = false;
        _isGrounded = isGrounded;
        _animator.SetBool("IsGrounded", _isGrounded);
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
            Debug.Log("LightPunch Reset");
            this.AttackState = 0;            
        }        

    }
    void UpdateAttackStateMediumKick(int currentAttackState)
    {
        
        //Debug.Log();
        if (AttackState >= 2)
        {
            Debug.Log("MediumKick Reset");
            this.AttackState = 0;
        }

    }

    void UpdateAttackStateHeavyKick(int currentAttackState)
    {
        
        //Debug.Log();
        if (AttackState >= 0)
        {
            Debug.Log("HeavyKick Reset");
            this.AttackState = 0;
            _animator.SetInteger("AttackState", _attackState);
        }

    }


    public
    void AnimatiorStateBehaviour(Vector3 inputMovementDirection, float horizontal, float vertical, float jump)
    {
        Action<int, bool, Collider> Attack = (nextAttack, input, hitBoxCollider) => { if (input) { _isAttacking = true; _isMoving = false; _animator.SetBool("IsAttacking", _isAttacking); /*if (hitBoxCollider != null) hitBoxCollider.enabled = true;AttackState++;*/ AttackState = AttackState > nextAttack ? nextAttack : AttackState += 1; RotateTowardsOpponent(opponentController.gameObject.transform.position, _inAttackRange); } };
        Action<bool> Special = (x) => { if (x == true && flame.Value == flame.MaxValue) { Debug.Log(IsSpecialActive); _animator.SetBool("IsSpecialActive", IsSpecialActive); AttackState = 0; special.isExecuted = false; StartCoroutine(SpecialTimer()); }; };
        Action ResetTime = () => { _isAttacking = _isAttacking == true ? false : _isAttacking; _animator.SetBool("IsAttacking", _isAttacking); };

        delayActionTimer.IsActive = _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Running") ? true : false;
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (delayActionTimer.count > .5f)
            {
                if (_isGrounded)
                {
                    Attack(1, playerInput.GetKeyDown(2), null);
                }
            }
            

            RotateTowardsMovementDirection(inputMovementDirection, horizontal, vertical);
            IdlePlayerMovement(jump); // Movement allowed while player is idle
            Special(playerInput.GetKeyDown(3));
            //if (_attackState < 2) {_attackState = 0; _animator.SetInteger("AttackState", _attackState);}
            //_animator.SetInteger("AttackState", _attackState);


        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
        {
            if (delayActionTimer.count > 3)
            {
                if (_isGrounded)
                {
                    Attack(1, playerInput.GetKeyDown(2), liteAttackHitbox);
                }
            }


            RotateTowardsMovementDirection(inputMovementDirection, horizontal, vertical);
            rb.AddForce(0, jump, 0, ForceMode.Impulse);
            //RunningPlayerMovement(inputMovementDirection, horizontal, vertical, jump); // Movement allowed while player is running
            Special(playerInput.GetKeyDown(3));
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Lite Punch"))
        {
            //delayActionTimer.ResetCount(true);
            Attack(2, playerInput.GetKeyDown(2), mediumAttackHitbox);

            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Movement allowed while player is weak punching
            Special(playerInput.GetKeyDown(3));
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Medium Kick"))
        {
            delayActionTimer.ResetCount(true);
            Attack(4, playerInput.GetKeyDown(2), heavyAttackHitbox);
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Movement allowed while player is medium kicking
            Special(playerInput.GetKeyDown(3));
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Heavy Kick"))
        {
            AttackState = 0;
            _animator.SetInteger("AttackState", AttackState);
            delayActionTimer.ResetCount(true);
            Attack(0, playerInput.GetKeyDown(2), specialAttackHitbox);
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // Movement allowed while player is heavy kicking
            Special(playerInput.GetKeyDown(3));
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hurricane Kick"))
        {
            delayActionTimer.ResetCount(true);
            RotateTowardsMovementDirection(inputMovementDirection, horizontal, vertical);
            //RunningPlayerMovement(inputMovementDirection, horizontal, vertical, jump); // Movement allowed while player is using Special
            Debug.Log("Hurricane Kick");
            _animator.SetFloat("SpecialSpeed", SpecialSpeed);

        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Defeated")) ;
        {

        }

       
    }







    
    void RunningPlayerMovement(Vector3 inputMovementDirection, float horizontal, float vertical, float jump)
    {
        // Rotating Playing towards the Input directions.
        Vector3 movementDirection = Vector3.RotateTowards(transform.forward, inputMovementDirection, 50 * Time.deltaTime, 0.0f); // Rotates the characters forward towards the direction of the Input movement;
        if (horizontal != 0 || vertical != 0)
            transform.rotation = Quaternion.LookRotation(movementDirection);

        Vector3 movementSpeed;
        movementSpeed.x = Mathf.Abs(rb.velocity.x) > speedLimit ? (speedLimit - Mathf.Abs(rb.velocity.x)) * Mathf.Sign(rb.velocity.x) : inputMovementDirection.x * acceleration * Time.deltaTime; //rb.velocity.x < speedLimit ? movement.x * acceleration  : speedLimit - Mathf.Abs(rb.velocity.x) * Mathf.Sign(rb.velocity.x);
        movementSpeed.y = 0;
        movementSpeed.z = Mathf.Abs(rb.velocity.z) > speedLimit ? (speedLimit - Mathf.Abs(rb.velocity.z)) * Mathf.Sign(rb.velocity.z) : inputMovementDirection.z * acceleration * Time.deltaTime; // rb.velocity.z < speedLimit ? movement.x * acceleration  : speedLimit - Mathf.Abs(rb.velocity.z) * Mathf.Sign(rb.velocity.x);


        rb.AddForce(movementSpeed, ForceMode.VelocityChange);

        rb.AddForce(0, jump, 0, ForceMode.Impulse);
    }

    void RotateTowardsMovementDirection(Vector3 inputMovementDirection, float horizontal, float vertical)
    {
        Vector3 movementDirection = Vector3.RotateTowards(transform.forward, inputMovementDirection, 360 * Time.deltaTime,0.0f); // Rotates the characters forward towards the direction of the Input movement;
        if (horizontal != 0 || vertical != 0)
            transform.rotation = Quaternion.LookRotation(movementDirection);
    }
    public void RotateTowardsOpponent(Vector3 opponentLocation, bool inAttackRange)
    {
        //Debug.Log(opponentLocation);
        //Debug.Log(inAttackRange);
        Vector3 opponentDirection = opponentLocation - transform.position;
        opponentDirection.y = 0;
        Vector3 movementDirection = Vector3.RotateTowards(transform.forward, opponentDirection.normalized, 360 * Time.deltaTime, 0.0f); // Rotates the characters forward towards the direction of the Input movement;
        if (inAttackRange)
            transform.rotation = Quaternion.LookRotation(movementDirection);
    }


    void IdlePlayerMovement(float jump)
    {
        rb.AddForce(0, jump, 0, ForceMode.Impulse);
    }


    public IEnumerator SpecialTimer()
    {
        
        for (int i = flame.Value; i > 0; i--)
        {        
            yield return new WaitForSeconds(1);
            if (flame.Value == 0) break;
            flame.SubtractValue(1);
            Debug.Log(flame.MaxValue);
            
        }

        _animator.SetBool("IsSpecialActive", IsSpecialActive);
        special.isExecuted = false;
    }


    void HitBoxActivation()
    {
        liteAttackHitbox.enabled = _animator.GetCurrentAnimatorStateInfo(0).IsName("Lite Punch")? true:false;
        mediumAttackHitbox.enabled =  _animator.GetCurrentAnimatorStateInfo(0).IsName("Medium Kick")? true: false;

        heavyAttackHitbox.enabled = _animator.GetCurrentAnimatorStateInfo(0).IsName("Heavy Kick") ? true : false;
        //Animator.applyRootMotion = _animator.GetCurrentAnimatorStateInfo(0).IsName("Heavy Kick") ? false : true;
        specialAttackHitbox.enabled = _animator.GetCurrentAnimatorStateInfo(0).IsName("Hurricane Kick") ? true : false;
        
    }

    void MediumAttackEnabled()
    {
        mediumAttackHitbox.enabled = true;
    }



    private void Initialized()
    {
        if (isInitialized == false)
        playerInput = new CustomInput(playerID);
        rb = GetComponent<Rigidbody>();
        cameraContoller = Camera.main.GetComponent<CameraContoller>();
        bodyCollider = GetComponent<CapsuleCollider>();
        _animator = GetComponentInChildren<Animator>();
        playerCollisionBox = GetComponent<Collider>();
        playerHitboxes = GetComponentsInChildren<Collider>();
        isopponentHitboxesSet = false;
        opponentHitboxes = null;
        isInitialized = true;       
        
    }
    void Setup()
    {

        foreach (Collider playerHitbox in playerHitboxes)
        {
            if (playerHitbox.GetComponent<HitBox>() != null)
            {
                if (playerHitbox.GetComponent<HitBox>().attackDefinition.AttackType.Contains("LiteAttack")) liteAttackHitbox = playerHitbox.GetComponent<Collider>();
                else if (playerHitbox.GetComponent<HitBox>().attackDefinition.AttackType.Contains("MediumAttack")) mediumAttackHitbox = playerHitbox.GetComponent<Collider>();
                else if (playerHitbox.GetComponent<HitBox>().attackDefinition.AttackType.Contains("HeavyAttack")) heavyAttackHitbox = playerHitbox.GetComponent<Collider>();
                else if (playerHitbox.GetComponent<HitBox>().attackDefinition.AttackType.Contains("SpecialAttack")) specialAttackHitbox = playerHitbox.GetComponent<Collider>();
                else Debug.Log("Hitbox Does not have Attack Deffinition Assigned");
            }
        }




        health.Initialize();
        flame.Initialize();
        _attackState = 0;





        foreach (Collider playerHitbox in playerHitboxes)
        {
            if (playerHitbox.gameObject.CompareTag("Hitbox"))
            {
                Physics.IgnoreCollision(playerHitbox, playerCollisionBox);  // Prevent Player Hitboxes from Colliding with playerCollisionBox
            }
        }

        foreach (Collider environment in enviornments)
        {
            foreach (Collider playerHitbox in playerHitboxes)
            {
                if (playerHitbox.gameObject.CompareTag("Hitbox"))
                {
                    Physics.IgnoreCollision(environment, playerHitbox); // Prevent playerHitboxes form Colliding with the Environment;
                }
            }
        }
        isSetup = true;        
    }

    public void AttackAudio()
    {
        audioSource.clip = audioClip;
        audioSource.volume = 0.5f;
        audioSource.Play();
    }

   
    private void OnDisable()
    {
        isInitialized = false;
        isSetup = false;
    }

    
}
