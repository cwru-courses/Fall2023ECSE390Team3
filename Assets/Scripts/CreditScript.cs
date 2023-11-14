using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour
{
    [SerializeField] private GameObject titlePage;
    [SerializeField] private GameObject creditsPage;

    public void OnAnimationFinished()
    {
        titlePage.SetActive(true);
        creditsPage.SetActive(false);
    }
}
