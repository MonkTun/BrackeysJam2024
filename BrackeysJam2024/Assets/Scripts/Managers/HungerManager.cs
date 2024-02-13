using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerManager : MonoBehaviour
{
    [HideInInspector] public float hunger;
    public float maxHunger;
    [SerializeField] private float baseHungerDepletionRate;
    [SerializeField] private float sprintHungerDepletionRate;

    private PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        hunger = maxHunger;

        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        changeHunger(-1f * (playerMovement.isSprinting? sprintHungerDepletionRate : baseHungerDepletionRate) * Time.deltaTime);
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
