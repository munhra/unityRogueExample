using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {


	public GameObject[] floorTiles;
	public GameObject[] outerTiles;

	public int columns = 8;
	public int rows = 8;

	private Transform boardHolder;

	public void Awake(){
		MenuSceneSetup ();
	}

	public void newGame() {
		Debug.Log ("New Game");

		//AdsManager adsmanager = new AdsManager ();
		//adsmanager.ShowAd ();

		Application.LoadLevel ("MainScence");
	}

	void MenuSceneSetup()
	{
		boardHolder = new GameObject ("Board").transform;
		
		for (int x = -1; x < columns +1; x ++) {
			for (int y = -1; y < rows +1; y ++) {
				GameObject toInstantiate = floorTiles[Random.Range(0,floorTiles.Length)];
				
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = outerTiles[Random.Range(0,outerTiles.Length)];	
				}
				
				GameObject instance = Instantiate(toInstantiate,  new Vector3(x,y,0.0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent(boardHolder);
			}
		}
		
	}

}
