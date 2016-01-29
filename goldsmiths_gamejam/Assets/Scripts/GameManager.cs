using UnityEngine;
using System.Collections;

public class GameManager : StateMachineBase 
{

	public enum GameStates { INTRO, PATIENT_ENTER, HEALING, HEALED, DEAD, DAY_END }

	public int currentDay;
	public int currentPopulation;
	public int initialPopulation;

	public void Start() {
		currentState = GameStates.INTRO;
	}

	IEnumerator INTRO_EnterState()
	{
		// Initialize stuff
		// Current Day screen
		// Instantiate stuff
		// Reset stuff
		yield return new WaitForSeconds (2.0f);
		currentState = GameStates.PATIENT_ENTER;
	}


	
	void PATIENT_ENTER_OnEnterState()
	{
		// Patient animation
		// Playing silhouette animation
		// 

		Debug.Log ("Patient Enter");
	}

	void HEALING_OnEnterState()
	{
	}

	void HEALING_Update()
	{
		if (Input.GetKeyDown (KeyCode.Q))
			currentState = GameStates.HEALED;

		if (Input.GetKeyDown (KeyCode.W))
			currentState = GameStates.DEAD;
	}

	void HEALED_OnEnterState()
	{
	}

	void HEALED_Update()
	{
		if (Input.GetKeyDown (KeyCode.W))
			currentState = GameStates.DEAD;
	}

	void DEAD_OnEnterState()
	{
	}
	void DEAD_Update()
	{
		
	}

	void OnPatientHealed()
	{
		currentState = GameStates.HEALED;
	}


	void OnPatientDead()
	{
		currentState = GameStates.DEAD;
	}
}
