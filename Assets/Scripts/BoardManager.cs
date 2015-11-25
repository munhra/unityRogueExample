using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimun;
		public int maximun;

		public Count(int min, int max){
			minimun = min;
			maximun = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5,9);
	public Count foodCount = new Count(1,5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerTiles;

	private Transform boardHolder;
	private List<Vector3> gridPostions = new List<Vector3>();

	// it leaves one square wihout any object to not create an impossible level
	void InitializeList()
	{
		gridPostions.Clear ();
		for (int x = 1; x < columns -1; x ++) {

			for (int y = 1; y < rows -1; y ++) {
				gridPostions.Add(new Vector3(x,y,0.0f));
			}
		}
	}

	void BoardSetup()
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

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPostions.Count);
		Vector3 randomPosition = gridPostions [randomIndex];
		gridPostions.RemoveAt (randomIndex);

		return randomPosition;
	}
	
	void LayoutObjectAtRandomPosition(GameObject[] tileArray, int minimun, int maximun){
		int objectCount = Random.Range (minimun, maximun + 1);

		for (int i = 0; i <objectCount; i ++) {
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0,tileArray.Length)];
			Instantiate(tileChoice,randomPosition,Quaternion.identity);
		}

	}

	public void setupScene(int level)
	{
		BoardSetup ();

		InitializeList ();
		LayoutObjectAtRandomPosition (wallTiles, wallCount.maximun, wallCount.minimun);
		LayoutObjectAtRandomPosition (foodTiles, foodCount.maximun, foodCount.minimun);

		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandomPosition (enemyTiles, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0), Quaternion.identity);

	}
}
