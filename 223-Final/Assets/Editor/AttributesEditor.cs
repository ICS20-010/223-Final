using UnityEditor;

[CustomEditor(typeof(Attributes))]
public class AttributesEditor : Editor
{
  public override void OnInspectorGUI()
  {
    var attributes = target as Attributes;

    SerializedObject serializedAttr = new UnityEditor.SerializedObject(attributes);

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Total Health");
    serializedAttr.FindProperty("healthTotal").intValue = EditorGUILayout.IntField(attributes.healthTotal);
    serializedAttr.FindProperty("health").intValue = serializedAttr.FindProperty("healthTotal").intValue;
    EditorGUILayout.EndHorizontal();

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Total Stamina");
    serializedAttr.FindProperty("staminaTotal").intValue = EditorGUILayout.IntField(attributes.staminaTotal);
    serializedAttr.FindProperty("stamina").intValue = serializedAttr.FindProperty("staminaTotal").intValue;
    EditorGUILayout.EndHorizontal();

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Stamina Regen Amount");
    serializedAttr.FindProperty("regenAmount").intValue = EditorGUILayout.IntField(attributes.regenAmount);
    EditorGUILayout.EndHorizontal();

    serializedAttr.ApplyModifiedProperties();
  }
}