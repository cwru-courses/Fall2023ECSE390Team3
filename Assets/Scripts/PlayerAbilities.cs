using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    public static PlayerAbilities _instance;
    private DefaultInputAction playerInputAction;

    //private PlayerStats playerStatsInstance;
    //private PlayerMovement playerMovementInstance;

    public int abilityType = 0; //0: none, 1:block, 2:stun

    [SerializeField] GameObject blockPrefab;
    private const float blockCD = 5f;
    private const float blockDuration = 3f;
    private const float blockMovementSpeedMultiplier = 0.4f;

    [SerializeField] GameObject stunMeshPrefab;
    private const float stunCD = 5f;
    private const float stunDuration = 3f;
    private const float stunMaxCastingDuration = 10f;
    private bool stunCasting = false;
    private List<Vector3> stunCulledPath;
    private List<Vector3> stunFullPath;
    private LineRenderer pathRender;

    private float lastAbilityTime;


    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        GameObject pathRenderObj = new GameObject("PathRenderer", typeof(LineRenderer));
        pathRenderObj.transform.position = Vector3.zero;
        pathRender = pathRenderObj.GetComponent<LineRenderer>();
        pathRender.startWidth = 0.1f;
        pathRender.endWidth = 0.1f;
        pathRender.material = new Material(Shader.Find("Sprites/Default"));
        pathRender.startColor = Color.magenta;
        pathRender.endColor = Color.magenta;
        

        playerInputAction = new DefaultInputAction();
        playerInputAction.Player.Ability1.started += startAbility;

        lastAbilityTime = -Mathf.Max(blockCD, stunCD);

        stunFullPath = new List<Vector3>(); // used for line renderer during cast
        pathRender.SetPositions(stunFullPath.ToArray());
        stunCulledPath = new List<Vector3>(); // used for mechanics/ functionality

    }

    void FixedUpdate()
    {
        if (stunCasting)
        {
            if (Time.time - lastAbilityTime < stunMaxCastingDuration)
            {
                //add new point
                Vector3 newPoint = transform.position;
                if (newPoint != stunCulledPath[stunCulledPath.Count - 1])
                {
                    if (stunCulledPath.Count >= 2)
                    {
                        // add to full path
                        stunFullPath.Add(newPoint);

                        //check if its inline with previous points for culled path
                        Vector3 oldDir = (stunCulledPath[stunCulledPath.Count - 1] - stunCulledPath[stunCulledPath.Count - 2]).normalized;
                        Vector3 newDir = (newPoint - stunCulledPath[stunCulledPath.Count - 1]).normalized;
                        if (oldDir != newDir && oldDir != newDir*-1f)
                        {
                            //print("adding new point");
                            stunCulledPath.Add(newPoint);
                        }
                    }
                    else
                    {
                        //print("adding new point");
                        stunFullPath.Add(newPoint);
                        stunCulledPath.Add(newPoint);
                    }
                    
                }
            }
            else
            {
                //finish stun cast
                stun();
            }
            pathRender.positionCount = stunFullPath.Count;
            pathRender.SetPositions(stunFullPath.ToArray());
        }
    }

    private void OnEnable()
    {
        playerInputAction.Player.Ability1.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.Ability1.Disable();
    }

    public void OnPause(bool paused)
    {
        if (paused)
        {
            playerInputAction.Player.Ability1.Disable();
        }
        else
        {
            playerInputAction.Player.Ability1.Enable();
        }
    }

    private void startAbility(InputAction.CallbackContext ctx)
    {
        //print("ability triggered");
        if (abilityType == 1 && Time.time - lastAbilityTime > blockCD)
        {
            lastAbilityTime = Time.time;
            StartCoroutine(block());
        }
        else if(abilityType == 2 && (Time.time- lastAbilityTime> stunCD || stunCasting))
        {
            lastAbilityTime = Time.time;
            StartCoroutine(stun());
        }
    }

    private IEnumerator block()
    {
        PlayerMovement playerMovementInstance = PlayerMovement._instance;
        PlayerStats playerStatsInstance = PlayerStats._instance;
        playerMovementInstance.MultiplySpeed(blockMovementSpeedMultiplier);
        playerStatsInstance.blocking = true;
        GameObject blockObject = Instantiate(blockPrefab) as GameObject;
        blockObject.transform.parent = this.transform;
        blockObject.transform.position = this.transform.position;
        yield return new WaitForSeconds(blockDuration);
        Destroy(blockObject);
        playerMovementInstance.MultiplySpeed(1f/blockMovementSpeedMultiplier);
        playerStatsInstance.blocking = false;
        
    }

    private IEnumerator stun()
    {
        if (stunCasting)
        {
            //print("stun cast finished");
            stunCasting = false;
            GameObject stunObject = Instantiate(stunMeshPrefab) as GameObject;
            stunObject.GetComponent<MeshGenerator>().SetVertices(stunCulledPath.ToArray());
            stunObject.transform.position = Vector3.zero;
            stunCulledPath = new List<Vector3>();
            stunFullPath = new List<Vector3>();
            pathRender.positionCount = stunFullPath.Count;
            pathRender.SetPositions(stunFullPath.ToArray());
            
            yield return new WaitForSeconds(stunDuration);
            Destroy(stunObject);

        }
        else
        {
            //print("stun cast started");
            stunFullPath.Add(transform.position);
            stunCulledPath.Add(transform.position);
            lastAbilityTime = Time.time;
            stunCasting = true;
        }
    }

}
