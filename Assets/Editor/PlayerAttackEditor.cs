using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerAttack))]
public class PlayerAttackEditor : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.red;

        PlayerAttack paTarget = (PlayerAttack)target;

        Vector3 lookAtDir = paTarget.GetLookAtDir();
        float attackAngleH = paTarget.GetAttackAngleH();
        float attackRadius = paTarget.GetAttackRad();
        Handles.DrawSolidArc(
            paTarget.transform.position,
            paTarget.transform.forward,
            lookAtDir,
            attackAngleH,
            attackRadius
        );
        Handles.DrawSolidArc(
            paTarget.transform.position,
            paTarget.transform.forward,
            lookAtDir,
           -attackAngleH,
            attackRadius
        );
    }
}
