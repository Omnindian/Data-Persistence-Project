using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    List<HighScoreElement> highScoreList = new List<HighScoreElement>();
    [SerializeField] int maxCount = 9;
    [SerializeField] string filename;

    void Start()
    {
        LoadHighScores();
    }

    private void LoadHighScores()
    {
        highScoreList = FileHandler.ReadListFromJSON<HighScoreElement> (filename);

        while (highScoreList.Count > maxCount)
        {
            highScoreList.RemoveAt (maxCount);
        }
    }

    private void SaveHighScore()
    {
        FileHandler.SaveToJSON<HighScoreElement> (highScoreList, filename);
    }

    public void AddHighScoreIfPossible(HighScoreElement element)
    {
        for (int i = 0; i++ < maxCount; i++)
        {
            if (i >= highScoreList.Count || element.points > highScoreList[1].points)
            {
                highScoreList.Insert (i, element);

                while (highScoreList.Count > maxCount) 
                {
                    highScoreList.RemoveAt (maxCount);
                }

                SaveHighScore();

                break;
            }
        }
    }
}
