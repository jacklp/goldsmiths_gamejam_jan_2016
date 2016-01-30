using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : StateMachineBase 
{

	public enum GameStates { INTRO, PATIENT_ENTER, HEALING, HEALED, DEAD, DAY_END, GAME_OVER }
	InputController inputController;

	// Vars for the War
	public int currentPopulation;
	public int initialPopulation;



	// Vars for the battle
	public int currentDay;
	public int currentDayHealed;
	public int currentDayDead;
	public int currentMask;
	public int money;



	public PatientController currentPatient;

	private static GameManager instance;
	public Text stateText;



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
		inputController = GetComponent<InputController> ();
		currentState = GameStates.INTRO;
	}

	protected override void OnUpdate() {
		stateText.text = currentState.ToString ();
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
		currentPatient.GoToSeat ();

		Debug.Log ("Patient Enter");
		currentPatient.seatReachedEvent += OnPatientSeated;
	}

	void HEALING_OnEnterState()
	{
		inputController.successComboEvent += OnPatientHealed;
	}

	void HEALING_Update()
	{
		if (Input.GetKeyDown (KeyCode.Q)) {
			currentPatient.Heal();
			currentState = GameStates.HEALED;
		}

		if (Input.GetKeyDown (KeyCode.W))
			currentState = GameStates.DEAD;
	}

	void HEALING_OnExitState() 
	{
		inputController.successComboEvent -= OnPatientHealed;
	}

	void HEALED_OnEnterState()
	{
		// Play healing animation
	}

	void HEALED_Update()
	{

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

	void DAY_END_OnEnterState(){

		money = money - currentDayDead < 0 ?  0 : money - currentDayDead;
		money = money + currentDayHealed;
	}

	void OnPatientSeated()
	{
		currentState = GameStates.HEALING;
	}

	void OnPatientHealed()
	{
		currentState = GameStates.HEALED;
		currentPatient.Heal ();
	}


	void OnPatientDead()
	{
		currentState = GameStates.DEAD;
	}


}
