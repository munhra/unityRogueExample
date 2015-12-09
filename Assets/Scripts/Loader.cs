using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public GameObject gameManager;
	public bool presentation;


	void Awake(){
		Debug.Log("Create game manager");
		if (GameManager.instance == null) {
			GameManager.presentationMode = presentation;
			Instantiate(gameManager);
		}
	}


}
