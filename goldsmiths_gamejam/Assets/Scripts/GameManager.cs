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
    private GameObject gameOverSplashScreen;
    private GameObject patientBar;

    // Vars for the battle
    public int currentDay;
    public int currentDayHealed;
    public int currentDayDead;
    public int money;

    public PatientController patient1;
    public PatientController patient2;
    public PatientController patient3;
    private PatientController currentPatient;

    public PatientController CurrentPatient
    {
        get { return currentPatient; }
    }

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
    public float patientTime;

    private int hardMode;
    private int powerUp;

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
        powerUp = 0;
        hardMode = PlayerPrefs.GetInt("hardmode");
        if (hardMode == 1) {
            dayLengh = 60.0f;
        } else {
            dayLengh = 45.0f;
        }

        patients = new Queue<PatientController>();

        pat1Pos = patient1.transform.position;
        pat2Pos = patient2.transform.position;
        pat3Pos = patient3.transform.position;

        currentPopulation = initialPopulation;

        dayIntroSplashScreen = GameObject.Find("DayIntroSplashScreen");
        dayEndSplashScreen = GameObject.Find("DayEndSplashScreen");
        gameOverSplashScreen = GameObject.Find("GameOverSplashScreen");

        patientBar = GameObject.Find("PatientTime");
        gameHUD = GameObject.Find("GameHUD");

        dayEndSplashScreen.SetActive(false);
        patientBar.SetActive(false);
        gameOverSplashScreen.SetActive(false);

        currentState = GameStates.INTRO;
    }

    protected override void OnUpdate() {
        //stateText.text = currentState.ToString();
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
            illPopulation = processedPatients = Mathf.Min(UnityEngine.Random.Range(8, 12), currentPopulation);
        } else {
            illPopulation = processedPatients = Mathf.Min(UnityEngine.Random.Range(5, 10), currentPopulation);
        }
        
        healedPopulation = 0;

        dayIntroSplashScreen.SetActive(true);
        gameHUD.SetActive(false);

        yield return new WaitForSeconds(2.0f);

        currentDayDead = 0;
        currentDayHealed = 0;

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

        if (currentPatient.currentIllnesses.Count == 0) {
            currentPatient.UpdateCurrentIllnesses();
            Debug.Log("*********** WAS 0 ************");
        }

        patientTime = patientTimeOut * Mathf.Max(1, currentPatient.currentIllnesses.Count);
        patientTimeOut = patientTime;
       // patientTime = patientTimeOut;

        if (powerUp == 1) {
            patientTime += 3.0f;
        }

        inputController.successComboEvent += OnPatientHealed;
        comboUI.AddCombo(inputController.GetTopCombo());
        patientBar.SetActive(true);
    }
    /*
    IEnumerator HEALING_EnterState() {
        yield return new WaitForSeconds(patientTimeOut);
        if ((GameStates)currentState == GameStates.HEALING) {
            currentPatient.Die();
            currentState = GameStates.DEAD;
        }
    }
    */
    void HEALING_Update() {

        patientTime -= Time.deltaTime;
        if(patientTime <= 0.0f) {
            currentPatient.Die();
            currentState = GameStates.DEAD;
        }

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
        patientBar.SetActive(false);
        patientTimeOut = 5.0f;

    }

    /************************************** HEALED **************************************/

    void HEALED_OnEnterState() {
        // Play healing animation
        ++healedPopulation;
        ++currentDayHealed;
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
        ++currentDayDead;
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

    //IEnumerator DAY_END_EnterState() {
    public void DAY_END_OnEnterState() {

        if (hardMode == 1) {
            money += healedPopulation;
        } else {
            money += 2 * healedPopulation;
        }

        powerUp = 0;

        comboUI.ClearChildren();

        gameHUD.SetActive(false);
        dayEndSplashScreen.SetActive(true);

        //yield return new WaitForSeconds(3.0f);

        ++currentDay;
        currentPopulation -= illPopulation;
        if (currentPopulation <= 0) {
            currentState = GameStates.GAME_OVER;
        } 
        
        /*else {
            currentState = GameStates.INTRO;
        }*/

        GameObject.Find("ConeContainer").GetComponent<Animator>().SetTrigger("exitDie");

    }

    void DAY_END_OnExitState() {
        dayEndSplashScreen.SetActive(false);

    }

    /************************************** GAME_OVER  **************************************/

    void GAME_OVER_OnEnterState()
    {
        gameOverSplashScreen.SetActive(true);
    }

    void GAME_OVER_OnExitState()
    {
        gameOverSplashScreen.SetActive(false);
    } 

    /************************************** CALL-BACKS  **************************************/

    void OnPatientSeated() {
        currentState = GameStates.HEALING;
    }

    void OnPatientHealed(bool finished, string illnessName) {
        currentPatient.OnIllnessRemoved(illnessName);

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

    public void OnMaskBought(int mask)
    {
        if (mask == 0) {
            powerUp = 0;
        }

        if (money >= 5 && mask == 1) {
            money -= 5;
            powerUp = 1;
        } else if (money < 5 && mask == 1) {
            return;
        }

        if (money >= 10 && mask == 2) {
            money -= 10;
            ++currentPopulation;
        } else if (money < 10 && mask == 2) {
            return;
        }

        if ((GameStates)currentState == GameStates.DAY_END)
            currentState = GameStates.INTRO;
    }

    public void ReplayOnClick()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
