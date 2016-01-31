using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour {

	GameManager gameManager;
	Text dayVal;
	Text deadVal;
	Text healedVal;
	Text moneyVal;


	void OnEnable(){
		
		//Set the Dynamic Scores
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		dayVal = GameObject.Find ("DayVal").GetComponent<Text> ();
		dayVal.text = gameManager.currentDay.ToString();

		deadVal = GameObject.Find ("DeadVal").GetComponent<Text> ();
		deadVal.text = (gameManager.currentDayDead + gameManager.illPopulation).ToString();

		healedVal = GameObject.Find ("HealedVal").GetComponent<Text> ();
		healedVal.text = gameManager.currentDayHealed.ToString();

		moneyVal = GameObject.Find ("MoneyVal").GetComponent<Text> ();
		moneyVal.text = gameManager.money.ToString();
	}
}