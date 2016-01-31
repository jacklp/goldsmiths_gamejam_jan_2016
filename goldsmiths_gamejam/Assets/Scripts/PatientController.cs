using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PatientController : MonoBehaviour {

	public Transform sittingPoint;
	public Transform healedExitPoint;
    public Transform entrancePoint;
    public Transform deathExitPoint;

	public AudioSource audio1;
	public AudioSource audio2;
	public AudioSource audio3;
	public AudioSource audio4;

	Vector3 targetPos;

	public event Action seatReachedEvent;
    public event Action exitReachedEvent;
    public event Action deathExitReachedEvent;

    public Animator animator; /*** KUNKUNKUNKUN ***/

	public float speed = 2.0f;

    private ParticleSystem particleSystem;
    private Animator coneAnimator; /*** KUNKUNKUNKUN ***/

    public List<Illness> currentIllnesses;

    public GameObject[] wounds;

	// Use this for initialization
	void Start () {
        currentIllnesses = new List<Illness>();

		animator = transform.GetChild (0).GetComponent<Animator> ();

		sittingPoint = GameObject.Find ("SeatPosition").transform;
        healedExitPoint = GameObject.Find("HealedExit").transform;
        entrancePoint = GameObject.Find("TentEnterPosition").transform;
        deathExitPoint = GameObject.Find("DeadExit").transform;
        coneAnimator = GameObject.Find("ConeContainer").GetComponent<Animator>(); /*** KUNKUNKUNKUN ***/
        particleSystem = GameObject.Find("PurplePS").GetComponent<ParticleSystem>();
        particleSystem.playOnAwake = true;
        //particleSystem.gameObject.SetActive(false);

		targetPos = sittingPoint.position;
		targetPos.y = transform.position.y;

        ClearLooks();
        UpdateCurrentIllnesses();

		// Move towards the sitting position
	}

    public void UpdateCurrentIllnesses()
    {
        currentIllnesses.Clear();
        int nrIllnesses;

        int day = GameManager.Instance.currentDay;
        if (PlayerPrefs.GetInt("hardmode") == 1)
        {
            nrIllnesses = UnityEngine.Random.Range(Mathf.Max(4, day / 3), Mathf.Max(5, day / 2));
        }
        else
        {
            nrIllnesses = UnityEngine.Random.Range(Mathf.Max(1, day / 3), Mathf.Max(3, day / 2));
        }
        nrIllnesses = (nrIllnesses > 6) ? 6 : nrIllnesses;
        int limit = 5;
        for (uint i = 0; i < nrIllnesses; ++i)
        {
            int index = UnityEngine.Random.Range(0, limit);
            currentIllnesses.Add(InputController.IllnessesDef[index]);
            Illness temp = InputController.IllnessesDef[index];
            InputController.IllnessesDef[index] = InputController.IllnessesDef[5];
            InputController.IllnessesDef[5] = temp;
            --index;
        }
        UpdateLooks();
    }

    public void UpdateLooks()
    {
        Debug.Log("ILLNESES LIST: ------------------");
        foreach (Illness i in currentIllnesses)
        {
            string name = i.GetName();
            Debug.Log(name);
            if (name == "Eye")
                wounds[0].SetActive(true);
            else if (name == "Axe")
                wounds[1].SetActive(true);
            else if (name == "Arrow")
                wounds[2].SetActive(true);
            else if (name == "Knife")
                wounds[3].SetActive(true);
            else if (name == "Hair")
                wounds[4].SetActive(false);
            else if (name == "Green")
                wounds[5].SetActive(true);
        }
        Debug.Log("ILLNESES END: ------------------");

    }

    public void ClearLooks()
    {
        foreach(GameObject g in wounds)
        {
            g.SetActive(false);
        }
        wounds[4].SetActive(true);

    }

	public void GoToSeat()
	{
		audio1.Play ();

		animator.SetBool ("isWalking", true); /*** KUNKUNKUNKUN ***/
		StartCoroutine (GotToPos (sittingPoint.position, OnSeatReached));
	}

    public void GoToEntrance() {
        animator.Play("Idle"); /*** KUNKUNKUNKUN ***/

        animator.SetBool("isWalking", true); /*** KUNKUNKUNKUN ***/
        StartCoroutine(GotToPos(entrancePoint.position));//, () => { animator.SetBool("isWalking", false); }));
    }

	IEnumerator GotToPos(Vector3 pos, Action onFinish = null)
	{
		//pos.y = transform.position.y;
		while (Vector3.Distance(transform.position, pos) > 0.1f) {
			transform.position += (pos - transform.position).normalized * Time.deltaTime * speed;
			yield return null;
		}
        if (onFinish != null) {
            onFinish();
        }
	}
    public bool isMoving;
    Vector3 lastPos;

	// Update is called once per frame
	void Update () {
        Vector3 speed = (transform.position - lastPos) / Time.deltaTime;
        isMoving = (speed.sqrMagnitude > 0.01f);
        animator.SetBool("isWalking", isMoving); /*** KUNKUNKUNKUN ***/
        lastPos = transform.position;
	}

	public void Heal() {
		audio2.Stop ();
		audio4.Play ();
        particleSystem.gameObject.SetActive(true);
		// Walk Away
        StartCoroutine(GotToPos(healedExitPoint.position, () => {
            Debug.Log("Patient Healed!");

            ClearLooks();
            if (exitReachedEvent != null) {
                exitReachedEvent();
                transform.position = entrancePoint.position - entrancePoint.forward * 1.25f;
                UpdateCurrentIllnesses();

            }
        }));
	}

    public void Die() {
		audio2.Stop ();
		audio3.Play ();
        coneAnimator.SetTrigger("enterDie"); /*** KUNKUNKUNKUN ***/
        animator.Play("Dead"); /*** KUNKUNKUNKUN ***/

        StartCoroutine(GotToPos(deathExitPoint.position, () => {
            Debug.Log("Patient Died!");

            ClearLooks();
            if (deathExitReachedEvent != null) {
                deathExitReachedEvent();
                transform.position = entrancePoint.position - entrancePoint.forward * 1.25f;
                coneAnimator.SetTrigger("exitDie"); /*** KUNKUNKUNKUN ***/

                animator.Play("Idle"); /*** KUNKUNKUNKUN ***/


                UpdateCurrentIllnesses();
            }
        }));
    }

	/*
	public void OnTriggerEnter(Collider other) {
		if (other.name == sittingPoint.name)
			OnSeatReached ();
	}
*/
	// TO be called by Mecanim event or OnTriggerEnter!!
	public void OnSeatReached() 
	{
		if (seatReachedEvent != null) {
            animator.SetBool("isWalking", false); /*** KUNKUNKUNKUN ***/
			audio1.Stop ();
			audio2.Play ();
			seatReachedEvent ();
		}
	}

    public void OnIllnessRemoved(string name)
    {
        int remIndex = -1;
        foreach(Illness i in currentIllnesses)
        {
            if (i.GetName() == name)
            {
                remIndex = currentIllnesses.IndexOf(i);
                break;
            }
        }
        if (remIndex != -1)
            currentIllnesses.RemoveAt(remIndex);

        Debug.Log("ILLNESS REMOVED " + name);

        if (name == "Eye")
            wounds[0].GetComponent<ItemRemove>().VanishItem();
        else if (name == "Axe")
            wounds[1].GetComponent<ItemRemove>().VanishItem();
        else if (name == "Arrow")
            wounds[2].GetComponent<ItemRemove>().VanishItem();
        else if (name == "Knife")
            wounds[3].GetComponent<ItemRemove>().VanishItem();
        else if (name == "Hair")
        {
            wounds[4].SetActive(true);
            wounds[4].GetComponent<ItemRemove>().VanishItem(true);
        }
        else if (name == "Green")
        
            wounds[5].GetComponent<ItemRemove>().VanishItem();
        
        
            //wounds[0].SetActive(false);
    }

}
