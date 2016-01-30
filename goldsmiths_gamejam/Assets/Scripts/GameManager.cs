using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : StateMachineBase 
{

	public enum GameStates { INTRO, PATIENT_ENTER, HEALING, HEALED, DEAD, DAY_END, GAME_OVER }
	InputController inputController;

	public int currentDay;
	public int currentPopulation;
	public int initialPopulation;

    // Update every day
    private int illPopulation;
    private int healedPopulation;

	public PatientController patient1;
    public PatientController patient2;
    public PatientController patient3;
    private PatientController currentPatient;

	private static GameManager instance;
	public Text stateText;

    private Queue<PatientController> patients;



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
        currentDay = 1;

        patients = new Queue<PatientController>();
        patients.Enqueue(patient1);
        patients.Enqueue(patient2);
        patients.Enqueue(patient3);
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
        currentPopulation -= (illPopulation - healedPopulation);
        illPopulation = Mathf.Min(UnityEngine.Random.Range(10, 25), currentPopulation);
        healedPopulation = 0;
		yield return new WaitForSeconds (2.0f);
		currentState = GameStates.PATIENT_ENTER;
	}


	
	void PATIENT_ENTER_OnEnterState()
	{
		// Patient animation
		// Playing silhouette animation
		// Instantiate patient
        currentPatient = patients.Dequeue();
		currentPatient.GoToSeat ();

        PatientController p = patients.Peek();
        p.GoToEntrance();

        GetComponent<InputController>().UpdateCurrentIllnesses();

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
        currentPatient.exitReachedEvent += OnPatientLeft;
	}

	void HEALED_Update()
	{

	}

    void HEALED_OnExitState() {
        patients.Enqueue(currentPatient);
        currentPatient.exitReachedEvent -= OnPatientLeft;
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

    void DEAD_OnExitState() {
        patients.Enqueue(currentPatient);
    }

	void OnPatientSeated()
	{
		currentState = GameStates.HEALING;
	}

	void OnPatientHealed(bool finished)
	{
		
        if (finished) {
            currentState = GameStates.HEALED;
            currentPatient.Heal();
        } else {
            //flash
        }
	}


	void OnPatientDead()
	{
		currentState = GameStates.DEAD;
	}

    void OnPatientLeft() {
        currentState = GameStates.PATIENT_ENTER;
    }
}
