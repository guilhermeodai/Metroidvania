using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
	[SerializeField] GameObject playerStanding = default;
	[SerializeField] GameObject playerCrouching = default;
	[SerializeField] float movingSpeed = 8f;

	[Header ("Jumping")]
	[SerializeField] float jumpingForce = 20f;
	[SerializeField] Transform groundCheckpoint = default;
	[SerializeField] LayerMask groundLayer = default;

	[Header ("Shooting")]
	[SerializeField] Transform gunpoint = default;
	[SerializeField] ProjectileController projectilePrefab = default;
	[SerializeField] Transform bombpoint = default;
	[SerializeField] GrenadeController bombPrefab = default;

	[Header ("Dashing")]
	[SerializeField][Range (1f, 10f)] float speedMultiplier = 4f;
	[SerializeField][Range (0.1f, 2f)] float dashingDuration = 0.25f;
	[SerializeField][Range (0.1f, 60f)] float dashingCooldown = 0.5f;
	[SerializeField] SpriteRenderer afterimageRenderer = default;
	[SerializeField][Range (0.01f, 1f)] float afterimageDuration = 0.15f;
	[SerializeField][Range (0.01f, 1f)] float afterimageInterval = 0.05f;
	[SerializeField] Color afterimageTint = default;

	[Header ("Crouching")]
	[SerializeField][Range (0.01f, 1f)] float crouchingDuration = 0.5f;


	bool isGrounded;
	bool canDoubleJump;
	float dashingTimer;
	float afterimageTimer;
	float cooldownTimer;
	float crouchingTimer;
	Rigidbody2D rbody2D;
	Animator standingAnimator;
	Animator crouchingAnimator;
	SpriteRenderer spriteRenderer;
	Vector2 moveVector;
	Vector2 crouchVector;
	AbilityTracker abilityTracker;

	void Awake ()
	{
		rbody2D = GetComponent<Rigidbody2D> ();
		standingAnimator = playerStanding.GetComponent<Animator> ();
		crouchingAnimator = playerCrouching.GetComponent<Animator> ();
		spriteRenderer = playerStanding.GetComponent<SpriteRenderer> ();
		abilityTracker = GetComponent<AbilityTracker> ();
	}


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start ()
	{
		playerCrouching.SetActive (false);
	}

	void OnMove (InputValue value)
	{
		moveVector = new Vector2 (value.Get<Vector2> ().x, 0).normalized;
		crouchVector = new Vector2 (0, value.Get<Vector2> ().y);
	}

	void OnJump ()
	{
		if (isGrounded || (abilityTracker.isDoubleJumpUnlocked && canDoubleJump))
		{
			if (isGrounded)
			{
				canDoubleJump = true;
			}
			else
			{
				canDoubleJump = false;
				standingAnimator.SetTrigger ("doubleJump");
			}

			rbody2D.linearVelocity = new Vector2 (rbody2D.linearVelocity.x, rbody2D.linearVelocity.y + jumpingForce);
		}
	}

	void OnShoot ()
	{
		if (playerStanding.activeSelf)
		{
			if (projectilePrefab != null & gunpoint != null)
				Instantiate (projectilePrefab, gunpoint.position, gunpoint.rotation).directionVector = new Vector2 (transform.localScale.x, 0);

			standingAnimator.SetTrigger ("projectileFired");
		}
		else if (playerCrouching.activeSelf && abilityTracker.isGrenadeLaunchUnlocked)
		{
			if (bombPrefab != null)
				Instantiate (bombPrefab, bombpoint.position, Quaternion.identity);
		}
	}

	void OnDash ()
	{
		if (cooldownTimer <= 0)
		{
			if (playerStanding.activeSelf && abilityTracker.isDashUnlocked)
			{
				dashingTimer = dashingDuration;
				DisplayAfterimages ();
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		MoveAndDash ();
		Crouch ();

		// Checks if player is grounded
		isGrounded = Physics2D.OverlapCircle (groundCheckpoint.position, 0.2f, groundLayer);

		// Sets animations
		if (playerStanding.activeSelf)
		{
			standingAnimator.SetBool ("isGrounded", isGrounded);
			standingAnimator.SetFloat ("speed", Mathf.Abs (rbody2D.linearVelocity.x));
		}

		if (playerCrouching.activeSelf)
		{
			crouchingAnimator.SetFloat ("speed", Mathf.Abs (rbody2D.linearVelocity.x));
		}

	}

	void MoveAndDash ()
	{
		// Dash cooldown period
		if (cooldownTimer > 0)
		{
			cooldownTimer -= Time.deltaTime;
		}

		//
		if (dashingTimer > 0)
		{
			// Dash moving speed
			rbody2D.linearVelocity = new Vector2 (transform.localScale.x * movingSpeed * speedMultiplier, rbody2D.linearVelocity.y);

			dashingTimer -= Time.deltaTime;
			afterimageTimer -= Time.deltaTime;

			if (afterimageTimer <= 0)
				DisplayAfterimages ();

			// Sets dash cooldown period
			cooldownTimer = dashingCooldown;
		}
		else
		{
			// Normal moving speed
			rbody2D.linearVelocity = new Vector2 (moveVector.x * movingSpeed, rbody2D.linearVelocity.y);

			// Flips character to moving direction
			if (rbody2D.linearVelocity.x < 0)
				transform.localScale = new Vector3 (-1f, 1f, 1f);
			else if (rbody2D.linearVelocity.x > 0)
				transform.localScale = Vector3.one;
		}
	}

	void DisplayAfterimages ()
	{
		SpriteRenderer afterimage = Instantiate (afterimageRenderer, transform.position, transform.rotation);
		afterimage.transform.localScale = transform.localScale;
		afterimage.sprite = spriteRenderer.sprite;
		afterimage.color = afterimageTint;

		Destroy (afterimage.gameObject, afterimageDuration);

		afterimageTimer = afterimageInterval;
	}

	void Crouch ()
	{
		if (playerCrouching != null && !playerCrouching.activeSelf)
		{
			// Crouches if pressing down for longer than crouching duration
			if (abilityTracker.isCrouchingUnlocked && crouchVector.y < -0.75f)
			{
				crouchingTimer -= Time.deltaTime;
				if (crouchingTimer <= 0)
				{
					playerCrouching.SetActive (true);
					playerStanding.SetActive (false);
				}
			}
			else
			{
				crouchingTimer = crouchingDuration;
			}
		}
		else
		{
			if (crouchVector.y > 0.75f)
			{
				crouchingTimer -= Time.deltaTime;
				if (crouchingTimer <= 0)
				{
					playerCrouching.SetActive (false);
					playerStanding.SetActive (true);
				}
			}
			else
			{
				crouchingTimer = crouchingDuration;
			}
		}
	}
}