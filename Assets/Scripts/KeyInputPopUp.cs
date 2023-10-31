using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInputPopUp : MonoBehaviour
{
    public Canvas pickupIcon;
    public Canvas putdownIcon;

    private void Update()
    {
    }

    public void EnablePickupIcon(bool enable)
    {
        pickupIcon.enabled = enable;
    }

    public void EnablePutdownIcon(bool enable)
    {
        putdownIcon.enabled = enable;
    }

}
