using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour {

    public Text dayLabel;
    public Text illLabel;
    public Text populationLabel;

    private GameManager gm;

	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	void Update () {
        dayLabel.text = "Day: " + gm.currentDay;
        illLabel.text = "Wounded: " + gm.illPopulation;
        populationLabel.text = "Tribe Warriors: " + gm.currentPopulation;
	}
}
