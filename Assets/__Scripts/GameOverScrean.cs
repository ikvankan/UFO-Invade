using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScrean : MonoBehaviour
{
    // Start is called before the first frame update
    public Text pointsText;
    public GameObject GameObject;
    public void SetUp(int score)
    {
        GameObject.SetActive(true);
        pointsText.text = "Points: " + score;
    }
}
