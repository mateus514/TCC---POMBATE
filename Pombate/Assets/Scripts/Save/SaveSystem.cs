using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem _instance;
    public static SaveSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SaveSystem");
                _instance = go.AddComponent<SaveSystem>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private string savePath;
    private SaveData currentSave;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        savePath = Path.Combine(Application.persistentDataPath, "gamesave.json");
        Debug.Log($"Save Path: {savePath}");
        LoadGame();
    }

    public void SaveGame(int levelCompleted, string sceneName)
    {
        if (currentSave == null)
        {
            currentSave = new SaveData();
        }

        if (levelCompleted > currentSave.maxUnlockedLevel)
        {
            currentSave.maxUnlockedLevel = levelCompleted;
        }

        bool alreadyCompleted = currentSave.levelsCompleted.Exists(
            level => level.levelIndex == levelCompleted && level.sceneName == sceneName
        );

        if (!alreadyCompleted)
        {
            currentSave.levelsCompleted.Add(new LevelProgress(levelCompleted, sceneName));
        }

        currentSave.lastPlayedDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string json = JsonUtility.ToJson(currentSave, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Jogo salvo! Nível desbloqueado: {levelCompleted}");
        Debug.Log($"JSON salvo:\n{json}");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                currentSave = JsonUtility.FromJson<SaveData>(json);
                Debug.Log($"Save carregado! Nível máximo: {currentSave.maxUnlockedLevel}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Erro ao carregar save: {e.Message}");
                currentSave = new SaveData();
            }
        }
        else
        {
            Debug.Log("Nenhum save encontrado. Criando novo save...");
            currentSave = new SaveData();
            SaveGame(0, "Fase1");
        }
    }

    public int GetMaxUnlockedLevel()
    {
        if (currentSave == null) LoadGame();
        return currentSave.maxUnlockedLevel;
    }

    public bool IsLevelCompleted(int levelIndex)
    {
        if (currentSave == null) LoadGame();
        return currentSave.levelsCompleted.Exists(level => level.levelIndex == levelIndex);
    }

    public void ResetProgress()
    {
        currentSave = new SaveData();
        SaveGame(0, "Fase1");
        Debug.Log("Progresso resetado!");
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            currentSave = new SaveData();
            Debug.Log("Save deletado!");
        }
    }
}