using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

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
        // CARREGA O SAVE
        maxUnlockedLevel = SaveSystem.Instance.GetMaxUnlockedLevel();
        Debug.Log($"Nível máximo desbloqueado: {maxUnlockedLevel}");
        
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
            
            btn.isUnlocked = i <= maxUnlockedLevel;
            
            Image img = btn.button.GetComponent<Image>();
            if (img != null)
            {
                Color imgColor = img.color;
                imgColor.a = 0f;
                img.color = imgColor;
            }
            
            Outline outline = btn.button.GetComponent<Outline>();
            if (outline == null)
            {
                outline = btn.button.gameObject.AddComponent<Outline>();
            }
            
            outline.effectDistance = new Vector2(outlineWidth, outlineWidth);
            outline.enabled = false;
            
            Text numberText = btn.button.GetComponentInChildren<Text>();
            TextMeshProUGUI numberTMP = btn.button.GetComponentInChildren<TextMeshProUGUI>();
            
            Color textColor = btn.isUnlocked ? normalTextColor : lockedTextColor;
            
            if (numberText != null) numberText.color = textColor;
            if (numberTMP != null) numberTMP.color = textColor;
            
            btn.button.onClick.RemoveAllListeners();
            btn.button.onClick.AddListener(() => OnLevelClick(index));
        }
    }
    
    void HandleInput()
    {
        int newIndex = currentIndex;
        
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            newIndex = Mathf.Min(currentIndex + 1, levelButtons.Count - 1);
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            newIndex = Mathf.Max(currentIndex - 1, 0);
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            newIndex = Mathf.Max(currentIndex - columns, 0);
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            newIndex = Mathf.Min(currentIndex + columns, levelButtons.Count - 1);
        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            OnLevelClick(currentIndex);
            return;
        }
        
        if (newIndex != currentIndex)
            UpdateSelection(newIndex);
    }
    
    void UpdateSelection(int newIndex)
    {
        if (currentIndex >= 0 && currentIndex < levelButtons.Count)
        {
            LevelButton oldBtn = levelButtons[currentIndex];
            Outline oldOutline = oldBtn.button.GetComponent<Outline>();
            if (oldOutline != null) oldOutline.enabled = false;
            
            Text oldText = oldBtn.button.GetComponentInChildren<Text>();
            TextMeshProUGUI oldTMP = oldBtn.button.GetComponentInChildren<TextMeshProUGUI>();
            Color normalColor = oldBtn.isUnlocked ? normalTextColor : lockedTextColor;
            
            if (oldText != null) oldText.color = normalColor;
            if (oldTMP != null) oldTMP.color = normalColor;
        }
        
        currentIndex = newIndex;
        
        if (currentIndex >= 0 && currentIndex < levelButtons.Count)
        {
            LevelButton newBtn = levelButtons[currentIndex];
            Outline newOutline = newBtn.button.GetComponent<Outline>();
            if (newOutline != null)
            {
                newOutline.enabled = true;
                newOutline.effectColor = selectedTextColor;
            }
            
            Text newText = newBtn.button.GetComponentInChildren<Text>();
            TextMeshProUGUI newTMP = newBtn.button.GetComponentInChildren<TextMeshProUGUI>();
            
            if (newText != null) newText.color = selectedTextColor;
            if (newTMP != null) newTMP.color = selectedTextColor;
        }
    }
    
    void OnLevelClick(int index)
    {
        LevelButton btn = levelButtons[index];
        
        if (!btn.isUnlocked)
        {
            Debug.Log("Fase bloqueada!");
            return;
        }
        
        SceneManager.LoadScene(btn.sceneName);
    }
    
    public void ResetAllProgress()
    {
        SaveSystem.Instance.ResetProgress();
        maxUnlockedLevel = 0;
        SetupLevels();
        UpdateSelection(0);
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