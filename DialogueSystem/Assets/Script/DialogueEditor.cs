using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {

        Dialogue selectedDialogue = null;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if(dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChange;
        }

        private void OnSelectionChange()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if(newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }


        private void OnGUI()
        {
            if(selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected");
            }
            else
            {
                foreach(DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    OnGUINode(node);

                }

            }
           
            
        }

        private void OnGUINode(DialogueNode node)
        {

            GUILayout.BeginArea(node.position);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node:");

            string newText = EditorGUILayout.TextField(node.text);
            string newUniqueID = EditorGUILayout.TextField(node.uniqueID);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                node.text = newText;
                node.uniqueID = newUniqueID;

            }
        }
    }

}

