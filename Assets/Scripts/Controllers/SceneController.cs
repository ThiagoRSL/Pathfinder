using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SceneController : MonoBehaviour
{ 
    //Lazy Singleton Implementation
    private static SceneController instance;

    public static SceneController Instance { get { return instance; } }

    void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    //
    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
}
