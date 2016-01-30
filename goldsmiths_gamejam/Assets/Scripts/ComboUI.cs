using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour {

    public GameObject upPrefab;
    public GameObject downPrefab;
    public GameObject rightPrefab;
    public GameObject leftPrefab;

    private static ComboUI instance;

    public static ComboUI Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<ComboUI>();
            }
            return instance;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            AddCombo("lur");
        } else if (Input.GetKeyDown(KeyCode.T)) {
            ClearChildren();
        }
    }

    public void AddCombo(string combo) {
        for (int i = combo.Length-1; i >= 0; --i) {
            if (combo[i] == 'l') {
                GameObject arrow = Instantiate(leftPrefab) as GameObject;
                arrow.transform.SetParent(gameObject.transform);
            } else if (combo[i] == 'r') {
                GameObject arrow = Instantiate(rightPrefab) as GameObject;
                arrow.transform.SetParent(gameObject.transform);
            } else if (combo[i] == 'u') {
                GameObject arrow = Instantiate(upPrefab) as GameObject;
                arrow.transform.SetParent(gameObject.transform);
            } else if (combo[i] == 'd') {
                GameObject arrow = Instantiate(downPrefab) as GameObject;
                arrow.transform.SetParent(gameObject.transform);
            }
        }
    }

    public void ClearChildren() {
        for (int i = 0; i < transform.childCount; ++i) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
