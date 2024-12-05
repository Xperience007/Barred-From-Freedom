using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource myAudio;

    public static MusicManager instance;

    [SerializeField] private List<string> destroyOnScenes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (destroyOnScenes.Contains(currentSceneName))
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
