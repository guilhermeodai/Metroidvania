using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[SerializeField] int startingHealth = 3;
	[SerializeField] GameObject deathEffect = default;

	public void DamageEnemy (int damage)
	{
		startingHealth -= damage;

		if (startingHealth <= 0)
		{
			if (deathEffect != null)
				Instantiate (deathEffect, transform.position, Quaternion.identity);

			Destroy (gameObject);
		}
	}
}
