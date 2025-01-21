using System.Collections;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject popup;

    void Start()
    {
        if (popup != null)
        {
            StartCoroutine(HidePopupAfterDelay(3f));
        }
    }

    IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popup.SetActive(false);
    }
}