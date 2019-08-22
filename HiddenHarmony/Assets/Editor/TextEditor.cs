using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextBox))]
public class TextEditor : Editor
{
    TextBox textBox;

    public List<string> textBlocks;
    public override void OnInspectorGUI(){
        textBox = (TextBox) target;
        base.OnInspectorGUI();

        EditorUtility.SetDirty(target);
        if(textBox.textBlocks.Count < 1){
            textBox.AddText("(Block " + (textBox.textBlocks.Count + 1) + ")\nEnter Text Here:");
        }

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Add Text Block")){
            textBox.AddText("(Block " + (textBox.textBlocks.Count + 1) + ")\nEnter Text Here:");
        }
        if(GUILayout.Button("Remove Text Block")){
            textBox.RemoveText();
        }
        GUILayout.EndHorizontal();

        for(int i = 0; i < textBox.textBlocks.Count; i++){
            textBox.textBlocks[i] = GUILayout.TextArea(textBox.textBlocks[i]);
        }

    }
}
