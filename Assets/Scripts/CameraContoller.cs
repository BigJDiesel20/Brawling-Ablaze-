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

    // Start is called before the first frame update   

    private void Awake()
    {
        InitializeCameraDirection();
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
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
