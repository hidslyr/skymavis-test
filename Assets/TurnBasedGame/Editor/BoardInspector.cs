using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Board), true)]
public class BoardInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("________________", EditorStyles.largeLabel);

        Board board = (Board)target;

        if (GUILayout.Button("Regenerate board"))
        {
            board.RegenerateBoard();
        }
    }
}
