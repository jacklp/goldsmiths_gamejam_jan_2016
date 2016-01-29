using UnityEngine;
using System.Collections;

public class GameManager : StateMachineBase 
{

	public enum GameStates { INTRO, PATIENT_ENTER, HEALING, HEALED, DEAD, DAY_END, GAME_OVER }

	public int currentDay;
	public int currentPopulation;
	public int initialPopulation;

	public PatientController currentPatient;

	private static GameManager instance;
	
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<GameManager>();
			}
			return instance;
		}
	} 

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
		// Instantiate patient


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
		currentPopulation--;
		if (currentPopulation <= 0) {
			currentState = GameStates.GAME_OVER;
		}
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
