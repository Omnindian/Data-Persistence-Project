using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string PlayerName;
    public int PlayerScore;
    public float MusicVolume, SoundEffectVolume;
    public bool MusicOn, SoundEffectsOn, GameRunning;
    private List<HighScoreElement> highScoreEntries = new List<HighScoreElement>();
    [SerializeField]
    int highScoreMaxCount = 20; // Choose a max number of entries to save on file
    [SerializeField]
    string listSaveFileName;

    [System.Serializable]
    class PlayerData
    {
        public string PlayerName;
        public int PlayerScore;
        public float MusicVolume, SoundEffectVolume;
        public bool MusicOn, SoundEffectsOn;
    }

    
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
        data.MusicVolume = MusicVolume;
        data.SoundEffectVolume = SoundEffectVolume;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(GetPath("savefile.json"), json);
        Debug.Log(Application.persistentDataPath);
    }

    public void LoadPlayerInfo()
    {
        string path = GetPath("savefile.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            PlayerName = data.PlayerName;
            PlayerScore = data.PlayerScore;
            MusicOn = data.MusicOn;
            SoundEffectsOn = data.SoundEffectsOn;
            MusicVolume = data.MusicVolume;
            SoundEffectVolume = data.SoundEffectVolume;
        }
    }

    private static string GetPath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, filename);
    }

    public void OnEndGame()
    {
        PlayerScore = MainManager.m_Points;
        // Update High Score List
        AddHighScoreIfPossible(new HighScoreElement(PlayerName, PlayerScore));
    }


#region CODE TO DEAL WITH SAVING HIGH SCORE LIST

    public void SaveHighScore ()
    {
        // highScoreEntries.Add(new HighScoreElement("Omni", Random.Range(0, 100))); // Tester code

        SaveListToJson<HighScoreElement> (highScoreEntries, listSaveFileName);
    }

    public List<HighScoreElement> GetHighScores()
    {
        highScoreEntries = ReadListFromJson<HighScoreElement>(listSaveFileName);
        return highScoreEntries;

    }

    public HighScoreElement TopScoreInfo()
    {
        HighScoreElement topScore;

        if(File.Exists(GetPath(listSaveFileName)))
        {
            highScoreEntries = ReadListFromJson<HighScoreElement>(listSaveFileName);
            topScore = highScoreEntries[0];
            return topScore;
        }
        else
        {
            AddHighScoreIfPossible(new HighScoreElement("Nobody", 0));
            topScore = highScoreEntries[0];
            return topScore;
        }
    }

    public void AddHighScoreIfPossible(HighScoreElement highScoreElement)
    {
        if (highScoreEntries.Count == 0)
        {
            highScoreEntries.Add(highScoreElement);

            SaveHighScore();
        }
        else
        {
            for (int i = 0; i < highScoreEntries.Count; i++)
            {
                if (highScoreElement.highScore >= highScoreEntries[i].highScore)
                {
                    highScoreEntries.Insert(i, highScoreElement);

                    break;
                }
                else if (i == highScoreEntries.Count - 1)
                {
                    highScoreEntries.Add(highScoreElement);

                    break;
                }
            }

            SaveHighScore();
        }

        while (highScoreEntries.Count > highScoreMaxCount) 
        {
            // Cull extra highscores off the highscore save file
            highScoreEntries.RemoveAt(highScoreMaxCount);
        }
    }


    public static void SaveListToJson<T> (List<T> toSave, string filename)
    {
        Debug.Log(GetPath(filename));
        string content = JsonHelper.ToJson<T> (toSave.ToArray());
        WriteFile(GetPath(filename), content );
    }

    public static List<T> ReadListFromJson<T> (string filename)
    {
        string content = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T> ();
        }

        List<T> result = JsonHelper.FromJson<T>(content).ToList();

        return result;
    }


    private static void WriteFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }


#endregion CODE TO DEAL WITH SAVING HIGH SCORE LIST

}






public static class JsonHelper 
{
    public static T[] FromJson<T> (string json) {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
        return wrapper.Items;
    }

    public static string ToJson<T> (T[] array) {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.Items = array;
        return JsonUtility.ToJson (wrapper);
    }

    public static string ToJson<T> (T[] array, bool prettyPrint) {
        Wrapper<T> wrapper = new Wrapper<T> ();
        wrapper.Items = array;
        return JsonUtility.ToJson (wrapper, prettyPrint);
    }

    private class Wrapper<T> {
        public T[] Items;
    }
}
