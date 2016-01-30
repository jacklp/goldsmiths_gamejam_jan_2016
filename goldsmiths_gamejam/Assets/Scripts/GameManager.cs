using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : StateMachineBase {

    public enum GameStates { INTRO, PATIENT_ENTER, HEALING, HEALED, DEAD, DAY_END, GAME_OVER }
    private InputController inputController;
    private ComboUI comboUI;

    // Vars for the War
    public int currentPopulation;
    public int initialPopulation;

    // Update every day
    public int illPopulation;
    public int healedPopulation;
    public int processedPatients;

    // UI References
    public GameObject gameHUD;
    private GameObject dayIntroSplashScreen;
    private GameObject dayEndSplashScreen;

    // Vars for the battle
    public int currentDay;
    public int currentDayHealed;
    public int currentDayDead;
    public int currentMask;
    public int money;

    public PatientController patient1;
    public PatientController patient2;
    public PatientController patient3;
    private PatientController currentPatient;

    private Vector3 pat1Pos;
    private Vector3 pat2Pos;
    private Vector3 pat3Pos;

    private static GameManager instance;
    public Text stateText;

    private Queue<PatientController> patients;

    public float dayLengh = 60.0f;
    private float dayEnd;
    public float DayEnd { get { return dayEnd; } }
    public float patientTimeOut = 5.0f;

    private int hardMode;

    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public void Start() {
        inputController = GetComponent<InputController>();
        comboUI = ComboUI.Instance;
        comboUI.enabled = false;

        currentDay = 1;
        hardMode = PlayerPrefs.GetInt("hardmode");
        if (hardMode == 1) {
            dayLengh = 45.0f;
        } else {
            dayLengh = 60.0f;
        }

        patients = new Queue<PatientController>();

        pat1Pos = patient1.transform.position;
        pat2Pos = patient2.transform.position;
        pat3Pos = patient3.transform.position;

        currentPopulation = initialPopulation;

        dayIntroSplashScreen = GameObject.Find("DayIntroSplashScreen");
        dayEndSplashScreen = GameObject.Find("DayEndSplashScreen");
        dayEndSplashScreen.SetActive(false);

        currentState = GameStates.INTRO;
    }

    protected override void OnUpdate() {
        stateText.text = currentState.ToString();
    }

    /************************************** DAY - START **************************************/

    IEnumerator INTRO_EnterState() {
        // Initialize stuff
        // Current Day screen
        // Instantiate stuff
        // Reset stuff
        patients.Clear();
        patients.Enqueue(patient1);
        patients.Enqueue(patient2);
        patients.Enqueue(patient3);

        patient1.StopAllCoroutines();
        patient2.StopAllCoroutines();
        patient3.StopAllCoroutines();

        patient1.transform.position = pat1Pos;
        patient2.transform.position = pat2Pos;
        patient3.transform.position = pat3Pos;

        dayEnd = Time.time + dayLengh;
        if (hardMode == 1) {
            illPopulation = processedPatients = Mathf.Min(UnityEngine.Random.Range(20, 30), currentPopulation);
        } else {
            illPopulation = processedPatients = Mathf.Min(UnityEngine.Random.Range(10, 20), currentPopulation);
        }
        
        healedPopulation = 0;

        dayIntroSplashScreen.SetActive(true);
        gameHUD.SetActive(false);

        yield return new WaitForSeconds(2.0f);



        currentState = GameStates.PATIENT_ENTER;
    }

    void INTRO_OnExitState() {
        dayIntroSplashScreen.SetActive(false);
        gameHUD.SetActive(true);
    }
    /************************************** PATIENT - ENTER **************************************/

    void PATIENT_ENTER_OnEnterState() {
        // Patient animation
        // Playing silhouette animation
        // Instantiate patient
        currentPatient = patients.Dequeue();
        currentPatient.GoToSeat();

        PatientController p = patients.Peek();
        p.GoToEntrance();

        GetComponent<InputController>().UpdateCurrentIllnesses();

        Debug.Log("Patient Enter");
        currentPatient.seatReachedEvent += OnPatientSeated;

        --processedPatients;
    }

    /************************************** HEALING - START **************************************/

    void HEALING_OnEnterState() {
        comboUI.enabled = true;
        inputController.successComboEvent += OnPatientHealed;
        comboUI.AddCombo(inputController.GetTopCombo());
    }

    IEnumerator HEALING_EnterState() {
        yield return new WaitForSeconds(patientTimeOut);
        if ((GameStates)currentState == GameStates.HEALING) {
            currentPatient.Die();
            currentState = GameStates.DEAD;
        }
    }

    void HEALING_Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            currentPatient.Heal();
            currentState = GameStates.HEALED;
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            currentPatient.Die();
            currentState = GameStates.DEAD;
        }
            
    }

    void HEALING_OnExitState() {
        comboUI.enabled = false;
        comboUI.ClearChildren();
        inputController.successComboEvent -= OnPatientHealed;
    }

    /************************************** HEALED **************************************/

    void HEALED_OnEnterState() {
        // Play healing animation
        ++healedPopulation;
        --illPopulation;

        currentPatient.exitReachedEvent += OnPatientLeft;
        if (Time.time >= dayEnd || processedPatients <= 0) {
            currentState = GameStates.DAY_END;
        }
    }

    void HEALED_Update() {

    }

    void HEALED_OnExitState() {
        patients.Enqueue(currentPatient);
        currentPatient.exitReachedEvent -= OnPatientLeft;
    }

    /************************************** DEAD **************************************/

    /*IEnumerator DEAD_EnterState() {
        --currentPopulation;
        --illPopulation;

        yield return new WaitForSeconds(1.5f);

        if (Time.time >= dayEnd || processedPatients <= 0) {
            currentState = GameStates.DAY_END;
        } else {
            currentState = GameStates.PATIENT_ENTER;
        }
    }*/

    void DEAD_OnEnterState() {
        --currentPopulation;
        --illPopulation;
        currentPatient.deathExitReachedEvent += OnPatientDied;

        if (Time.time >= dayEnd || processedPatients <= 0) {
            currentState = GameStates.DAY_END;
        }
    }

    void DEAD_Update() {

    }

    void DEAD_OnExitState() {
        patients.Enqueue(currentPatient);
        currentPatient.deathExitReachedEvent -= OnPatientDied;
    }

    /************************************** DAY - END  **************************************/

    IEnumerator DAY_END_EnterState() {

        money = money - currentDayDead < 0 ? 0 : money - currentDayDead;
        money = money + currentDayHealed;

        comboUI.ClearChildren();

        gameHUD.SetActive(false);
        dayEndSplashScreen.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        ++currentDay;
        currentPopulation -= illPopulation;
        if (currentPopulation <= 0) {
            currentState = GameStates.GAME_OVER;
        } else {
            currentState = GameStates.INTRO;
        }
    }

    void DAY_END_OnExitState() {
        dayEndSplashScreen.SetActive(false);

    }


    /************************************** CALL-BACKS  **************************************/

    void OnPatientSeated() {
        currentState = GameStates.HEALING;
    }

    void OnPatientHealed(bool finished) {
        if (finished) {
            currentState = GameStates.HEALED;
            currentPatient.Heal();
        } else {
            //flash
        }
    }

    void OnPatientDead() {
        currentState = GameStates.DEAD;
    }

    void OnPatientLeft() {
        if ((GameStates)currentState == GameStates.HEALED)
            currentState = GameStates.PATIENT_ENTER;
    }

    void OnPatientDied() {
        if ((GameStates)currentState == GameStates.DEAD) {
            currentState = GameStates.PATIENT_ENTER;
        }
    }

}
