using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class DayIntroSplashScreen : MonoBehaviour {
    public Text text;

	// Use this for initialization
	void OnEnable () {
        text = GetComponentInChildren<Text>();
        text.text = "DAY " + GameManager.Instance.currentDay;
	} 
	
	// Update is called once per frame
	void Update () {
	    
	}
}
