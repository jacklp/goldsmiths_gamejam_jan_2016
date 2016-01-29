using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PatientController : MonoBehaviour {

	public Transform sittingPoint;
	public Transform exitPoint;

	Vector3 targetPos;

	public event Action seatReachedEvent;

	public float speed = 2.0f;

	// Use this for initialization
	void Start () {
		sittingPoint = GameObject.Find ("Seat").transform;
		exitPoint = GameObject.Find ("Exit").transform;
		targetPos = sittingPoint.position;
		targetPos.y = transform.position.y;
		// Move towards the sitting position
	}

	public void GoToSeat()
	{
		StartCoroutine (GotToPos (sittingPoint.position, OnSeatReached));
	}

	IEnumerator GotToPos(Vector3 pos, Action onFinish)
	{
		pos.y = transform.position.y;
		while (Vector3.Distance(transform.position, pos) > 0.1f) {
			transform.position += (pos - transform.position).normalized * Time.deltaTime * speed;
			yield return null;
		}
		onFinish ();
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void Heal() {
		// Walk Away
		StartCoroutine (GotToPos (exitPoint.position, () => {Debug.Log("Patient Healed!");}));
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
			seatReachedEvent ();
	}
	
}
