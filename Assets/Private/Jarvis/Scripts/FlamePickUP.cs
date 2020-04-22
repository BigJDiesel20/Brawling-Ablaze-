using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePickUP : MonoBehaviour
{
    [SerializeField] private int value;
    [System.Serializable]
    public class IntenistyRange
    {
        public float min;
        public float max;
    }
    public IntenistyRange intenisty;
    private Light lightComponent;
    private Coroutine Flicker;
    public CharacterController player;
    private float range;
    public bool hitcheck;

    private void Start()
    {
        lightComponent = GetComponent<Light>();
        Flicker = StartCoroutine(LightFlicker());
    }

    private void Update()
    {
        
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<CharacterController>();
            player.flame.AddValue(1);
            Destroy(gameObject);
        }


    }

    IEnumerator LightFlicker()
    {
        do
        {
            range = Random.Range(intenisty.min,intenisty.max);
            lightComponent.intensity = range;
            yield return new WaitForSeconds(Random.Range(0,1));
        }
        while (true);
    }

    private void OnDestroy()
    {        
        StopCoroutine(Flicker);
    }
}
