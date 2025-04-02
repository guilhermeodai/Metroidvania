using UnityEngine;

public class SelfDestructor : MonoBehaviour
{
	[SerializeField][Range (0.1f, 60f)] float selfDestructionTimer = 1f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start ()
	{
		Destroy (gameObject, selfDestructionTimer);
	}
}
