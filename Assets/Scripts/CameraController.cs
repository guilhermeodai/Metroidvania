using UnityEngine;

public class CameraController : MonoBehaviour
{

	[SerializeField] BoxCollider2D boundingBox = default;
	PlayerController playerController = default;

	float boxHeight;
	float boxWidth;

	void Awake ()
	{
		playerController = FindAnyObjectByType<PlayerController> ();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start ()
	{
		boxHeight = Camera.main.orthographicSize;
		boxWidth = boxHeight * Camera.main.aspect;
	}

	// Update is called once per frame
	void Update ()
	{
		if (playerController != null)
			transform.position = new Vector3
			(
				Mathf.Clamp (playerController.transform.position.x, boundingBox.bounds.min.x + boxWidth, boundingBox.bounds.max.x - boxWidth),
				Mathf.Clamp (playerController.transform.position.y, boundingBox.bounds.min.y + boxHeight, boundingBox.bounds.max.y - boxHeight),
				transform.position.z
			);
	}
}
