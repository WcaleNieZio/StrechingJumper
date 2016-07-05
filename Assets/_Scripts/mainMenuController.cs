using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class mainMenuController : MonoBehaviour {



	public void ButtonPressed (string levelName) {

		SceneManager.LoadScene(levelName);

	}
}
