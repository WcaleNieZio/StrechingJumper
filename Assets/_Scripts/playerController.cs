using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	//*** Player Controller Script ***//

	public float playerThrowForce;

	Transform playerTransform;
	Rigidbody2D playerRigidbody2D;
	public AudioSource throwSound;
	public Camera playerCamera;

	public Transform playerCameraTransform;

	public Transform trackerTransform;
	public Transform trackerTransformSprite;
	public Transform trackerTransformEnd;

	public float clickMaxDistance;
	float actualMouseDistance;
	float mouseMaxDistance;
	public float strechMaxDistance;
	bool clickedInMaxDistance;

	public SpriteRenderer strechSprite;
	public SpriteRenderer idleSprite;
	public SpriteRenderer idleSprite2;
	public SpriteRenderer midairSprite;
	public Transform faceTransform;

	public LayerMask whatIsCollidingWithPlayer;
	public LayerMask whatIsKillingPlayer;

	public Vector3 playerSpawnPosition;



	void Start () {
	
		playerTransform = GetComponent<Transform> ();
		playerRigidbody2D = GetComponent<Rigidbody2D> ();
		clickedInMaxDistance = false;

	}

	void FixedUpdate () {

		//Debug.Log(Mathf.Clamp(Vector2.Distance(playerCamera.ScreenToWorldPoint(Input.mousePosition), transform.position), 1, 5));
		//Debug.Log(playerCamera.ScreenToWorldPoint(Input.mousePosition).x);

		// Some variables for tracker transform //

		actualMouseDistance = Vector2.Distance (playerCamera.ScreenToWorldPoint (Input.mousePosition), transform.position);

		//***					    ***//
		//*** Tracker rotation here ***//
		//***			 		    ***//

		Vector2 _mousePosition = playerCamera.ScreenToWorldPoint (Input.mousePosition);
		Vector2 _direction = (_mousePosition - (Vector2)trackerTransform.position).normalized;

		float _angle = Mathf.Atan2 (_direction.y, _direction.x) * Mathf.Rad2Deg;
		trackerTransform.rotation = Quaternion.AngleAxis (_angle, Vector3.forward);
		faceTransform.rotation = playerTransform.rotation;

		//***					   ***//
		//*** Tracker scaling here ***//
		//***			 		   ***//

		if (Input.GetMouseButton (0) && actualMouseDistance < clickMaxDistance && GetComponent<CircleCollider2D>().IsTouchingLayers(whatIsCollidingWithPlayer)) {

			// Clicked on maximum range //

			clickedInMaxDistance = true;
			FaceSwitcher (1, strechSprite, idleSprite, idleSprite2, midairSprite);

		} else if (Input.GetMouseButton(0) && clickedInMaxDistance && GetComponent<CircleCollider2D>().IsTouchingLayers(whatIsCollidingWithPlayer)){

			// Resizing tracker //

			trackerTransformSprite.localScale = new Vector2 (Mathf.Clamp(actualMouseDistance, 1, 5) * 2, 2);
			trackerTransformSprite.localPosition = new Vector2 (Mathf.Clamp (actualMouseDistance, 1, 5) * 2.5f, 0);
			trackerTransformEnd.localPosition = new Vector3 (Mathf.Clamp (actualMouseDistance, 1, 5) * 5.1f, .09f, -.1f);
			playerCamera.orthographicSize = 8 + Mathf.Clamp(actualMouseDistance, 0, 5);

		} else if (Input.GetMouseButtonUp(0) && actualMouseDistance > clickMaxDistance && clickedInMaxDistance && GetComponent<CircleCollider2D>().IsTouchingLayers(whatIsCollidingWithPlayer)) {

			// Throwing player //

			for (int i = 0; i < 1; i++) {
				
				PlayerThrow (playerThrowForce, playerRigidbody2D, playerTransform, playerCamera);
				throwSound.pitch = 1 + Random.value;
				throwSound.Play ();
				clickedInMaxDistance = false;

			}

		} else if(!GetComponent<CircleCollider2D>().IsTouchingLayers(whatIsCollidingWithPlayer)) {

			FaceSwitcher (4, strechSprite, idleSprite, idleSprite2, midairSprite);
			trackerTransformSprite.localScale = new Vector2 (2,2);
			trackerTransformSprite.localPosition = new Vector2 (0, 0);
			trackerTransformEnd.localPosition = new Vector3 (0, .09f, -.1f);
			playerCamera.orthographicSize = Mathf.Clamp (playerCamera.orthographicSize - Time.deltaTime * 10, 8, 13);
			clickedInMaxDistance = false;

		} else {

			// Reseting everything //

			trackerTransformSprite.localScale = new Vector2 (2,2);
			trackerTransformSprite.localPosition = new Vector2 (0, 0);
			trackerTransformEnd.localPosition = new Vector3 (0, .09f, -.1f);
			clickedInMaxDistance = false;
			FaceSwitcher (2, strechSprite, idleSprite, idleSprite2, midairSprite);
			playerCamera.orthographicSize = Mathf.Clamp (playerCamera.orthographicSize - Time.deltaTime * 10, 8, 13);

		}


		// Death face switch //

		if(GetComponent<BoxCollider2D>().IsTouchingLayers(whatIsKillingPlayer))
			FaceSwitcher (1, strechSprite, idleSprite, idleSprite2, midairSprite);

		// Velocity Limiting //

		playerRigidbody2D.velocity = new Vector2 (Mathf.Clamp (playerRigidbody2D.velocity.x, -25, 25), Mathf.Clamp (playerRigidbody2D.velocity.y, -25, 25));

		//***					   ***//
		//*** Just some dev stuff  ***//
		//***			 		   ***//

		// Respawn bind 'SpawnPoint' //

		if (Input.GetKeyDown (KeyCode.R)) {
			playerTransform.position = playerSpawnPosition;
			playerRigidbody2D.isKinematic = false;
			playerRigidbody2D.velocity = new Vector2 (0, 0);
		}
	}




	void PlayerThrow (float throwForce, Rigidbody2D playerRigid, Transform transform, Camera playerCamera) {

		// Throw Force applied here: //

		playerRigid.AddForce(new Vector3((trackerTransformEnd.position.x - transform.position.x) * throwForce, (trackerTransformEnd.position.y - transform.position.y) * throwForce, 0), ForceMode2D.Impulse);

		// Debug.Log //

		//Debug.Log(Mathf.Clamp(Vector2.Distance(playerCamera.ScreenToWorldPoint(Input.mousePosition), transform.position), 1, 5));
	}

	void FaceSwitcher (int indexNumber, SpriteRenderer sprite1, SpriteRenderer sprite2, SpriteRenderer sprite3, SpriteRenderer sprite4) {

		// Face Switcher //
		//				 //
		// 1 Streching   //
		// 2 Idle1		 //
		// 3 Idle2		 //
		// 4 MidAir      //

		if (indexNumber == 1) {

			sprite1.color = new Vector4 (sprite1.color.r, sprite1.color.g, sprite1.color.b, 1);
			sprite2.color = new Vector4 (sprite2.color.r, sprite2.color.g, sprite2.color.b, 0);
			sprite3.color = new Vector4 (sprite3.color.r, sprite3.color.g, sprite3.color.b, 0);
			sprite4.color = new Vector4 (sprite4.color.r, sprite4.color.g, sprite4.color.b, 0);

		} else if (indexNumber == 2) {

			sprite1.color = new Vector4 (sprite1.color.r, sprite1.color.g, sprite1.color.b, 0);
			sprite2.color = new Vector4 (sprite2.color.r, sprite2.color.g, sprite2.color.b, 1);
			sprite3.color = new Vector4 (sprite3.color.r, sprite3.color.g, sprite3.color.b, 0);
			sprite4.color = new Vector4 (sprite4.color.r, sprite4.color.g, sprite4.color.b, 0);

		} else if (indexNumber == 3) {

			sprite1.color = new Vector4 (sprite1.color.r, sprite1.color.g, sprite1.color.b, 0);
			sprite2.color = new Vector4 (sprite2.color.r, sprite2.color.g, sprite2.color.b, 0);
			sprite3.color = new Vector4 (sprite3.color.r, sprite3.color.g, sprite3.color.b, 1);
			sprite4.color = new Vector4 (sprite4.color.r, sprite4.color.g, sprite4.color.b, 0);

		} else if (indexNumber == 4) {

			sprite1.color = new Vector4 (sprite1.color.r, sprite1.color.g, sprite1.color.b, 0);
			sprite2.color = new Vector4 (sprite2.color.r, sprite2.color.g, sprite2.color.b, 0);
			sprite3.color = new Vector4 (sprite3.color.r, sprite3.color.g, sprite3.color.b, 0);
			sprite4.color = new Vector4 (sprite4.color.r, sprite4.color.g, sprite4.color.b, 1);

		}
	}
}
