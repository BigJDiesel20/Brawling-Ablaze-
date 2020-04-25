using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    [SerializeField] private GameObject camDirectionPrefab;
    private GameObject camDirection;
    private bool _isInitialized;
    public GameObject CamDirection {get {return camDirection;}}
    public bool IsInitiaized {get {return _isInitialized;}}
    public Vector3 player1pos;
    public Vector3 player2pos;
    public Vector3 middlePos;
    public Collider player1Render;
    public Collider player2Render;
    public bool p1;
    public bool p2;
    public RaycastHit hit;
    public Ray ray;
    public float startValue;
    public Vector3 startPos;
    public float look;
    float number;
    float number2;
    float InitialDistancefromMidpoint;
    float minDistance, maxDistance;

    // Start is called before the first frame update   

    private void Awake()
    {
        InitializeCameraDirection();
        startValue = transform.position.z;
        startPos = transform.position;
        
    }
    void Start()
    {
        
        
    }
    // Update is called once per frame
    void Update()
    {
        


        look = Mathf.Clamp(look + 1 * Time.deltaTime, -10, 10);
        //Debug.Log(look);
        ray = new Ray(transform.position, middlePos.normalized);
        if (Physics.Raycast(ray, out hit, (middlePos - transform.position).sqrMagnitude))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<MeshRenderer>().material.name.Contains("WallTransparent"))
            {
                hit.transform.GetComponent<MeshRenderer>().material.color = new Color(hit.transform.GetComponent<MeshRenderer>().material.color.r, hit.transform.GetComponent<MeshRenderer>().material.color.g, hit.transform.GetComponent<MeshRenderer>().material.color.b, Mathf.Lerp(1, 0, 1));
            }
        }

        if (GameManager.Instance.playerOne != null && GameManager.Instance.playerTwo != null)
        {
            Debug.Log((GameManager.Instance.playerOne != null).ToString() + " " + (GameManager.Instance.playerTwo != null).ToString());
            player1Render = GameManager.Instance.playerOne.transform.root.gameObject.GetComponent<Collider>();
            player2Render = GameManager.Instance.playerTwo.transform.root.gameObject.GetComponent<Collider>();
            InitialDistancefromMidpoint = (middlePos - startPos).magnitude;
        }
        if (player1Render != null && player2Render != null)
        {
            
            if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), player1Render.bounds) == false || GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), player2Render.bounds) == false)
            {
                number = transform.position.z + Time.deltaTime;
                number2 = (middlePos - startPos).magnitude * .5f;
                transform.position = new Vector3(transform.position.x, transform.position.y,Mathf.Clamp(number, middlePos.z + InitialDistancefromMidpoint, middlePos.z + InitialDistancefromMidpoint+ 20));
                //Debug.Log(number);
            }
            else
            {
                number = transform.position.z - Time.deltaTime;
                number2 = (middlePos - startPos).sqrMagnitude;
                
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(number, middlePos.z + InitialDistancefromMidpoint, middlePos.z + InitialDistancefromMidpoint+20));
                //Debug.Log(Mathf.Clamp(number, 40,50));
            } 

            p1 = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), player1Render.bounds);
            p2 = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), player2Render.bounds);

           
        }

        if (GameManager.Instance.playerOne != null && GameManager.Instance.playerTwo != null)
        {
            player1pos = GameManager.Instance.playerOne.transform.position;
            player2pos = GameManager.Instance.playerTwo.transform.position;
            middlePos = GameManager.Instance.playerTwo.transform.position +((GameManager.Instance.playerOne.transform.position - GameManager.Instance.playerTwo.transform.position)* .5f);
            transform.LookAt(middlePos);
            
        }
    }

    //private void OnDrawGizmos()
    //{

    //    if (GameManager.Instance.playerOne != null && GameManager.Instance.playerTwo != null)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawSphere(player1pos, .5f);
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawSphere(player2pos, .5f);
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawSphere(middlePos, .5f);

    //    }
    //}

    private void LateUpdate()
    {
        UpdateCameraDirection();
    }

    void InitializeCameraDirection()
    {
        if (camDirectionPrefab == null)
        {            
            camDirection = new GameObject("CameraDirection");
            camDirection.transform.position = Camera.main.transform.position;
            camDirection.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
            _isInitialized = true;
        }
        else
        {
            camDirection = Instantiate(camDirectionPrefab);
            camDirection.transform.position = Camera.main.transform.position;
            camDirection.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
            _isInitialized = true;
        }
    }

    void UpdateCameraDirection()
    {
        if (_isInitialized)
        {
            camDirection.transform.position = Camera.main.transform.position;
            camDirection.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    

    
}
