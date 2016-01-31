using UnityEngine;
using System.Collections;

public class ItemRemove : MonoBehaviour {
    public Renderer renderer;
    float disableTime; 
    private ParticleSystem ps;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        ps = GetComponent<ParticleSystem>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void VanishItem(bool enable = false)
    {
        renderer.enabled = enable;
        ps.Emit(100);
        disableTime = Time.time + 2.0f;
    }

}
