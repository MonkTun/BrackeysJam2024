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

    [HideInInspector] public bool isLampOn;

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

        isLampOn = true;

        torchBaseIntensity = torch.intensity;
        lightNearPlayerBaseIntensity = lightNearPlayer.intensity;
        mainLightRayBaseIntensity = mainLightRay.intensity;
        lightMuzzleBaseIntensity = lightMuzzle.intensity;
        lightSpreadBaseIntensity = lightSpread.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isLampOn = !isLampOn;
        }
        
        if (isLampOn)
        {
            changeBrightness(-1f * brightnessDepletionRate * Time.deltaTime);

            torch.intensity = torchBaseIntensity * brightness;
            lightNearPlayer.intensity = lightNearPlayerBaseIntensity * brightness;
            mainLightRay.intensity = mainLightRayBaseIntensity * brightness;
            lightMuzzle.intensity = lightMuzzleBaseIntensity * brightness;
            lightSpread.intensity = lightSpreadBaseIntensity * brightness;
        }
        else
        {
            torch.intensity = 0f;
            lightNearPlayer.intensity = 0f;
            mainLightRay.intensity = 0f;
            lightMuzzle.intensity = 0f;
            lightSpread.intensity = 0f;
        }

		UIManager.Instance.PlayerUI.UpdateLightBar(brightness, maxBrightness);
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
