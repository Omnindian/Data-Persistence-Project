using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string PlayerName;
    public int PlayerScore;
    public bool MusicOn, SoundEffectsOn, GameRunning;
    private HighScoreHandler highScoreHandler;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadPlayerInfo();
    }

    [System.Serializable]
    class PlayerData
    {
        public string PlayerName;
        public int PlayerScore;
        public bool MusicOn, SoundEffectsOn;
    }

    void Update()
    {
        PlayerScore = MainManager.m_Points;
    }


    public void SavePlayerInfo()
    {
        PlayerData data = new PlayerData();
        data.PlayerName = PlayerName;
        data.PlayerScore = PlayerScore;
        data.MusicOn = MusicOn;
        data.SoundEffectsOn = SoundEffectsOn;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log(Application.persistentDataPath);
    }

    public void LoadPlayerInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            PlayerName = data.PlayerName;
            PlayerScore = data.PlayerScore;
            MusicOn = data.MusicOn;
            SoundEffectsOn = data.SoundEffectsOn;
        }
    }

    public void OnEndGame()
    {
        ///////// Problem code here!!! //////////
        //highScoreHandler.AddHighScoreIfPossible(new HighScoreElement(PlayerName, PlayerScore));
        PlayerScore = MainManager.m_Points;
        Debug.Log("End Score is " + PlayerScore + " for " + PlayerName);
    }

        public void Exit()
    {
        SavePlayerInfo();
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }


}