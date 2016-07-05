using UnityEngine;
using System.Collections;

public class firethrowerController : MonoBehaviour {

	//*** FireThrower Obstacle Controller ***//

	public playerController playerController;

	public Collider2D burningZone;

	public Collider2D playerCollider;
	public Rigidbody2D playerRigidbody;
	public Transform playerTransform;
	public SpriteRenderer playerSprite;

	ParticleSystem fireParticleSystem;

	float burningTime;
	float deathTimeout = 2;
	float deathTimer;

	bool burningNow = false;
	bool playerBurned;
	bool particleSpawned = false;

	public Vector3 playerSpawnPoint;

	Vector4 recoveryColor;

	GameObject burnedParticlePrefab;
	GameObject burnedParticle;

	void Start () {

		fireParticleSystem = GetComponentInChildren<ParticleSystem> ();
		recoveryColor = playerSprite.color;
		burnedParticle = Resources.Load ("burnedDeathParticle") as GameObject;

	}
	void Update () {
	
		if (burningNow) {
			fireParticleSystem.Play ();
			burningZone.enabled = true;
		}
		if (!burningNow) {
			fireParticleSystem.Stop ();
			burningZone.enabled = false;
		}

		burningTime = burningTime + Time.deltaTime;

		if (burningTime > 3) {

			burningNow = !burningNow;
			burningTime = 0;

		}

		if (burningZone.IsTouching (playerCollider)) {
			playerBurned = true;
		}

		if (playerBurned){

			playerSprite.color = new Vector4 (0, 0, 0, 1);
			deathTimer = deathTimer + Time.deltaTime;
			if (!particleSpawned) {
				
				for (int i = 0; i < 1; i++) {
					
					burnedParticlePrefab = (GameObject)Instantiate (burnedParticle, playerTransform.position, Quaternion.identity);
					particleSpawned = true;

				}

			}

			Destroy (burnedParticlePrefab, 2);
			//playerRigidbody.isKinematic = true;

			if (deathTimer > 0.2f)
				playerController.enabled = false;

			if (deathTimeout < deathTimer) {

				for (int i = 0; i < 1; i++) {

					playerSprite.color = recoveryColor;
					playerController.enabled = true;
					playerRigidbody.isKinematic = false;
					playerTransform.position = playerSpawnPoint;
					deathTimer = 0;
					particleSpawned = false;
					playerBurned = false;

				}
			}
		}
	}
}
