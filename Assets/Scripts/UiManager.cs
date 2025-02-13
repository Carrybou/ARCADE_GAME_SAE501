using System.Collections;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text doublePointsText;

    void Start()
    {
        scoreText.text = "0";
        doublePointsText.gameObject.SetActive(false);
    }

    void Update()
    {
        scoreText.text = GameManager.Instance.score.ToString();
    }

    public void ShowDoublePointsText()
    {
        doublePointsText.gameObject.SetActive(true);
        doublePointsText.alpha = 1f;
    }


    public void HideDoublePointsText()
    {
        doublePointsText.gameObject.SetActive(false);
    }

    public IEnumerator BlinkDoublePointsText()
    {
        for (int i = 0; i < 5; i++)
        {
            doublePointsText.enabled = !doublePointsText.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        doublePointsText.enabled = true;
    }
}
