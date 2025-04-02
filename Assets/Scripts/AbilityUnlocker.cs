using UnityEngine;
using TMPro;

public class AbilityUnlocker : MonoBehaviour
{
	public bool unlockCrouching, unlockDash, unlockDoubleJump, unlockGrenadeLaunch;

	[Header ("Player Feedback")]
	[SerializeField] GameObject pickupEffect = default;
	[SerializeField] TMP_Text unlockTextContainer = default;
	[SerializeField] string unlockText = default;

	AbilityTracker abilityTracker;


	void Awake ()
	{
		unlockTextContainer.gameObject.SetActive (false);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player")
		{
			abilityTracker = other.GetComponentInParent<AbilityTracker> ();

			if (unlockCrouching)
				abilityTracker.isCrouchingUnlocked = true;

			if (unlockDash)
				abilityTracker.isDashUnlocked = true;

			if (unlockDoubleJump)
				abilityTracker.isDoubleJumpUnlocked = true;

			if (unlockGrenadeLaunch)
				abilityTracker.isGrenadeLaunchUnlocked = true;

			// Triggers pick-up effect	
			if (pickupEffect != null)
				Instantiate (pickupEffect, transform.position, Quaternion.identity);

			// Display unlock message
			unlockTextContainer.transform.parent.SetParent (null);
			unlockTextContainer.transform.position = transform.position;
			unlockTextContainer.gameObject.SetActive (true);
			unlockTextContainer.text = unlockText;
			Destroy (unlockTextContainer.transform.parent.gameObject, 3f);

			Destroy (gameObject);
		}
	}
}
