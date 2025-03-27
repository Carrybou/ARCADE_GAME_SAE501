using UnityEngine;

public class ReplayButtonController : MonoBehaviour
{
    [SerializeField] GameObject replayButton;

    private bool isGameOver;

    void Update()
    {
        Debug.Log($"GameOver: {GameManager.Instance?.isGameOver}");
        Debug.Log($"HighscoreInputScreenShown: {Anatidae.HighscoreManager.IsHighscoreInputScreenShown}");

    }
    public void replayButtonFunction(bool state)
    {
        replayButton.SetActive(state);

        /*
        if (Anatidae.HighscoreManager.IsHighscoreInputScreenShown == false)
            {
                replayButton.SetActive(true);
            }*/
    }
}