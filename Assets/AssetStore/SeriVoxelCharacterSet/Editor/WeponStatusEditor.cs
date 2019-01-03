using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeponStatus))]
public class WeponStatusEditor : Editor {

	private static readonly string[] DisplayedType = {"None","One hand wepon","Two hand wepon","Dual wepon","Pole wepon","Bow","Magic wepon"};
	private static readonly int[] OptionType       = { 0, 1, 2, 3, 4, 5, 6};
	public override void OnInspectorGUI()
	{
		WeponStatus Wepon_Status = target as WeponStatus;

		Wepon_Status.WeponType = EditorGUILayout.IntPopup ("Type",Wepon_Status.WeponType,DisplayedType,OptionType);
	}
}
