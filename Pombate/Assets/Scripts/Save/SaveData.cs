using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int maxUnlockedLevel;
    public List<LevelProgress> levelsCompleted;
    public string lastPlayedDate;
    
    public SaveData()
    {
        maxUnlockedLevel = 0;
        levelsCompleted = new List<LevelProgress>();
        lastPlayedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

[System.Serializable]
public class LevelProgress
{
    public int levelIndex;
    public string sceneName;
    public bool completed;
    public string completedDate;
    
    public LevelProgress(int index, string scene)
    {
        levelIndex = index;
        sceneName = scene;
        completed = true;
        completedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}