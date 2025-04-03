using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	[SerializeField] int maxHealth = default;
	[SerializeField] GameObject deathEffect = default;

	public int currentHealth { get; set; }
	public static PlayerHealth instance;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		currentHealth = maxHealth;
	}

	public void DamagePlayer (int damage)
	{
		currentHealth -= damage;

		if (currentHealth <= 0)
		{
			currentHealth = 0;

			if (deathEffect != null)
				Instantiate (deathEffect, transform.position, Quaternion.identity);

			Destroy (gameObject);
		}
	}
}
