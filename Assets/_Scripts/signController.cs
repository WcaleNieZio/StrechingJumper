using UnityEngine;
using System.Collections;

public class signController : MonoBehaviour {

	public Collider2D playerCollider;
	Collider2D signCollider;
	TextMesh signText;


	void Start () {
	
		signCollider = GetComponent<CircleCollider2D> ();
		signText = GetComponentInChildren<TextMesh> ();
		signText.color = new Vector4 (1, 1, 1, 0);

	}
	void Update () {
	
		if (signCollider.IsTouching (playerCollider))
			signText.color = new Vector4 (1, 1, 1, Mathf.Clamp (signText.color.a + Time.deltaTime, 0, 1));
		else
			signText.color = new Vector4 (1, 1, 1, Mathf.Clamp (signText.color.a - Time.deltaTime, 0, 1));

	}
}
