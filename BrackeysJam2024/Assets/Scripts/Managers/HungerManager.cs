using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerManager : MonoBehaviour
{
    [HideInInspector] public float hunger;
    public float maxHunger;
    [SerializeField] private float baseHungerDepletionRate;
    [SerializeField] private float sprintHungerDepletionRate;
    [SerializeField] private float showPPHunger;

    private PlayerMovement playerMovement;
    private bool isDead;
    
    // Start is called before the first frame update
    void Start()
    {
        hunger = maxHunger;

        playerMovement = GetComponent<PlayerMovement>();

        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        changeHunger(-1f * (playerMovement.isSprinting? sprintHungerDepletionRate : baseHungerDepletionRate) * Time.deltaTime);
        UIManager.Instance.PlayerUI.UpdateHungerBar(hunger, maxHunger);
        if (hunger <= showPPHunger)
        {
            PostprocessingManager.Instance.NearDeathPPOn();
        }
        else
        {
            PostprocessingManager.Instance.NearDeathPPOff();
        }
        if (hunger <= 0f && !isDead)
        {
            PostprocessingManager.Instance.NearDeathPPOff();
            PostprocessingManager.Instance.PausedPPOff();
            PostprocessingManager.Instance.GameplayPPOff();
            UIManager.Instance.ManageGameViews(UIManager.ViewState.Death);
            UIManager.Instance.Death();
            isDead = true;
        }
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
