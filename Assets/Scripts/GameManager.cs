using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager gameM;    

    private void Awake()
    {
        if (gameM == null)
        {
            gameM = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gameM != this)
        {
            Destroy(gameObject);
        }        
    }
}
