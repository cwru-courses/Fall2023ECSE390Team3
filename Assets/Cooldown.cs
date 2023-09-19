using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Cooldown : MonoBehaviour
{
    public float coolDown;
    public float coolDownTimer; 

    public Slider slider; 
    public Gradient gradient; 
    public Image fill; 

    void Start() {
        StartCooldown(); 
    }

    void StartCooldown() {
        coolDown = 3; 
        slider.maxValue = coolDown; 
        fill.color = gradient.Evaluate(1f); 
    }

    void Update()
    {
        if(coolDownTimer > 0) {
            coolDownTimer -= Time.deltaTime; 
        } else {
            //if time goes below zero, reset to 0 
            coolDownTimer = 0;
        }

        slider.value = coolDownTimer; 
        fill.color = gradient.Evaluate(slider.normalizedValue); 

        //for now, press "r" to restart timer
        if(Input.GetKeyDown("r") && coolDownTimer == 0) {
            Debug.Log("Restarting timer"); 
            StartCooldown(); 
            coolDownTimer = coolDown; 
        }
        
    }
}
