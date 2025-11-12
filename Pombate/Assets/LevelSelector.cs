using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro; // Para TextMeshPro

public class LevelSelector : MonoBehaviour
{
    [Header("Level Configuration")]
    public List<LevelButton> levelButtons = new List<LevelButton>();
    
    [Header("Visual Settings")]
    public Color normalTextColor = new Color(0.3f, 0.3f, 0.8f);
    public Color selectedTextColor = new Color(0.5f, 1f, 1f);
    public Color lockedTextColor = new Color(0.2f, 0.2f, 0.3f);
    public float outlineWidth = 4f;
    
    [Header("Navigation")]
    public int columns = 4;
    
    private int currentIndex = 0;
    private int maxUnlockedLevel = 0;
    
    void Start()
    {
        maxUnlockedLevel = PlayerPrefs.GetInt("MaxUnlockedLevel", 0);
        
        SetupLevels();
        UpdateSelection(0);
    }
    
    void Update()
    {
        HandleInput();
    }
    
    void SetupLevels()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            int index = i;
            LevelButton btn = levelButtons[i];
            
            // Configura se está desbloqueada
            btn.isUnlocked = i <= maxUnlockedLevel;
            
            // Configura a imagem do botão com alpha 0
            Image img = btn.button.GetComponent<Image>();
            if (img != null)
            {
                Color imgColor = img.color;
                imgColor.a = 0f; // Alpha zero na imagem de fundo
                img.color = imgColor;
            }
            
            // Configura o Outline
            Outline outline = btn.button.GetComponent<Outline>();
            if (outline == null)
            {
                outline = btn.button.gameObject.AddComponent<Outline>();
            }
            
            outline.effectDistance = new Vector2(outlineWidth, outlineWidth);
            outline.enabled = false;
            
            // Configura a cor do texto do número
            Text numberText = btn.button.GetComponentInChildren<Text>();
            TextMeshProUGUI numberTMP = btn.button.GetComponentInChildren<TextMeshProUGUI>();
            
            Color textColor = btn.isUnlocked ? normalTextColor : lockedTextColor;
            
            if (numberText != null)
            {
                numberText.color = textColor;
            }
            if (numberTMP != null)
            {
                numberTMP.color = textColor;
            }
            
            // Adiciona listener de clique
            btn.button.onClick.RemoveAllListeners();
            btn.button.onClick.AddListener(() => OnLevelClick(index));
        }
    }
    
    void HandleInput()
    {
        int newIndex = currentIndex;
        
        // Navegação horizontal
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            newIndex = Mathf.Min(currentIndex + 1, levelButtons.Count - 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            newIndex = Mathf.Max(currentIndex - 1, 0);
        }
        
        // Navegação vertical
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            newIndex = Mathf.Max(currentIndex - columns, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            newIndex = Mathf.Min(currentIndex + columns, levelButtons.Count - 1);
        }
        
        // Confirmar seleção
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            OnLevelClick(currentIndex);
            return;
        }
        
        if (newIndex != currentIndex)
        {
            UpdateSelection(newIndex);
        }
    }
    
    void UpdateSelection(int newIndex)
    {
        // Remove outline e cor de seleção do botão anterior
        if (currentIndex >= 0 && currentIndex < levelButtons.Count)
        {
            LevelButton oldBtn = levelButtons[currentIndex];
            
            Outline oldOutline = oldBtn.button.GetComponent<Outline>();
            if (oldOutline != null)
            {
                oldOutline.enabled = false;
            }
            
            // Restaura cor normal do texto
            Text oldText = oldBtn.button.GetComponentInChildren<Text>();
            TextMeshProUGUI oldTMP = oldBtn.button.GetComponentInChildren<TextMeshProUGUI>();
            
            Color normalColor = oldBtn.isUnlocked ? normalTextColor : lockedTextColor;
            
            if (oldText != null)
            {
                oldText.color = normalColor;
            }
            if (oldTMP != null)
            {
                oldTMP.color = normalColor;
            }
        }
        
        currentIndex = newIndex;
        
        // Adiciona outline e cor de seleção no novo botão
        if (currentIndex >= 0 && currentIndex < levelButtons.Count)
        {
            LevelButton newBtn = levelButtons[currentIndex];
            
            Outline newOutline = newBtn.button.GetComponent<Outline>();
            if (newOutline != null)
            {
                newOutline.enabled = true;
                newOutline.effectColor = selectedTextColor;
            }
            
            // Aplica cor de seleção no texto
            Text newText = newBtn.button.GetComponentInChildren<Text>();
            TextMeshProUGUI newTMP = newBtn.button.GetComponentInChildren<TextMeshProUGUI>();
            
            if (newText != null)
            {
                newText.color = selectedTextColor;
            }
            if (newTMP != null)
            {
                newTMP.color = selectedTextColor;
            }
        }
    }
    
    void OnLevelClick(int index)
    {
        LevelButton btn = levelButtons[index];
        
        if (!btn.isUnlocked)
        {
            // Feedback visual/sonoro de fase bloqueada
            Debug.Log("Fase bloqueada!");
            // Adicione aqui: som de erro, animação de shake, etc
            return;
        }
        
        // Carrega a cena
        if (btn.isCutscene)
        {
            Debug.Log($"Carregando cutscene: {btn.sceneName}");
        }
        else
        {
            Debug.Log($"Carregando fase: {btn.sceneName}");
        }
        
        SceneManager.LoadScene(btn.sceneName);
    }
    
    // Método para desbloquear próxima fase (chamar ao completar uma fase)
    public void UnlockNextLevel()
    {
        maxUnlockedLevel++;
        PlayerPrefs.SetInt("MaxUnlockedLevel", maxUnlockedLevel);
        PlayerPrefs.Save();
        SetupLevels();
        UpdateSelection(currentIndex); // Atualiza a seleção atual
    }
}

[System.Serializable]
public class LevelButton
{
    public Button button;
    public string sceneName;
    public bool isCutscene = false;
    [HideInInspector] public bool isUnlocked = false;
}