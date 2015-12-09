using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public static bool presentationMode = false;
	
	public float levelStartDelay = 2f;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	public float turnDelay = .1f;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private GameObject levelImage;
	private int level = 1;

	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;
	public bool loadedFromMenu = true;

	void Awake()
	{

		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}

		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy> ();
		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	
		
	}

	public void GameOver() 
	{
		levelText.text = "After " + level +" days, you starved. ";
		levelImage.SetActive (true);
		enabled = false;
		Invoke ("LoadMenuScene", levelStartDelay);
		loadedFromMenu = false;

	}

	private void LoadMenuScene(){
		Destroy(gameObject);
		Application.LoadLevel ("MenuScene");
	}

	void InitGame()
	{
		Debug.Log ("InitGame");
		if (!presentationMode) {
			doingSetup = true;
			levelImage = GameObject.Find ("LevelImage");
			levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
			levelText.text = "Day " + level;
			levelImage.SetActive (true);
			Invoke ("HideLevelImage", levelStartDelay);
		} else {
		
			doingSetup = false;
		}

		Debug.Log ("before clear enemies");
		enemies.Clear ();
		boardScript.setupScene (level);
		Debug.Log ("started setupScene");	
	}

	private void HideLevelImage() {
		levelImage.SetActive (false);
		doingSetup = false;
	}


	private void OnLevelWasLoaded (int index) {

		AdsManager adsmanager = new AdsManager ();
		adsmanager.ShowRewardedAd ();

		if (!loadedFromMenu) {
			Debug.Log ("OnLevelWasLoaded " + index);
			level++;
			InitGame();
		}
	}

	IEnumerator MoveEnemies() {

		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);

		if (enemies.Count == 0) {
			yield return new WaitForSeconds(turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) {
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		
		}

		playersTurn = true;
		enemiesMoving = false;
	
	}


	// Update is called once per frame
	void Update () {
		if (playersTurn || enemiesMoving || doingSetup) {
			return;
		}

		StartCoroutine (MoveEnemies ());
	}


	public void addEnemiesToList(Enemy script) {
		enemies.Add (script);
	}

}
