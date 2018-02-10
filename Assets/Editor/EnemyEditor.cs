using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Enemy enemy = (Enemy) target;

        GUILayout.Label("Word");
        enemy.word = EditorGUILayout.TextField(enemy.word);
        enemy.wordText.text = enemy.word;
    }
}
