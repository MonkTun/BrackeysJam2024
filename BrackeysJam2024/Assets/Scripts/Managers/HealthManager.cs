using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private AudioClip hurtAudioClip;
    public float health;
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
        if (value < 0&&hurtAudioClip!=null) { MMSoundManagerSoundPlayEvent.Trigger(hurtAudioClip, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position); }
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
