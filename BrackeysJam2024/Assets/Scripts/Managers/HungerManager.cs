using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerManager : MonoBehaviour
{
    [HideInInspector] public float hunger;
    public float maxHunger;
    [SerializeField] private float hungerDepletionRate;
    
    // Start is called before the first frame update
    void Start()
    {
        hunger = maxHunger;
    }

    // Update is called once per frame
    void Update()
    {
        changeHunger(-1f * hungerDepletionRate * Time.deltaTime);
        UIManager.Instance.PlayerUI.UpdateHungerBar(hunger, maxHunger);

	}

    public void changeHunger(float value) //Increase/Decrease hunger by the value
    {
        hunger += value;
        if (hunger > maxHunger)
        {
            hunger = maxHunger;
        }
        else if (hunger < 0f)
        {
            hunger = 0f;
        }
    }
}
