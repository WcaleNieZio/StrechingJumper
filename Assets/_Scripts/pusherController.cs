using UnityEngine;
using System.Collections;

public class pusherController : MonoBehaviour {

	// Pusher Obstacle Controller Script //

	public Transform platformTransform;
	public float pushDistance;
	public float pushSpeed;
	float minimumZero;
	public float stopTime;

	public Collider2D playerCollider;

	Collider2D catchZone;

	bool tripDone = false;
	bool playerCaught = false;
	float stopTimer;

	void Start () {
	
		catchZone = GetComponent<Collider2D> ();
		minimumZero = platformTransform.position.y;

	}
	void FixedUpdate () {

		if (catchZone.IsTouching (playerCollider))
			playerCaught = true;

		if (playerCaught && !tripDone) {

			platformTransform.position = new Vector2 (platformTransform.position.x, Mathf.Clamp (platformTransform.position.y + Time.deltaTime * pushSpeed, minimumZero, minimumZero + pushDistance * 2));

			if (platformTransform.position.y == (minimumZero + pushDistance * 2)) {

				stopTimer = stopTimer + Time.deltaTime;

				if(stopTimer > stopTime && !catchZone.IsTouching(playerCollider))
					tripDone = true;

			}
		} 

		if (tripDone) {

			platformTransform.position = new Vector2 (platformTransform.position.x, Mathf.Clamp (platformTransform.position.y - Time.deltaTime * pushSpeed, minimumZero, minimumZero + pushDistance * 2));

			if (platformTransform.position.y == minimumZero) {
				
				tripDone = false;
				playerCaught = false;
				stopTimer = 0;

			}
		}
	}
}
