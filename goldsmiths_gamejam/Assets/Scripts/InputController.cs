using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputController : MonoBehaviour {

    private StringBuffer inputBuffer;

    void Start() {
        inputBuffer = new StringBuffer();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            inputBuffer.AddKey('r');
            inputBuffer.PrintBuffer();
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            inputBuffer.AddKey('l');
            inputBuffer.PrintBuffer();
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            inputBuffer.AddKey('u');
            inputBuffer.PrintBuffer();
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            inputBuffer.AddKey('d');
            inputBuffer.PrintBuffer();
        }

        if (inputBuffer.CheckSequence("uuu", 3.0f)) {
            Debug.Log("***** FOUND SEQUENCE *****");
        }

    }

    private class StringBuffer {

        private KeyTime[] buffer;

        public StringBuffer() {
            buffer = new KeyTime[10];
            for (uint i = 0; i < 10; ++i) {
                KeyTime kt = new KeyTime('-', 0.0f);
                buffer[i] = kt;
            }
        }

        public void ClearBuffer() {
            for (uint i = 0; i < 10; ++i) {
                buffer[i].SetCharacter('-');
                buffer[i].SetTime(0.0f);
            }
        }

        public void AddKey(char c) {
            for (uint i = 9; i > 0; --i) {
                buffer[i].SetCharacter(buffer[i-1].GetCharacter());
                buffer[i].SetTime(buffer[i-1].GetTime());
            }
            buffer[0].SetCharacter(c);
            buffer[0].SetTime(Time.time);
        }

        public bool CheckSequence(string combo, float time) {

            if (combo.Length > 10) {
                Debug.Log("String: " + combo + " is too long!");
                return false;
            }

            for (int i = combo.Length - 1; i >= 0; --i) {
                if (buffer[i].GetCharacter() != combo[i]) {
                    return false;
                }
            }

            if (buffer[0].GetTime() - buffer[combo.Length - 1].GetTime() > time) {
                return false;
            }

            ClearBuffer();
            return true;
        }

        public void PrintBuffer() {
            string chars = "";
            for (int i = 9; i >= 0; --i) {
                chars += buffer[i].GetCharacter();
            }
            Debug.Log("Buffer Chars: " + chars);
        }

    }

    private class KeyTime {
        private char character;
        private float time;

        public KeyTime(char character, float time) {
            this.character = character;
            this.time = time;
        }

        public char GetCharacter() { return character; }
        public void SetCharacter(char c) { this.character = c; }
        public float GetTime() { return time; }
        public void SetTime(float t) { this.time = t; }
    }
}
