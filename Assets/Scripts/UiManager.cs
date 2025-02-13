using System.Collections;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text doublePointsText;
    [SerializeField] GameObject freezeScreen; // ✅ Ajout du GameObject pour l'effet de gel

    void Start()
    {
        scoreText.text = "0";
        doublePointsText.gameObject.SetActive(false);

        // ✅ Désactiver le freezeScreen au démarrage
        if (freezeScreen != null)
        {
            freezeScreen.SetActive(false);
        }
    }

    void Update()
    {
        scoreText.text = GameManager.Instance.score.ToString();

        // ✅ Active/Désactive l'effet de gel selon le slow motion
        if (freezeScreen != null)
        {
            freezeScreen.SetActive(GameManager.Instance.isSlowMotionActive);
        }
    }

    // ✅ Affichage du texte "Double Points"
    public void ShowDoublePointsText()
    {
        doublePointsText.gameObject.SetActive(true);
        doublePointsText.alpha = 1f;
    }

    // ✅ Masquer le texte "Double Points"
    public void HideDoublePointsText()
    {
        doublePointsText.gameObject.SetActive(false);
    }

    // ✅ Faire clignoter "Double Points" avant expiration
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
