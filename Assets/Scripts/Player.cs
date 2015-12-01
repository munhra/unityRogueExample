using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

	public int wallDamange = 1;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f;
	public Text foodText;

	private Animator animator;
	private int food;

	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator> ();
		food = GameManager.instance.playerFoodPoints;
		foodText.text = "Food: " + food;
		base.Start ();
	}

	protected override void AttemptMove<T> (int xDir, int yDir)
	{
		Debug.Log ("AttemptMove xDir " + xDir);
		Debug.Log ("AttemptMove yDir " + yDir);
		food--;
		foodText.text = "Food: " + food;
		//Debug.Break ();

		base.AttemptMove <T> (xDir, yDir);

		RaycastHit2D hit;

		CheckIfGameOver ();

		GameManager.instance.playersTurn = false;


	}

	private void OnTriggerEnter2D (Collider2D other) {

		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Food") {
			food += pointsPerFood;
			foodText.text = "+" + pointsPerFood + " Food: " + food;
			other.gameObject.SetActive (false);
		} else if (other.tag == "Soda") {
			food += pointsPerSoda;
			foodText.text = "+" + pointsPerFood + " Food: " + food;
			other.gameObject.SetActive (false);
		}
	}

	protected override void OnCantMove<T> (T component) {
	
		Wall hitwall = component as Wall;
		hitwall.DamageWall (wallDamange);
		animator.SetTrigger ("PlayerChop");
	}

	private void Restart() {
		Application.LoadLevel (Application.loadedLevel);
	}

	public void LooseFood(int loss) {
		animator.SetTrigger ("PlayerHit");
		food -= loss;
		foodText.text = "-" + loss + " Food: " + food;
		CheckIfGameOver ();
	}

	//protected override void OnCantMove<T>

	private void OnDisable()
	{
		GameManager.instance.playerFoodPoints = food;
	}

	private void CheckIfGameOver()
	{
		if (food <= 0) {
			GameManager.instance.GameOver();
		}

	}

	// Update is called once per frame
	void Update () {

		if (!GameManager.instance.playersTurn) {
			//Debug.Log ("Attempt to move");
			return;
		}

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int) Input.GetAxisRaw ("Horizontal");
		vertical = (int) Input.GetAxisRaw ("Vertical");

		if (horizontal != 0) {
			//Debug.Log ("Attempt to move horizontal");
			vertical = 0;
		}

		if (horizontal != 0 || vertical != 0) {
			//Debug.Log ("Player Attempt to move vertical");
			AttemptMove<Wall>(horizontal,vertical);
		}

	}
}
