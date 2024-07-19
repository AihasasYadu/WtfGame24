using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance => instance;
    public virtual void Awake ()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();
        }
        else if (instance != FindObjectOfType<T>())
        {
            Destroy (FindObjectOfType<T>());
        }

        DontDestroyOnLoad(gameObject);
    }
}
