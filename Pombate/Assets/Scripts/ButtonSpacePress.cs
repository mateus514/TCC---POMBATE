using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ButtonSpacePress : MonoBehaviour
{
    private Button button;
    private Text buttonText;
    private TextMeshProUGUI buttonTextTMP;
    private Color originalColor;
    
    [Header("Configurações de Brilho")]
    [SerializeField] private Color brightColor = new Color(0.5f, 0.8f, 1f, 1f); // Azul claro
    [SerializeField] private float brightDuration = 0.2f;

    void Start()
    {
        // Pega o componente Button do próprio GameObject
        button = GetComponent<Button>();
        
        if (button == null)
        {
            Debug.LogError("Este script precisa estar em um GameObject com componente Button!");
            return;
        }
        
        // Tenta pegar o texto (suporta Text padrão e TextMeshPro)
        buttonText = GetComponentInChildren<Text>();
        buttonTextTMP = GetComponentInChildren<TextMeshProUGUI>();
        
        // Salva a cor original do texto
        if (buttonTextTMP != null)
        {
            originalColor = buttonTextTMP.color;
        }
        else if (buttonText != null)
        {
            originalColor = buttonText.color;
        }
    }

    void Update()
    {
        // Verifica se a tecla Espaço foi pressionada
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Aciona o botão se ele estiver ativo e interagível
            if (button != null && button.interactable)
            {
                button.onClick.Invoke();
                StartCoroutine(BrilharTexto());
            }
        }
    }
    
    private IEnumerator BrilharTexto()
    {
        // Faz o texto brilhar em azul claro
        if (buttonTextTMP != null)
        {
            buttonTextTMP.color = brightColor;
            yield return new WaitForSeconds(brightDuration);
            buttonTextTMP.color = originalColor;
        }
        else if (buttonText != null)
        {
            buttonText.color = brightColor;
            yield return new WaitForSeconds(brightDuration);
            buttonText.color = originalColor;
        }
    }
}