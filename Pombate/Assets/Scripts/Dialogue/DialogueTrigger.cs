using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool jaAtivado = false; // impede repetir

    public void TriggerDialogue(GameObject player)
    {
        // inicia diálogo
        DialogueManager.Instance.StartDialogue(dialogue);

        // trava movimento do player
        player.GetComponent<Player>().BloquearMovimento();

        // destrói o trigger para nunca mais ativar
        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (jaAtivado) return; // evita multitrigger

        if (collision.CompareTag("Player"))
        {
            jaAtivado = true;
            TriggerDialogue(collision.gameObject);
        }
    }
}
