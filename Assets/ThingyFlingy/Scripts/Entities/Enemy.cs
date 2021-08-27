using System;

using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	[SerializeField] private float maxHealth = 10;
	private float health;

	private void Die()
	{
		Destroy(gameObject);
	}
	
	public void Hit(float _damage)
	{
		health -= _damage;
		if(health < 0)
		{ 
			Die();			
		}
	}

	protected abstract void OnStart(); 
	private void Start()
	{
		health = maxHealth;
	}
}