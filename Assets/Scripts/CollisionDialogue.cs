using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class CollisionDialogue : MonoBehaviour
{
    [SerializeField] public AudioClip dialogueTypingSoundClip;

    [SerializeField] public AudioSource audioSource;
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
        dialogueBox = inputDialogueBox;
        isRunning = true;
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (textComponent.text == lines[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }

    }

    void StartDialogue()
    {
        TimeManager._instance.OnDialog(true);
        index = 0;
        StartCoroutine(TypeLine());

    }

    public virtual IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            if(c != ' '){
                audioSource.PlayOneShot(dialogueTypingSoundClip);
            }
            
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    void endDialogue()
    {
        isRunning = false;
        dialogueBox.SetActive(false);
        TimeManager._instance.OnDialog(false);
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            endDialogue();
        }
    }
}
