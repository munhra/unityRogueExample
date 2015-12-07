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

	public AudioClip moveSound1;
	public AudioClip moveSound2;
	public AudioClip eatSound1;
	public AudioClip eatSound2;
	public AudioClip drinkSound1;
	public AudioClip drinkSound2;
	public AudioClip gameOverSound;

	private Vector2 touchOrigin = -Vector2.one;

	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator> ();
		food = GameManager.instance.playerFoodPoints;
		foodText.text = "Food: " + food;
		base.Start ();
	}

	protected override void AttemptMove<T> (int xDir, int yDir)
	{

		food--;
		foodText.text = "Food: " + food;

		base.AttemptMove <T> (xDir, yDir);

		RaycastHit2D hit;


		if (Move (xDir,yDir, out hit)) {
			SoundManager.instance.RandomizeSfx(moveSound1,moveSound2);
		}

		CheckIfGameOver ();

		GameManager.instance.playersTurn = false;


	}

	private void OnTriggerEnter2D (Collider2D other) {

		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Food") {
			food += pointsPerFood;
			SoundManager.instance.RandomizeSfx(eatSound1,eatSound1);
			foodText.text = "+" + pointsPerFood + " Food: " + food;
			other.gameObject.SetActive (false);
		} else if (other.tag == "Soda") {
			food += pointsPerSoda;
			SoundManager.instance.RandomizeSfx(drinkSound1,drinkSound2);
			foodText.text = "+" + pointsPerFood + " Food: " + food;
			other.gameObject.SetActive (false);
		}
	}

	protected override void OnCantMove<T> (T component) {
	
		Wall hitwall = component as Wall;
		hitwall.DamageWall (wallDamange);
		animator.SetTrigger ("playerChop");
	}

	private void Restart() {
		GameManager.instance.loadedFromMenu = false;
		Application.LoadLevel (Application.loadedLevel);
	}

	public void LooseFood(int loss) {
		animator.SetTrigger ("playerHit");
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
			SoundManager.instance.PlaySingle(gameOverSound);
			SoundManager.instance.musicSource.Stop();
			GameManager.instance.GameOver();
		}

	}

	// Update is called once per frame
	void Update () {

		if (!GameManager.instance.playersTurn) {
			return;
		}

		int horizontal = 0;
		int vertical = 0;
#if UNITY_STANDALONE || UNITY_WEBPLAYER
		horizontal = (int) Input.GetAxisRaw ("Horizontal");
		vertical = (int) Input.GetAxisRaw ("Vertical");
		if (horizontal != 0) {
			vertical = 0;
		}
#else

		if (Input.touchCount > 0) {

			Touch mytouch = Input.touches[0];

			if (mytouch.phase == TouchPhase.Began) {
				touchOrigin = mytouch.position;
			} else if (mytouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
			
				Vector2 touchEnded = mytouch.position;
				float x = touchEnded.x - touchOrigin.x;
				float y = touchEnded.y - touchOrigin.y;

				touchOrigin.x = -1;

				if (Mathf.Abs(x) > Mathf.Abs(y)) {
					horizontal = x > 0 ? 1 : -1;
				}else{

					vertical = y > 0 ? 1 : -1;
				}
			}

		}
#endif
		if (horizontal != 0 || vertical != 0) {
			AttemptMove<Wall>(horizontal,vertical);
		}

	}
}
