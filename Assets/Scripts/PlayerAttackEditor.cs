using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerAttack))]
public class PlayerAttackEditor : Editor
{
    void OnSceneGUI()
    {
        //Handles.color = Color.red;

        //PlayerAttack paTarget = (PlayerAttack)target;

        //Handles.DrawSolidArc(
        //    paTarget.transform.position,
        //    paTarget.transform.forward,
        //    paTarget.GetLookAtDir(),
        //    paTarget.GetAttackAngleH(),
        //    paTarget.GetAttackRad()
        //);
        //Handles.DrawSolidArc(
        //    paTarget.transform.position,
        //    paTarget.transform.forward,
        //    paTarget.GetLookAtDir(),
        //   -paTarget.GetAttackAngleH(),
        //    paTarget.GetAttackRad()
        //);
    }
}
