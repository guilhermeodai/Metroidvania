using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{
	[SerializeField] Transform[] waypoints = default;
	[SerializeField] float movingSpeed = 5f;
	[SerializeField] float patrollingDuration = 2f;

	[Header ("Jumping")]
	[SerializeField] float jumpingForce = 12f;
	[SerializeField] Transform groundCheckpoint = default;
	[SerializeField] LayerMask groundLayer = default;

	Rigidbody2D rbody2D;
	Animator animator;
	int currentWaypoint;
	float patrollingTimer;
	bool isGrounded;


	void Awake ()
	{
		rbody2D = GetComponent<Rigidbody2D> ();
		animator = GetComponentInChildren<Animator> ();

		foreach (Transform waypoint in waypoints)
		{
			waypoint.parent.SetParent (null);
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start ()
	{
		patrollingTimer = patrollingDuration;
	}

	// Update is called once per frame
	void Update ()
	{
		// Checks if enemy is grounded
		isGrounded = Physics2D.OverlapCircle (groundCheckpoint.position, 0.2f, groundLayer);

		if (Mathf.Abs (transform.position.x - waypoints[currentWaypoint].position.x) > .1f)
		{
			if (transform.position.x < waypoints[currentWaypoint].position.x)
			{
				rbody2D.linearVelocity = new Vector2 (movingSpeed, rbody2D.linearVelocity.y);
				transform.localScale = new Vector3 (-1f, 1f, 1f);
			}
			else
			{
				rbody2D.linearVelocity = new Vector2 (-movingSpeed, rbody2D.linearVelocity.y);
				transform.localScale = Vector3.one;
			}

			if (transform.position.y < waypoints[currentWaypoint].position.y - .5f && rbody2D.linearVelocity.y < .1f && isGrounded)
			{
				rbody2D.linearVelocity = new Vector2 (rbody2D.linearVelocity.x, rbody2D.linearVelocity.y + jumpingForce);
			}
		}
		else
		{
			rbody2D.linearVelocity = new Vector2 (0f, rbody2D.linearVelocity.y);
			patrollingTimer -= Time.deltaTime;
			if (patrollingTimer <= 0)
			{
				patrollingTimer = patrollingDuration;
				currentWaypoint++;
				if (currentWaypoint >= waypoints.Length)
					currentWaypoint = 0;
			}
		}

		animator.SetFloat ("speed", Mathf.Abs (rbody2D.linearVelocity.x));
	}
}
