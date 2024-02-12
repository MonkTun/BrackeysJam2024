using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;

    private HealthManager playerHealthManager;
    private StaminaManager playerStaminaManager;

    // Start is called before the first frame update
    void Start()
    {
        playerHealthManager = GetComponent<HealthManager>();
        playerStaminaManager = GetComponent<StaminaManager>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerHealthManager.health/playerHealthManager.maxHealth;
        staminaBar.value = playerStaminaManager.stamina/playerStaminaManager.maxStamina;
    }

    /*
    public void changeHealth(float value)//Increase/Decrease health by the value
    {
        playerHealthManager.changeHealth(value);
        healthBar.value = playerHealthManager.health;
    }

    public void changeStamina(float value)//Increase/Decrease stamina by the value
    {

    }
    */
}
