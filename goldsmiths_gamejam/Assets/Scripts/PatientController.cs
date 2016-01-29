using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PatientController : MonoBehaviour {

	//List<Illness> illnesses;
	public Transform sittingPoint;
	public event Action seatReachedEvent;

	public float speed = 2.0f;

	// Use this for initialization
	void Start () {
		// Move towards the sitting position
		StartCoroutine (GoToSeat ());

	}
	IEnumerator GoToSeat()
	{
		while (Vector3.Distance(transform.position, sittingPoint.position) > 0.1f) {
			transform.position += (sittingPoint.position - transform.position).normalized * Time.deltaTime * speed;
			yield return null;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter(Collider other) {
		if (other.name == sittingPoint.name)
			OnSeatReached ();
	}

	// TO be called by Mecanim event or OnTriggerEnter!!
	public void OnSeatReached() 
	{
		if (seatReachedEvent != null)
			seatReachedEvent ();
	}
}
