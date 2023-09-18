using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseShiftUI : MonoBehaviour
{
    public Image phaseShiftImage;
    // Start is called before the first frame update
    void Start()
    {
        phaseShiftImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhaseShift.isCooldown == true)
        {
            CoolDownPhaseShift();
        }
    }

    public void CoolDownPhaseShift()
    {
        phaseShiftImage.fillAmount = 1 - PhaseShift.coolDownRemaining / PhaseShift.phaseShiftCoolDown;
    }
}
