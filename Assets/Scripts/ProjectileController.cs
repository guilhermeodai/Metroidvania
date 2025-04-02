using Unity.Mathematics;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

	[SerializeField] float speed = default;
	public Vector2 directionVector = default;
	[SerializeField] GameObject impactEffect = default;

	Rigidbody2D projectileRigidbody;
	Rigidbody2D playerRigidbody;

	void Awake ()
	{
		projectileRigidbody = GetComponent<Rigidbody2D> ();
		playerRigidbody = FindAnyObjectByType<PlayerController> ().GetComponent<Rigidbody2D> ();
	}


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		projectileRigidbody.linearVelocity = directionVector * (speed + Mathf.Abs (playerRigidbody.linearVelocity.x));
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (impactEffect != null)
			Instantiate (impactEffect, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

	void OnBecameInvisible ()
	{
		Destroy (gameObject);
	}
}
