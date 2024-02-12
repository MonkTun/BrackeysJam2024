using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
	[SerializeField] private Slider staminaBar;
	[SerializeField] private Slider hungerBar;
	[SerializeField] private Slider lightBar;

    public void UpdateHealthBar(float health, float maxHealth)
    {
		healthBar.value = health / maxHealth;
	}

    public void UpdateStaminaBar(float stamina, float maxStamina)
    {
		staminaBar.value = stamina / maxStamina;
	}

    public void UpdateHungerBar(float hunger, float maxHunger)
    {
        hungerBar.value = hunger / maxHunger;
    }

    public void UpdateLightBar(float light, float maxLight)
    {
        lightBar.value = light / maxLight;
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
