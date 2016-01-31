using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour {

    public GameObject upPrefab;
    public GameObject downPrefab;
    public GameObject rightPrefab;
    public GameObject leftPrefab;

    private static ComboUI instance;

    private string currentCombo = "";
    private int currentPos;

    private Animation flashAnim;

    public static ComboUI Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<ComboUI>();
            }
            return instance;
        }
    }

    void Start() {
        flashAnim = GetComponent<Animation>();
    }

    void Update() {
        int index = currentCombo.Length - currentPos - 1;

        if (index < 0 || index >= currentCombo.Length) { 
            ClearColors(); 
            return; 
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (currentCombo[index] == 'r') {
                ColorGreen();
            } else {
                FlashArrows();
            }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (currentCombo[index] == 'l') {
                ColorGreen();
            } else {
                FlashArrows();
            }
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (currentCombo[index] == 'u') {
                ColorGreen();
            } else {
                FlashArrows();
            }
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Debug.Log("INDEX : " + index + " COMBOLEN " +currentCombo.Length);
            if (currentCombo[index] == 'd') {
                ColorGreen();
            } else {
                FlashArrows();
            }
        }
    }

    public void ClearColors() {
        currentPos = 0;
        for (int i = 0; i < transform.childCount; ++i) {
            transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    private void ColorGreen() {
        transform.GetChild(currentPos).gameObject.GetComponent<Image>().color = Color.green;
        ++currentPos;
    }

    public void FlashArrows() {
        flashAnim.Play();
    }

    public void AddCombo(string combo) {
        currentCombo = combo;
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
        currentPos = 0;
    }
}
