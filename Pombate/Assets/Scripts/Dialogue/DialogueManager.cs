using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    public CanvasGroup dialogueCanvasGroup; 

    private Queue<DialogueLine> lines;
    public bool isDialogueActive = false;
    public float typingSpeed = 0.02f;
    public float fadeSpeed = 5f;

    private Coroutine typingCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
        dialogueCanvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Return))
        {
            // Se ainda está digitando, termina instantâneo
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                dialogueArea.text = lines.Peek().line; // mostra a linha completa
                typingCoroutine = null;
                return;
            }

            DisplayNextDialogueLine();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

        dialogueArea.text = ""; 
        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            StartCoroutine(FadeOut());
            return;
        }

        DialogueLine currentLine = lines.Dequeue();
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null; // terminou de digitar
    }

    IEnumerator FadeIn()
    {
        while (dialogueCanvasGroup.alpha < 1f)
        {
            dialogueCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        dialogueCanvasGroup.alpha = 1f;
        DisplayNextDialogueLine();
    }

    IEnumerator FadeOut()
    {
        while (dialogueCanvasGroup.alpha > 0f)
        {
            dialogueCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        dialogueCanvasGroup.alpha = 0f;
        isDialogueActive = false;

        // 🔹 destrói o objeto quando o diálogo terminar
        Destroy(gameObject);
    }
}
