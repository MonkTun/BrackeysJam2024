using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffects : MonoBehaviour
{
    private HealthManager healthManager;
    private StaminaManager staminaManager;
    private HungerManager hungerManager;
    private BrightnessManager brightnessManager;

    [Header("Paper")]
    [SerializeField] private float paperFuelValue;
    
    [Header("Bread")]
    [SerializeField] private float breadValue;

    [Header("Raw Meat")]
    [SerializeField] private float rawMeatValue;
    [SerializeField] private float rawMeatDmg; //TOTAL damage dealt by consuming raw meat
    [SerializeField] private float rawMeatDmgDuration;
    [SerializeField] private float rawMeatDmgInterval;
    [SerializeField] private float rawMeatSlowDownRate; //new speed=original speed * slowDownRate, I'm leaving slowdown out for now

    [Header("Cooked Meat")]
    [SerializeField] private float cookedMeatValue;
    [SerializeField] private float cookedMeatHeal; //TOTAL amount healed by consuming raw meat
    [SerializeField] private float cookedMeatHealDuration;
    [SerializeField] private float cookedMeatHealInterval;

    [Header("Alcohol")]
    [SerializeField] private float alcoholFuelValue;
    [SerializeField] private float alcoholStaminaBoostValue; //raise stamina recovery speed by this amount
    [SerializeField] private float alcoholStaminaBoostDuration;

    [Header("Bandage")]
    [SerializeField] private float bandageFuelValue;
    [SerializeField] private float bandageHealValue;
    [SerializeField] private float bandageUseDuration;

    [Header("Wood")]
    [SerializeField] private float woodFuelValue;

    [Header("Candlestick")]
    [SerializeField] private float candlestickRepairValue;

    [Header("Plate")]
    [SerializeField] private float plateRepairValue;

    [Header("Barricade")]
    [SerializeField] private float barricadeFuelValue;

    [Header("Axe")]
    [SerializeField] private float dmg;
    [SerializeField] private int durability; //number of times it can hit enemies before breaking


    // Start is called before the first frame update
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        staminaManager = GetComponent<StaminaManager>();
        hungerManager = GetComponent<HungerManager>();
        brightnessManager = GetComponent<BrightnessManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BurnPaper()
    {
        brightnessManager.changeBrightness(paperFuelValue);
    }

    public void EatBread()
    {
        hungerManager.changeHunger(breadValue);
    }

    public void EatRawMeat()
    {
        hungerManager.changeHunger(rawMeatValue);
        StartCoroutine(RawMeatDOT());
    }

    private IEnumerator RawMeatDOT()
    {
        float time = 0f;
        while (time <= rawMeatDmgDuration)
        {
            yield return new WaitForSeconds(rawMeatDmgInterval);
            time += rawMeatDmgInterval;
            healthManager.changeHealth(-1f * rawMeatDmg / (rawMeatDmgDuration / rawMeatDmgInterval));
        }
    }

    public void EatCookedMeat()
    {
        hungerManager.changeHunger(cookedMeatValue);
        StartCoroutine(CookedMeatHeal());
    }

    private IEnumerator CookedMeatHeal()
    {
        float time = 0f;
        while (time <= cookedMeatHealDuration)
        {
            yield return new WaitForSeconds(cookedMeatHealInterval);
            time += cookedMeatHealInterval;
            healthManager.changeHealth(cookedMeatHeal / (cookedMeatHealDuration / cookedMeatHealInterval));
        }
    }

    public void BurnAlcohol()
    {
        brightnessManager.changeBrightness(alcoholFuelValue);
    }

    public void DrinkAlcohol()
    {
        StartCoroutine(DrinkAlcoholCoroutine());
    }

    private IEnumerator DrinkAlcoholCoroutine()
    {
        staminaManager.staminaRegenBoost = alcoholStaminaBoostValue;
        yield return new WaitForSeconds(alcoholStaminaBoostDuration);
        staminaManager.staminaRegenBoost = 0f;
    }

    public void BurnBandage()
    {
        brightnessManager.changeBrightness(bandageFuelValue);
    }

    public void UseBandage()
    {
        StartCoroutine(UseBandageCoroutine());
    }

    private IEnumerator UseBandageCoroutine() //Needs UI Indication & restrict movement
    {
        GameManager.Instance.canPlayerControl = false;
        yield return new WaitForSeconds(bandageUseDuration);
        healthManager.changeHealth(bandageHealValue);
        GameManager.Instance.canPlayerControl = true;
    }

    public void BurnWood()
    {
        brightnessManager.changeBrightness(woodFuelValue);
    }

    public void RepairCandlestick()
    {
        //TODO implement after weapon durability
    }

    public void RepairPlate()
    {
        //TODO implement after weapon durability
    }

    public void BurnBarricade()
    {
        brightnessManager.changeBrightness(barricadeFuelValue);
    }
}
