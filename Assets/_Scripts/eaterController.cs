using UnityEngine;
using System.Collections;

public class eaterController : MonoBehaviour {

	// Eater Obstacle Controller Script //

	public playerController playerController;

	public Transform leftPieceTransform;
	public Transform rightPieceTransform;
	Collider2D eaterCollider;
	Animator eaterAnimator;

	public Rigidbody2D playerRigidbody;
	public Transform playerTransform;
	public Collider2D playerCollider;
	public SpriteRenderer playerSprite;

	float timeCounter;
	float timeParticleSpawn;
	float particleSpawn = 0.1f;
	public float eatingTime;

	Object deathParticle;
	Object deathParticlePrefab;
	Object eaterTextParticle;
	Object eaterTextParticlePrefab;
	bool particleSpawned = false;

	void Start () {
	
		eaterCollider = GetComponent<Collider2D> ();
		eaterAnimator = GetComponent<Animator> ();
		deathParticlePrefab = Resources.Load ("deathParticle") as Object;
		eaterTextParticlePrefab = Resources.Load ("eaterTextParticle") as Object;

	}
	void FixedUpdate () {
	
		eaterAnimator.SetBool ("playerIn", eaterCollider.IsTouching (playerCollider));

		if (eaterCollider.IsTouching (playerCollider)) {

			timeCounter = timeCounter + Time.deltaTime;
			timeParticleSpawn = timeParticleSpawn + Time.deltaTime;

			playerRigidbody.isKinematic = true;
			playerTransform.position = Vector3.MoveTowards (playerTransform.position, new Vector3 (transform.position.x, transform.position.y + 1, 2), .5f);

			if (timeCounter > 0.2f)
				playerController.enabled = false;
			
		} 

		if (timeParticleSpawn > particleSpawn && !particleSpawned) {

			for (int i = 0; i < 1; i++) {
				
				deathParticle = (Object)Instantiate (deathParticlePrefab, playerTransform.position, playerTransform.rotation);
				eaterTextParticle = (Object)Instantiate (eaterTextParticlePrefab, playerTransform.position, new Quaternion(-90, 0, 0, 0));
				particleSpawned = true;

			}

		} 
		if (eatingTime < timeCounter) {

			//Destroy (deathParticle);
			for (int i = 0; i < 1; i++) {
				playerController.enabled = true;
				Debug.Log ("ate");
				playerRigidbody.isKinematic = false;
				playerTransform.position = new Vector3 (18, 10, 2);
				playerTransform.rotation = new Quaternion (0, 0, 0, 0);
				timeParticleSpawn = 0;
				timeCounter = 0;
				particleSpawned = false;
				Destroy (deathParticle);
				Destroy (eaterTextParticle);
			}
		}
	}
}
