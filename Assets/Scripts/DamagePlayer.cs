using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
	[SerializeField] int damage = 1;

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Player")
			DealDamage (other.gameObject);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player")
			DealDamage (other.gameObject);
	}

	void DealDamage (GameObject player)
	{
		PlayerHealth.instance.DamagePlayer (damage);
	}
}
