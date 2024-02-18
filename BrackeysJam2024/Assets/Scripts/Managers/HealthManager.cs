using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private AudioClip hurtAudioClip;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem onDamagePS;
    [SerializeField] private ParticleSystem onDeathPS;
    [SerializeField] private bool isPlayer;
    
    public float health;
    public float maxHealth;
    [SerializeField] private float showPPHealth;
    //Player only, didnt feel like making a subscript
    public bool isPoisoned;
    private float _timeOfLastPoisonTick=0;
    private float _delayBetweenPoisonTick=3;
    private int _poisonTickDamage=2;

    private bool isDead;

    // Start is called before the first frame update
    void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        else spriteRenderer = GetComponentInChildren<SpriteRenderer>();


        isDead = false;
		health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
        {
            if (health <= showPPHealth)
            {
                PostprocessingManager.Instance.NearDeathPPOn();
            }
            else
            {
                PostprocessingManager.Instance.NearDeathPPOff();
            }
            UIManager.Instance.PlayerUI.UpdateHealthBar(health, maxHealth);
            if (isPoisoned&&Time.time-_timeOfLastPoisonTick>_delayBetweenPoisonTick) { ChangeHealth(-_poisonTickDamage);_timeOfLastPoisonTick = Time.time; }
        }
	}

    public void ChangeHealth(float value) //Increase/Decrease health by the value
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
            Death();
			//Call some death function
		}

        StartCoroutine(DamageTakeVisual());
    }

    public void Death()
    {
        if (isPlayer == false) Destroy(gameObject);

        if (isPlayer && !isDead)
        {
            PostprocessingManager.Instance.NearDeathPPOff();
            PostprocessingManager.Instance.PausedPPOff();
            PostprocessingManager.Instance.GameplayPPOff();
            UIManager.Instance.ManageGameViews(UIManager.ViewState.Death);
            UIManager.Instance.Death();
        }

        if (onDeathPS != null) Destroy(Instantiate(onDeathPS, transform.position, Quaternion.identity), 1);
        isDead = true;
    }

    private IEnumerator DamageTakeVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.black;
			if (onDamagePS != null) onDamagePS.Play();
			yield return new WaitForSeconds(0.1f);
			spriteRenderer.color = Color.white;
		}
    }
}
