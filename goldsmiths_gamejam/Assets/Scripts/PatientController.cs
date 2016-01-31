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

	Animator animator;

	public float speed = 2.0f;

    private ParticleSystem particleSystem;
    private Animator coneAnimator;

	// Use this for initialization
	void Start () {
		
		animator = transform.GetChild (0).GetComponent<Animator> ();

		sittingPoint = GameObject.Find ("SeatPosition").transform;
        healedExitPoint = GameObject.Find("HealedExit").transform;
        entrancePoint = GameObject.Find("TentEnterPosition").transform;
        deathExitPoint = GameObject.Find("DeadExit").transform;
        coneAnimator = GameObject.Find("ConeContainer").GetComponent<Animator>();
        particleSystem = GameObject.Find("PurplePS").GetComponent<ParticleSystem>();
        particleSystem.playOnAwake = true;
        //particleSystem.gameObject.SetActive(false);

		targetPos = sittingPoint.position;
		targetPos.y = transform.position.y;
		// Move towards the sitting position
	}

	public void GoToSeat()
	{
		audio1.Play ();

		animator.SetBool ("isWalking", true);
		StartCoroutine (GotToPos (sittingPoint.position, OnSeatReached));
	}

    public void GoToEntrance() {
		animator.SetBool ("isWalking", true);
        StartCoroutine(GotToPos(entrancePoint.position));
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

	// Update is called once per frame
	void Update () {
		
	}

	public void Heal() {
		audio2.Stop ();
		audio4.Play ();
        particleSystem.gameObject.SetActive(true);
		// Walk Away
        StartCoroutine(GotToPos(healedExitPoint.position, () => {
            Debug.Log("Patient Healed!");
            if (exitReachedEvent != null) {
                exitReachedEvent();
                transform.position = entrancePoint.position - entrancePoint.forward * 1.25f;
            }
        }));
	}

    public void Die() {
		audio2.Stop ();
		audio3.Play ();
        coneAnimator.SetTrigger("enterDie");
        StartCoroutine(GotToPos(deathExitPoint.position, () => {
            Debug.Log("Patient Died!");
            coneAnimator.SetTrigger("exitDie");
            if (deathExitReachedEvent != null) {
                deathExitReachedEvent();
                transform.position = entrancePoint.position - entrancePoint.forward * 1.25f;
            }
        }));
    }

    public void CloseLights() {
        coneAnimator.SetTrigger("exitDie");
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
			animator.SetBool ("isWalking", false);
			audio1.Stop ();
			audio2.Play ();
			seatReachedEvent ();
		}
	}
	
}
