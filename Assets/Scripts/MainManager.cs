using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText, highScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    public static int m_Points;
    
    private bool m_GameOver = false;
    [SerializeField]
    private GameObject attachedToMusicAudio, brickSoundsSource;
    private AudioSource musicAudio, brickSounds;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Points = 0; // Reset present player points
        GameManager.Instance.GameRunning = true;
        musicAudio = attachedToMusicAudio.GetComponent<AudioSource>();
        if(GameManager.Instance.MusicOn == false)
        {
            musicAudio.volume = 0.0f;
        }
        else
        {
            musicAudio.volume = GameManager.Instance.MusicVolume;
        }
        
        brickSounds = brickSoundsSource.GetComponent<AudioSource>();

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void Awake()
    {
        ScoreText.text = GameManager.Instance.PlayerName + " Score : " + m_Points;
        ShowHighScoreText();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        UpdateTopScoreText();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = GameManager.Instance.PlayerName + " Score : " + m_Points;
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameManager.Instance.OnEndGame();
        GameOverText.SetActive(true);
    }

    private void ShowHighScoreText()
    {
        if (GameManager.Instance.TopScoreInfo() != null)
        {
            highScoreText.text = "Best Score: " + GameManager.Instance.TopScoreInfo().playerName + " : " + GameManager.Instance.TopScoreInfo().highScore;
        }
        else
        {
            highScoreText.text = "Best Score: Nobody : 0";
        }
    }

    private void UpdateTopScoreText()
    {
        // to update and show the high score if previous score is beaten by current player
        if (GameManager.Instance.TopScoreInfo() != null)
        {
            if (m_Points >= GameManager.Instance.TopScoreInfo().highScore)
            {
                highScoreText.text = "Best Score: " + GameManager.Instance.PlayerName + " : " + m_Points;
            }
        }
    }
}
