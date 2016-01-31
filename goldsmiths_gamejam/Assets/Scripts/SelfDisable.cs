using UnityEngine;
using System.Collections;

public class SelfDisable : MonoBehaviour {

    public float timeOut = 3.0f;
    private float disableTime;

    void OnEnable() {
        disableTime = Time.time + timeOut;
    }
	
	void Update () {
        if (Time.time >= disableTime) {
            gameObject.SetActive(false);
        }
	}
}
