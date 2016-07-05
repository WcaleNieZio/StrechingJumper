using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	//*** Camera controll script ***//

	public Transform playerTransform;

	void Update () {

		transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);

	}
}
