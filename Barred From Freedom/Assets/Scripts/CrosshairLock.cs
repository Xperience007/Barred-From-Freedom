using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairLock : MonoBehaviour
{
    public GameObject crosshair;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
