using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class OpeningDialogue : CollisionDialogue
{
    [SerializeField] protected GameObject health_arrow;
    [SerializeField] protected GameObject yarn_arrow;
    [SerializeField] protected GameObject shift_arrow;

    public override IEnumerator TypeLine(){
        if(base.index == 6){
            health_arrow.SetActive(true);
        }
        else if(base.index == 7){
            health_arrow.SetActive(false);
            yarn_arrow.SetActive(true);
        }
        else if(base.index == 8){
            yarn_arrow.SetActive(false);
            shift_arrow.SetActive(true);
        }
        else if(base.index > 8){
            shift_arrow.SetActive(false);
        }
        foreach(char c in base.lines[base.index].ToCharArray()){
            base.textComponent.text += c;
            if(c != ' '){
                base.audioSource.PlayOneShot(dialogueTypingSoundClip);
            }
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}