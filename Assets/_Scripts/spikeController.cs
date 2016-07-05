using UnityEngine;
using System.Collections;

public class spikeController : MonoBehaviour {

	// Spike Behaviour Script //

	public playerController playerController;

	public Collider2D playerCollider;
	public Rigidbody2D playerRigidbody;
	public Transform playerTransform;

	float timeCounter;
	public float cooldownTime;
	Collider2D spikeCollider;
	public Vector3 playerSpawnPosition;
	bool respawn = false;

	void Start () {
	
		spikeCollider = GetComponent<Collider2D> ();

	}
	void FixedUpdate () {
	
		if (spikeCollider.IsTouching (playerCollider)) {

			playerRigidbody.isKinematic = true;
			timeCounter = timeCounter + Time.deltaTime;

			if (timeCounter > 0.2f)
				playerController.enabled = false;

			if (timeCounter > cooldownTime)
				respawn = true;

		}
		if (respawn) {

			playerController.enabled = true;
			playerRigidbody.isKinematic = false;
			playerTransform.rotation = new Quaternion (0, 0, 0, 0);
			timeCounter = 0;
			respawn = false;
			playerTransform.position = playerSpawnPosition;

		}
	}
}
