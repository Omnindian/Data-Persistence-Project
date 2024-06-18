/* Attach this code to 
the list viewer gameobject
or contentholder */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreListHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject singleEntryUIPrefab, contentHolder;
    [SerializeField]
    private int maxEntriesToShow = 10; // physically measured in the list view but adjustable in inspector
    List<HighScoreElement> highscoreEntries;

    void Start()
    {
        highscoreEntries = GameManager.Instance.GetHighScores();
        ShowList();
    }


    public void ShowList()
    {
        int maxToUse()
        {
            // determine which is more betweem entry count or max varable defined 
            // return whichever is less
            return (maxEntriesToShow >= highscoreEntries.Count ? highscoreEntries.Count : maxEntriesToShow);
        }

        // use returned value for maxToUse here
        for (int i = 0; i < maxToUse(); i++)
            {
                PopulateItem(i);
            }
    }

    private void PopulateItem(int index)
    {
        GameObject listItem = Instantiate(singleEntryUIPrefab, contentHolder.transform);
        listItem.transform.GetChild(0).GetComponent<TMP_Text>().text = highscoreEntries[index].playerName;
        listItem.transform.GetChild(1).GetComponent<TMP_Text>().text = highscoreEntries[index].highScore.ToString();


        //// Test Populate
        // listItem.transform.GetChild(0).GetComponent<TMP_Text>().text = "OMNIGF";
        // listItem.transform.GetChild(1).GetComponent<TMP_Text>().text = Random.Range(0, 100).ToString();

        ////////This does not have a delegate nor the list a listener for dynamic updates /////////
    }

}
