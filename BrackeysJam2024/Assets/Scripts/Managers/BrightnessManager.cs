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

    [SerializeField] private float flicker;
    [SerializeField] private int flickerFrequency; //flickers every N frames
    private int flickerFrequencyCnt;

    [SerializeField] private float startFlickeringBrigtness = 0.35f;

    [SerializeField] private Transform torchSpriteMaskScaler;

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

        flickerFrequencyCnt = 0;

        torchBaseIntensity = torch.intensity;
        lightNearPlayerBaseIntensity = lightNearPlayer.intensity;
        mainLightRayBaseIntensity = mainLightRay.intensity;
        lightMuzzleBaseIntensity = lightMuzzle.intensity;
        lightSpreadBaseIntensity = lightSpread.intensity;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isLampOn = !isLampOn;
		}
        
        if (isLampOn)
        {
            changeBrightness(-1f * brightnessDepletionRate * Time.deltaTime);

            float noise = 0;

            if (brightness < startFlickeringBrigtness)
            {

                flickerFrequencyCnt++;
                if (flickerFrequencyCnt == flickerFrequency)
                {
                    flickerFrequencyCnt = 0;
                    noise = Random.Range(-1f * flicker, flicker) * (brightness / startFlickeringBrigtness);
                }
                else
                {
                    noise = 0f;
                }
            }


            torch.intensity = torchBaseIntensity * brightness+noise;
            lightNearPlayer.intensity = lightNearPlayerBaseIntensity * brightness+noise;
            mainLightRay.intensity = mainLightRayBaseIntensity * brightness+noise;
            lightMuzzle.intensity = lightMuzzleBaseIntensity * brightness+noise;
            lightSpread.intensity = lightSpreadBaseIntensity * brightness+noise;


            if (brightness < startFlickeringBrigtness)
            {
				torchSpriteMaskScaler.localScale = new Vector2(brightness / startFlickeringBrigtness, brightness / startFlickeringBrigtness);
			} else
            {
				torchSpriteMaskScaler.localScale = new Vector2(1, 1);
			}
		}
        else
        {
            torch.intensity = 0f;
            lightNearPlayer.intensity = 0f;
            mainLightRay.intensity = 0f;
            lightMuzzle.intensity = 0f;
            lightSpread.intensity = 0f;

			torchSpriteMaskScaler.localScale = Vector2.zero;
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
