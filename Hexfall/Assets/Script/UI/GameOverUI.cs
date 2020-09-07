using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameOverUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject newHighScore;
    public GameObject HomeMenu;
    Score score;
    void Start()
    {
        score = FindObjectOfType<Score>();
        Time.timeScale=0;
        if(score.GetScore()>PlayerPrefs.GetInt("HighScore",0)){
            Debug.Log("New high score");
            PlayerPrefs.SetInt("HighScore",score.GetScore());
            newHighScore.SetActive(true);

        }
        scoreText.text = "Score: "+score.GetScore().ToString();

    }

    
   public void GotoMenu(){
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
  
}
