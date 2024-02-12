using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    public float stamina;
    public float maxStamina;

    [SerializeField] private float staminaRegenRate;

    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        changeStamina(staminaRegenRate * Time.deltaTime);
    }

    public void changeStamina(float value) //Increase/Decrease stamina by the value
    {
        stamina += value;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        else if (stamina < 0f)
        {
            stamina = 0f;
        }

    }
}
