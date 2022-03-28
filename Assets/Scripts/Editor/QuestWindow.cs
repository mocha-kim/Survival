using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestWindow : EditorWindow
{
    //private string[] options;
    //private int index = 0;

    [MenuItem("Window/Quest Creator")]
    static void Init()
    {
        QuestWindow window = (QuestWindow)EditorWindow.GetWindow(typeof(QuestWindow));
        window.Show();
    }

    void OnGUI()
    {
        // The actual window code goes here
    }
}