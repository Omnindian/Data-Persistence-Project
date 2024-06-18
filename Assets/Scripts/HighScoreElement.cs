[System.Serializable]

public class HighScoreElement
{
    public string playerName;
    public int highScore;

    public HighScoreElement(string name, int points)
    {
        playerName = name;
        highScore = points;
    }
}
