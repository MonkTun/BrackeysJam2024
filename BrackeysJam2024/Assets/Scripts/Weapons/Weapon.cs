using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	public float StaminaCost => _staminaCost;
	public float HungerCost => _hungerCost;

	[SerializeField] protected float _attackDamage;
	[SerializeField] protected float _attackCooltime;

	[SerializeField] protected float _staminaCost;
	[SerializeField] protected float _hungerCost;


	protected float _lastAttackTime;

	public virtual bool Attack(out float staminaCosted, out float hungerCosted)
	{
		staminaCosted = _staminaCost;
		hungerCosted = _hungerCost;

		if (_lastAttackTime + _attackCooltime < Time.time)
		{
			_lastAttackTime = Time.time;
			return true;
		}

		return false;
	}
}
