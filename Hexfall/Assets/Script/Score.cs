using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Score : MonoBehaviour
{
    private int score { get; set; }
    public TMP_Text scoreText;
    GridManager gridManager;
    private void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        score = 0;
    }
    public int GetScore()
    {
        return score;
    }
    ///////// her 1000 skorda bir bomba altıgen üretilir.
    public void SetScore(int _score)
    {
        int mod1 = score % 1000;
        int mod2 = _score % 1000;

        score = _score;
        scoreText.text = "Score: " + score.ToString();

        if (mod1 > mod2)
        {
            gridManager.isNextBomb = true;
            //bomb
        }

    }




}