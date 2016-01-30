using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour {

	GameManager gameManager;
	Text dayVal;
	Text deadVal;
	Text healedVal;
	Text moneyVal;


	void Init(){
		
		//Set the Dynamic Scores
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		dayVal = GameObject.Find ("DayVal").GetComponent<Text> ();
		dayVal.text = gameManager.currentDay.ToString();

		deadVal = GameObject.Find ("DeadVal").GetComponent<Text> ();
		deadVal.text = gameManager.currentDayDead.ToString();

		healedVal = GameObject.Find ("HealedVal").GetComponent<Text> ();
		healedVal.text = gameManager.currentDayHealed.ToString();

		moneyVal = GameObject.Find ("HealedVal").GetComponent<Text> ();
		moneyVal.text = gameManager.money.ToString();
	}

	public void BuyMask(int maskType){
		switch (maskType) {
		case 0:
			gameManager.money = gameManager.money - 5;	
			gameManager.currentMask = 0;
			break;
		case 1:
			gameManager.money = gameManager.money - 10;
			gameManager.currentMask = 1;
			break;
		case 2:
			gameManager.money = gameManager.money - 20;
			gameManager.currentMask = 2;
			break;
		}
	}

}