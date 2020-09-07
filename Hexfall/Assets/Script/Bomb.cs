using UnityEngine;
using UnityEngine.UI;
public class Bomb : MonoBehaviour
{
    int bombStep,startStepCount;
    Text bombText;
    GameObject gameOverMenu;
    
    private void Start()
    {
        gameOverMenu=GameObject.Find("GameOverMenu");
        bombText = GetComponent<Text>();
        bombStep = Random.Range(4, 8);
        startStepCount = Point.stepCount;
    }

    //////Bomba objesi // eğer üzerindeki random atanan süre biterse oyun da biter.
    private void Update()
    {
        bombText.text = (-Point.stepCount + bombStep + startStepCount).ToString();

        if (bombStep + startStepCount <= Point.stepCount)
        {
            gameOverMenu.transform.GetChild(0).gameObject.SetActive(true);
        }

    }
    
}