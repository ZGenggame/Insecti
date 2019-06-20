using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {

    public Text CurrentScore, BestScore;

    public void ShowScore(int current,int best) {
        CurrentScore.text = current.ToString();
        BestScore.text = best.ToString();
    }
}
