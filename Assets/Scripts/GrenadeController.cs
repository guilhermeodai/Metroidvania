using UnityEngine;

public class GrenadeController : MonoBehaviour
{
	[SerializeField][Range (0.1f, 60f)] float detonationTimer = 1f;
	[SerializeField] GameObject detonationEffect = default;
	[SerializeField][Range (0.1f, 10f)] float blastRadius = 1f;
	[SerializeField] LayerMask destructiblesLayer = default;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		detonationTimer -= Time.deltaTime;

		if (detonationTimer <= 0)
		{
			if (detonationEffect != null)
				Instantiate (detonationEffect, transform.position, Quaternion.identity);

			// Remove the bomb from scene
			Destroy (gameObject);

			// Remove destructible objects hit by bomb from scene
			Collider2D[] blastTargets = Physics2D.OverlapCircleAll (transform.position, blastRadius, destructiblesLayer);

			if (blastTargets.Length > 0)
			{
				foreach (var target in blastTargets)
					Destroy (target.gameObject);
			}
		}
	}
}
