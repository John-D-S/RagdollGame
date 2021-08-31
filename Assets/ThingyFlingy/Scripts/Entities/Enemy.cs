using System;

using UnityEngine;

/// <summary>
/// Unused
/// </summary>
public abstract class Enemy : MonoBehaviour
{
	[SerializeField, Tooltip("The maximum Health of the enemy.")] private float maxHealth = 10;
	private float health;

	/// <summary>
	/// kills this enemy.
	/// </summary>
	private void Die()
	{
		Destroy(gameObject);
	}
	
	/// <summary>
	/// deals damage to this enemy's health.
	/// </summary>
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
		// set the health to max health.
		health = maxHealth;
	}
}