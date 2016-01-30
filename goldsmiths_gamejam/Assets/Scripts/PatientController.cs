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

	public float speed = 2.0f;

	// Use this for initialization
	void Start () {

		sittingPoint = GameObject.Find ("SeatPosition").transform;
        healedExitPoint = GameObject.Find("HealedExit").transform;
        entrancePoint = GameObject.Find("TentEnterPosition").transform;
        deathExitPoint = GameObject.Find("DeadExit").transform;

		targetPos = sittingPoint.position;
		targetPos.y = transform.position.y;
		// Move towards the sitting position
	}

	public void GoToSeat()
	{
		audio1.Play ();
		StartCoroutine (GotToPos (sittingPoint.position, OnSeatReached));
	}

    public void GoToEntrance() {
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
        StartCoroutine(GotToPos(deathExitPoint.position, () => {
            Debug.Log("Patient Died!");
            if (deathExitReachedEvent != null) {
                deathExitReachedEvent();
                transform.position = entrancePoint.position - entrancePoint.forward * 1.25f;
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
		if (seatReachedEvent != null)
			audio1.Stop ();
			audio2.Play ();
			seatReachedEvent ();
	}
	
}
