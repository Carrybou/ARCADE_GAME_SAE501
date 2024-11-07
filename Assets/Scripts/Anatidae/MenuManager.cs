using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text quitText;
    const float AfkTime = 200f;
    float afkTimer = 0f;
    const float HeldQuitTime = 1.5f;
    float heldQuitTimer = 0f;
    const string MenuMessage = "Retour au menu";

    [DllImport("__Internal")]
    public static extern void BackToMenu();

    void Update()
    {
        if (heldQuitTimer >= HeldQuitTime || afkTimer >= AfkTime) {
            BackToMenu();
        }

        if (Input.GetButton("Coin"))
            heldQuitTimer += Time.deltaTime;
        else
            heldQuitTimer = 0f;

        if (Input.GetAxisRaw("P1_Horizontal") != 0 || Input.GetAxisRaw("P1_Vertical") != 0 || Input.GetButton("P1_Start") || Input.GetButton("P1_B1") || Input.GetButton("P1_B2") || Input.GetButton("P1_B3") || Input.GetButton("P1_B4") || Input.GetButton("P1_B5") || Input.GetButton("P1_B6") ||
            Input.GetAxisRaw("P2_Horizontal") != 0 || Input.GetAxisRaw("P2_Vertical") != 0 || Input.GetButton("P2_Start") || Input.GetButton("P2_B1") || Input.GetButton("P2_B2") || Input.GetButton("P2_B3") || Input.GetButton("P2_B4") || Input.GetButton("P2_B5") || Input.GetButton("P2_B6"))
            afkTimer = 0f;
        else
            afkTimer += Time.deltaTime;

        if (heldQuitTimer != 0 || afkTimer - AfkTime + 10f > 0f) {
            quitText.gameObject.SetActive(true);
            quitText.text = MenuMessage + new string('.', (int)Mathf.Max(heldQuitTimer * 3f, afkTimer - AfkTime + 10f * 0.5f));
        } else quitText.gameObject.SetActive(false);
    }

    public void OnApplicationQuit()
    {
        BackToMenu();
    }
}
