using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BrightnessManager : MonoBehaviour
{
    public float maxBrightness;
    public float minBrightness;
    [HideInInspector] public float brightness; //light intensity = default intensity * brightness
    [SerializeField] private float brightnessDepletionRate;

    public Light2D torch;
    public Light2D lightNearPlayer;
    public Light2D mainLightRay;
    public Light2D lightMuzzle;
    public Light2D lightSpread;

    private float torchBaseIntensity;
    private float lightNearPlayerBaseIntensity;
    private float mainLightRayBaseIntensity;
    private float lightMuzzleBaseIntensity;
    private float lightSpreadBaseIntensity;

    // Start is called before the first frame update
    void Start()
    {
        brightness = maxBrightness;

        torchBaseIntensity = torch.intensity;
        lightNearPlayerBaseIntensity = lightNearPlayer.intensity;
        mainLightRayBaseIntensity = mainLightRay.intensity;
        lightMuzzleBaseIntensity = lightMuzzle.intensity;
        lightSpreadBaseIntensity = lightSpread.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        changeBrightness(-1f * brightnessDepletionRate * Time.deltaTime);

        torch.intensity = torchBaseIntensity * brightness;
        lightNearPlayer.intensity = lightNearPlayerBaseIntensity * brightness;
        mainLightRay.intensity = mainLightRayBaseIntensity * brightness;
        lightMuzzle.intensity = lightMuzzleBaseIntensity * brightness;
        lightSpread.intensity = lightSpreadBaseIntensity * brightness;
    }

    public void changeBrightness(float value) //Increase/Decrease brightness by the value
    {
        brightness += value;
        if (brightness > maxBrightness)
        {
            brightness = maxBrightness;
        }
        else if (brightness < minBrightness)
        {
            brightness = minBrightness;
        }
    }
}
