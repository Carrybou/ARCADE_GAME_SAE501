/*
 Vous n'aurez pas besoin de modifier cette classe ! Elle est utilisée pour la saisie du highscore par le joueur à la fin de la partie.
 La méthode ShowHighscoreInput() et HideHighscoreInput() permettent d'afficher et de cacher l'interface.
 Les animations sont dans Assets/Animations/Anatidae.
*/

using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections;

namespace Anatidae {
    public class HighscoreNameInput : MonoBehaviour
    {
        [SerializeField] RectTransform letterCaroussel;
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text inputName;
        Animator animator;
        char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789♥_  ".ToCharArray();
        char[] playerName = new char[3];
        private int carousselLetterIndex = 0;
        private int nameLetterIndex = 0;
        private bool blockInput = false;
        private int highscore = 0;

        void Start()
        {
            inputName.text = new string(playerName);
            animator = GetComponent<Animator>();
        }

        public void ShowHighscoreInput(int highscore)
        {
            gameObject.SetActive(true);
            if (playerName == new char[3]) {
                playerName = new char[3];
                nameLetterIndex = 0;
                carousselLetterIndex = 0;
                blockInput = false;
            }
            inputName.text = new string(playerName);
            this.highscore = highscore;
            scoreText.text = highscore.ToString();
        }

        public void FadeoutHighscoreInput()
        {
            animator.SetTrigger("Fadeout");
        }

        public void DisableHighscoreInput()
        {
            HighscoreManager.DisableHighscoreInput();
        }

        const float repeatTime = .1f;
        float lastKeyInputTime = 0f;
        bool neutralInput;
        bool inputLeft;
        bool inputRight;

        void Update()
        {
            letterCaroussel.anchoredPosition = Vector2.Lerp(letterCaroussel.anchoredPosition, new Vector2(-carousselLetterIndex * 140, 0), Time.deltaTime * 10);

            neutralInput = Input.GetAxisRaw("P1_Horizontal") > -.9f && Input.GetAxisRaw("P1_Horizontal") < .9f;

            if ((Input.GetAxisRaw("P1_Horizontal") < -.9f && neutralInput) || (Input.GetAxisRaw("P1_Horizontal") > .9f && neutralInput) || lastKeyInputTime + repeatTime < Time.time) {
                neutralInput = false;
                inputLeft = Input.GetAxisRaw("P1_Horizontal") < -.9f;
                inputRight = Input.GetAxisRaw("P1_Horizontal") > .9f;
            }

            if (inputLeft)
            {
                if (blockInput && carousselLetterIndex == alphabet.Length - 2) return;
                carousselLetterIndex = (carousselLetterIndex - 1 + alphabet.Length) % alphabet.Length;
                if (carousselLetterIndex == alphabet.Length - 1) letterCaroussel.anchoredPosition = new Vector2(alphabet.Length * -140, 0); // Wrap
                inputLeft = false;
                lastKeyInputTime = Time.time;
            }
            else if (inputRight)
            {
                if (blockInput && carousselLetterIndex == alphabet.Length - 1) return;
                carousselLetterIndex = (carousselLetterIndex + 1) % alphabet.Length;
                if (carousselLetterIndex == 0) letterCaroussel.anchoredPosition = new Vector2(140, 0); // Wrap
                inputRight = false;
                lastKeyInputTime = Time.time;

            }
            else if (Input.GetButtonDown("P1_B1"))
            {
                if (carousselLetterIndex == alphabet.Length - 2) { // Submit
                    HighscoreManager.PlayerName = Regex.Replace(new string(playerName), @"\0", "_");
                    StartCoroutine(SetHighscore(HighscoreManager.PlayerName, highscore));
                }

                if (carousselLetterIndex == alphabet.Length - 1) { // Backspace
                    if (nameLetterIndex == 0) return;
                    nameLetterIndex -= 1;
                    playerName[nameLetterIndex] = '\0';
                    inputName.text = new string(playerName);
                    blockInput = false;
                    return;
                }

                // Letter
                if (!blockInput)
                {
                    playerName[nameLetterIndex] = alphabet[carousselLetterIndex];
                    nameLetterIndex += 1;
                    if (nameLetterIndex == playerName.Length) {
                        blockInput = true;
                        carousselLetterIndex = alphabet.Length - 2; // Put to end char
                    }
                }

                inputName.text = new string(playerName);
            }
        }

        IEnumerator SetHighscore(string name, int score)
        {
            yield return HighscoreManager.SetHighscore(name, score);
            Debug.Log("HighscoreNameInput: Highscore submitted");
            FadeoutHighscoreInput();
        }
    }
}
