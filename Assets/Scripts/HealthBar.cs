using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HealthBar : MonoBehaviour
{
    //slider object attached to healthbar
    public Slider slider; 
    public Gradient gradient; 
    public Image fill;  
    public Image border; 

    public void SetMaxHealth(int health) {
        slider.maxValue = health; 
        slider.value = health; 

        fill.color = gradient.Evaluate(1f); 
    }

    public void Update(){
        if(slider.value < 50){
            fill.color = Color.Lerp(gradient.Evaluate(1f), Color.black, Mathf.PingPong(Time.time * 1.5f, 1));
        }
        else{
            fill.color = gradient.Evaluate(1f);
        }

        if(slider.value == 0 && border) {
            border.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * 2f, 1));
        }
    }

    public void SetHealth(int health) {
        // if(slider.value <= 0) {
        //     Debug.Log("Player health < 0"); 
        // }
        slider.value = health; 

        fill.color = gradient.Evaluate(slider.normalizedValue); 
    } 
}
