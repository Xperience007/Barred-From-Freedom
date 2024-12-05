using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float amplitude = 0.09f;
    public float frequency = 0.5f;
    public float timeDelay = 0.0f;

    Vector3 origin = new Vector3();
    Vector3 temp = new Vector3();


    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        temp = origin;
        temp.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency * timeDelay) * amplitude;
        transform.position = temp;
    }
}
