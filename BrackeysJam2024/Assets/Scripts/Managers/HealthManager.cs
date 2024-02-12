using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [HideInInspector] public float health;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
		UIManager.Instance.PlayerUI.UpdateHealthBar(health, maxHealth);
	}

    public void changeHealth(float value) //Increase/Decrease health by the value
    {
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health < 0f)
        {
            health = 0f;
            //Call some death function
        }

    }
}
