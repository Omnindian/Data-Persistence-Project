using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public void MyTester()
    {
        GameManager.Instance.AddHighScoreIfPossible(new HighScoreElement("Omni", Random.Range(0, 100)));
    }
}
