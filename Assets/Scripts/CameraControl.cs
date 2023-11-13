using System.ComponentModel.Design.Serialization;
using UnityEngine;

public enum CamBehavior
{
    PlayerFocus = 0, ObjectFocus, BossRoom
}

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    [SerializeField] private CamBehavior camMode;

    [Header("Playe Focus Mode")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerFocusFOV;
    [Header("Object Focus Mode")]
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float objectFocusFOV;
    [Header("Boos Room Mode")]
    [SerializeField] public static Vector3 roomCenterPosition;
    [SerializeField] private float bossRoomFOV;
    [SerializeField] private Vector2 playerOffsetMultiplier;

    private Camera cam;
    private float zOffset;

    void Awake()
    {
        cam = GetComponent<Camera>();
        zOffset = transform.position.z;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos;
        switch (camMode)
        {
            case CamBehavior.PlayerFocus:
                targetPos = playerTransform.position;

                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, playerFocusFOV, 0.3f);
                break;

            case CamBehavior.ObjectFocus:
                targetPos = targetTransform.position;

                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, objectFocusFOV, 0.3f);
                break;

            case CamBehavior.BossRoom:
                targetPos = targetTransform.position - roomCenterPosition;
                targetPos.x *= playerOffsetMultiplier.x;
                targetPos.y *= playerOffsetMultiplier.y;
                targetPos += roomCenterPosition;

                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, bossRoomFOV, 0.3f);
                break;

            default:
                targetPos = targetTransform.position;
                break;
        }
        targetPos.z = zOffset;
        transform.position = targetPos;
    }

    public void SwitchToPlayerFocus()
    {
        camMode = CamBehavior.PlayerFocus;
    }

    public void SwitchToObjectFocus(Vector3 targetObjPosition)
    {
        camMode = CamBehavior.ObjectFocus;
        targetTransform.position = targetObjPosition;
    }

    public void SwitchToBossRoom(Vector3 roomCenterPos)
    {
        camMode = CamBehavior.BossRoom;
        roomCenterPosition = roomCenterPos;
    }

    public static void SwitchSide()
    {
        roomCenterPosition = new Vector3(roomCenterPosition.x,-1.0f*roomCenterPosition.y,roomCenterPosition.z);
    }
}
