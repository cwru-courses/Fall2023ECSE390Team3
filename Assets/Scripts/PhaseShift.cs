using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseShift : MonoBehaviour
{
    public Camera[] cameras;
    public KeyCode switchKey = KeyCode.Q;
    private GameObject player;
    public static float phaseShiftCoolDown = 2;
    public static float coolDownRemaining = 2;
    public static bool isCooldown = false;
    public Slider phaseShiftSlider;
    public float phaseShiftPrecasting = 1;
    private float precastingLeft;

    private int currentCameraIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        phaseShiftSlider.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If player pressed key Q and phase shift is not in cooling down
        if (Input.GetKeyDown(switchKey) && (isCooldown == false) && (precastingLeft <= 0))
        {
            StartCoroutine(beginPhaseShift());
        }
        else
        {
            if (coolDownRemaining > 0)
            {
                coolDownRemaining -= Time.deltaTime;
            }
            else if (coolDownRemaining <= 0)
            {
                isCooldown = false;
            }
        }
    }

    IEnumerator beginPhaseShift()
    {
        // Do precasting first
        yield return StartCoroutine(phaseShiftPrecast());
        // Start cooling down
        isCooldown = true;
        coolDownRemaining = phaseShiftCoolDown;
        // Move character into the alternate world
        Vector3 currentLocation = player.transform.position;
        player.transform.position = new Vector3(currentLocation.x, currentLocation.y * -1, currentLocation.z);

        // Go to alternate camera
        cameras[currentCameraIndex].enabled = false;
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
        cameras[currentCameraIndex].enabled = true;
    }

    IEnumerator phaseShiftPrecast()
    {
        phaseShiftSlider.gameObject.SetActive(true);
        precastingLeft = phaseShiftPrecasting;
        while (precastingLeft > 0)
        {
            precastingLeft -= Time.deltaTime;
            phaseShiftSlider.value = precastingLeft / phaseShiftPrecasting;
            yield return null;
        }
        phaseShiftSlider.gameObject.SetActive(false);
    }
}
