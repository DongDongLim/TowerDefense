using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;

[CustomPropertyDrawer(typeof(AddressKeyDropDownAttribute))]
public class AddressKeyDropDownEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings is null)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        var allKeys = (from @group in settings.groups
            from entry in @group.entries
            select entry.address).ToList();

        var keyIndex = allKeys.IndexOf(property.stringValue);
        keyIndex = EditorGUI.Popup(position, label.text, keyIndex, allKeys.ToArray());
        if (keyIndex >= 0 && keyIndex < allKeys.Count)
        {
            property.stringValue = allKeys[keyIndex];
        }
    }
}