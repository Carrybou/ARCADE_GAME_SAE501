using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Anatidae {
    public class HighscoreEntryGo : MonoBehaviour
    {
        [SerializeField] TMP_Text nameText;
        [SerializeField] TMP_Text scoreText;

        public void SetData(HighscoreManager.HighscoreEntry entry)
        {
            nameText.text = entry.name;
            scoreText.text = entry.score.ToString();
        }

        public void SetScale(float scale)
        {
            nameText.fontSize = (int)(nameText.fontSize * scale);
            scoreText.fontSize = (int)(scoreText.fontSize * scale);
        }
    }
}