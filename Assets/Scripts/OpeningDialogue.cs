using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OpeningDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public String[] lines;
    public float textSpeed;
    private int index;
    private DefaultInputAction playerInputAction;


    [SerializeField] protected GameObject health_arrow;
    [SerializeField] protected GameObject yarn_arrow;
    [SerializeField] protected GameObject shift_arrow;



    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
        
    }

    // Update is called once per frame
    void Update()
    {
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

    void StartDialogue(){
        index = 0;
        StartCoroutine(TypeLine());
    
    }

    IEnumerator TypeLine(){
        if(index == 3){
            health_arrow.SetActive(true);
        }
        if(index == 4){
            health_arrow.SetActive(false);
            yarn_arrow.SetActive(true);
        }
        else if(index == 5){
            yarn_arrow.SetActive(false);
            shift_arrow.SetActive(true);
        }
        else if(index > 5){
            shift_arrow.SetActive(false);
        }
        foreach(char c in lines[index].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine(){
        if(index < lines.Length - 1){
            index ++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else{
            gameObject.SetActive(false);
        }
    }
}
