using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrabTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalTime;

    // Start is called before the first frame update
    void Start()
    {
        int minutes = Mathf.FloorToInt(Timer.elapsedTime / 60);
        int seconds = Mathf.FloorToInt(Timer.elapsedTime % 60);
        totalTime.text = string.Format("Total Time: {0:00}:{1:00}", minutes, seconds); ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
