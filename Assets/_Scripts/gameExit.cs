using UnityEngine;
using System.Collections;

public class gameExit : MonoBehaviour {

	float exitTime = 2;
	float timer;

	void Update () {

		timer = timer + Time.deltaTime;

		if(exitTime < timer)
			Application.Quit ();

	}
}
