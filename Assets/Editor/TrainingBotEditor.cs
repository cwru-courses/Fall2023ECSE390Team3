using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TrainingBotController))]
public class TrainingBotEditor : Editor
{
    void OnSceneGUI()
    {
        TrainingBotController tbTarget = (TrainingBotController)target;

        Handles.color = Color.red;
        List<Vector3> patrolPoints = tbTarget.GetPatrolPoints();
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            patrolPoints[i] = Handles.PositionHandle(patrolPoints[i], Quaternion.identity);
        }
        tbTarget.SetPatrolPoints(patrolPoints);
    }
}
