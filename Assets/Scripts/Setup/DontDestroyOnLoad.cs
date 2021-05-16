using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static GameController instance = null;

    private void Awake()
    {
        if (instance != null)
        {
            GameController.instance = instance;
            DontDestroyOnLoad(this);
        }else
        {
            Destroy(this);
        }
            

        
    }
}