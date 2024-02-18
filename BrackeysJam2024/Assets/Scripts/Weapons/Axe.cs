using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
	[SerializeField] private Animator _animator;
	[SerializeField] private Collider2D _attackCollider;

	public override bool Attack(out float staminaCosted, out float hungerCosted)
	{
		if (base.Attack(out float _staminaCosted, out float _hungerCosted))
		{
			staminaCosted = _staminaCosted;
			hungerCosted = _hungerCosted;

			_animator.SetTrigger("Attack");
			return true;
		}

		staminaCosted = 0f;
		hungerCosted = 0f;

		return false;
	}


	private void SwingStart()
	{
		_attackCollider.enabled = (true);
		//TODO audio
	}

	private void SwingStop()
	{
		_attackCollider.enabled = (false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out HealthManager health) && collision.gameObject.layer != LayerMask.NameToLayer("Player"))
		{
			health.ChangeHealth(-_attackDamage);
		}
	}
}
