using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CollisionDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public String[] lines;
    protected float textSpeed;
    protected int index;
    private DefaultInputAction playerInputAction;


    public bool isRunning = false;

    private GameObject dialogueBox;

    // Start is called before the first frame update
    public void StartRunning(GameObject inputDialogueBox)
    {
        this.dialogueBox = inputDialogueBox;
        isRunning = true;
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning){
            if(Input.GetKeyDown(KeyCode.Space)){
                if(textComponent.text == lines[index]){
                    NextLine();
                }
                else{
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }

    }

    void StartDialogue(){
        
        index = 0;
        PlayerMovement._instance.OnPause(true);
        PlayerAttack._instance.OnPause(true);
        PhaseShift._instance.OnPause(true);
        PlayerStats._instance.OnPause(true);
        StartCoroutine(TypeLine());
    
    }

    public virtual IEnumerator TypeLine(){
        foreach(char c in lines[index].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void endDialogue(){
        isRunning=false;
        dialogueBox.SetActive(false);
        PlayerMovement._instance.OnPause(false);
        PlayerAttack._instance.OnPause(false);
        PhaseShift._instance.OnPause(false);
        PlayerStats._instance.OnPause(false);
    }

    void NextLine(){
        if(index < lines.Length - 1){
            index ++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else{
            endDialogue();
        }
    }
}
