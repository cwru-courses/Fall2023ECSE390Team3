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
    void LateUpdate()
    {
        float cdPercentage = PhaseShift.Instance.GetCDPercentage();
        if (cdPercentage > 0)
        {
            phaseShiftImage.fillAmount = 1 - cdPercentage;
        }
        else
        {
            phaseShiftImage.fillAmount = 0;
        }
    }
}
