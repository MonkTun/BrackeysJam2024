using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariableHelper : MonoBehaviour
{
    //This object stores widely used variables in an easily accessible manner for every class so that variables like layermasks stay consistent
    public static GlobalVariableHelper instance;
    public LayerMask solidLayerMask;
    public LayerMask lootLayerMask;

	public const int MaxHotbarItems = 5;
	public const int MaxItems = 20;

    public static float CalculateDistSqr(Vector2 a, Vector2 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
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
