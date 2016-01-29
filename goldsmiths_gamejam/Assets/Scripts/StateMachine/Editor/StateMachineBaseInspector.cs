using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(StateMachineBase), true)]
public class StateMachineBaseInspector : Editor
{
    [SerializeField]

    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {       
        StateMachineBase stateMachine = (StateMachineBase)target;

        EditorGUILayout.LabelField("Previous State: " + stateMachine.lastState);
        EditorGUILayout.LabelField("Current State: " + stateMachine.currentState);
        EditorGUILayout.Separator();

        DrawDefaultInspector();

        if (stateMachine.lastState != stateMachine.currentState)
        {
            Repaint();
        }
    }
}
