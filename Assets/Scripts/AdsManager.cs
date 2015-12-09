﻿using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {
	
	public void ShowAd()
	{
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
		}
	}
	

	public void ShowRewardedAd()

	{
		//Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		//Debug.Log ("Food player " + player.food);
		//player.food = player.food + 50;

		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}
		
	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");

			//Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			//player.food = player.food + 50;

			GameManager.instance.playerFoodPoints = GameManager.instance.playerFoodPoints + 50;


				//
				// YOUR CODE TO REWARD THE GAMER
				// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}


}
