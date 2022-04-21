using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemType))]
public class ItemEditor : Editor
{
  public override void OnInspectorGUI()
  {
    // base.OnInspectorGUI();
    var item = target as ItemType;
    SerializedObject serializedItem = new UnityEditor.SerializedObject(item);
    Object editor_object;

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Item Name");
    serializedItem.FindProperty("item_name").stringValue = EditorGUILayout.TextField(item.item_name);
    EditorGUILayout.EndHorizontal();

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Item Tag");
    serializedItem.FindProperty("item_tag").stringValue = EditorGUILayout.TagField(item.item_tag);
    EditorGUILayout.EndHorizontal();

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Item Model");
    editor_object = EditorGUILayout.ObjectField(item.item_model, typeof(GameObject), false);
    serializedItem.FindProperty("item_model").objectReferenceValue = editor_object as GameObject;
    EditorGUILayout.EndHorizontal();

    if (item.item_model != null)
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Model Scale");
      serializedItem.FindProperty("scale").vector3Value = EditorGUILayout.Vector3Field("", item.scale);
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Model Offset");
      serializedItem.FindProperty("spawnOffset").vector3Value = EditorGUILayout.Vector3Field("", item.spawnOffset);
      EditorGUILayout.EndHorizontal();
    }

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Rotates");
    serializedItem.FindProperty("rotates").boolValue = EditorGUILayout.Toggle(item.rotates);
    EditorGUILayout.EndHorizontal();

    if (item.rotates)
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.Space();
      EditorGUILayout.LabelField("X Rotation");
      serializedItem.FindProperty("x").boolValue = EditorGUILayout.Toggle(item.x);
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Y Rotation");
      serializedItem.FindProperty("y").boolValue = EditorGUILayout.Toggle(item.y);
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Z Rotation");
      serializedItem.FindProperty("z").boolValue = EditorGUILayout.Toggle(item.z);
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
    } else {
      serializedItem.FindProperty("x").boolValue = false;
      serializedItem.FindProperty("y").boolValue = false;
      serializedItem.FindProperty("z").boolValue = false;
    }

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("isPickUp");
    serializedItem.FindProperty("pickup").boolValue = EditorGUILayout.Toggle(item.pickup);
    EditorGUILayout.EndHorizontal();
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("isInteractable");
    serializedItem.FindProperty("interactable").boolValue = EditorGUILayout.Toggle(item.interactable);
    EditorGUILayout.EndHorizontal();

    serializedItem.ApplyModifiedProperties();
  }
}