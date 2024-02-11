using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariableHelper : MonoBehaviour
{
    //This object stores widely used variables in an easily accessible manner for every class so that variables like layermasks stay consistent
    public static GlobalVariableHelper instance;
    public LayerMask solidLayerMask;
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
