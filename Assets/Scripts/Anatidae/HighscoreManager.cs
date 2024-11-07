using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

namespace Anatidae {

    public class HighscoreManager : MonoBehaviour
    {
        // Changez cette variable par le nom de votre jeu
        // Ce nom sera le même que le nom du dossier contenant votre build, il ne doit donc pas contenir de caractères spéciaux ni d'espaces
        // Cette variable est utilisée pour stocker les highscores sur le serveur !
        const string GameName = "Casse_Les_Boules";
        // Changez cette variable pour définir quand est-ce qu'un score est considéré comme un highscore (top 10 par défaut)
        const int NumHighscores = 10;

        [Serializable]
        public struct HighscoreData
        {
            public List<HighscoreEntry> highscores;
        }

        [Serializable]
        public struct HighscoreEntry
        {
            public string name;
            public int score;
        }

        public static HighscoreManager Instance { get; private set; }
        public static List<HighscoreEntry> Highscores { get; private set;}
        public static bool HasFetchedHighscores { get; private set; }
        public static bool IsHighscoreInputScreenShown { get; private set; }
        public static string PlayerName;

        [SerializeField] HighscoreNameInput highscoreNameInput;
        [SerializeField] HighscoreUI highscoreUi;

        void Awake()
        {
            if (Instance == null){
                Instance = this;
            }
            else{
                Destroy(gameObject);
            }

            if (Instance.highscoreNameInput is null)
                Debug.LogError("HighscoreNameInput de HighscoreManager n'est pas défini.");
            else highscoreNameInput.gameObject.SetActive(false);

            if (Instance.highscoreUi is null)
                Debug.LogError("HighscoreUI de HighscoreManager n'est pas défini.");
            else highscoreUi.gameObject.SetActive(false);
        }

        public static void ShowHighscores()
        {
            if (Instance.highscoreUi is null){
                Debug.LogError("HighscoreUI de HighscoreManager n'est pas défini.");
                return;
            }

            Instance.highscoreUi.gameObject.SetActive(true);
        }

        public static void HideHighscores()
        {
            if (Instance.highscoreUi is null){
                Debug.LogError("HighscoreUI de HighscoreManager n'est pas défini.");
                return;
            }

            Instance.highscoreUi.gameObject.SetActive(false);
        }
        
        public static void ShowHighscoreInput(int highscore)
        {
            if (Instance.highscoreNameInput is null){
                Debug.LogError("HighscoreNameInput de HighscoreManager n'est pas défini.");
                return;
            }

            Instance.highscoreNameInput.ShowHighscoreInput(highscore);
            IsHighscoreInputScreenShown = true;
        }

        public static void DisableHighscoreInput()
        {
            if (Instance.highscoreNameInput is null){
                Debug.LogError("HighscoreNameInput de HighscoreManager n'est pas défini.");
                return;
            }

            Instance.highscoreNameInput.gameObject.SetActive(false); 
            IsHighscoreInputScreenShown = false;
        }

        public static IEnumerator FetchHighscores()
        {
            Debug.Log("HighscoreManager: Fetching highscores...");
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/api/?game=" + GameName);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string data = request.downloadHandler.text;
                try {
                    HighscoreData highscoreData = JsonUtility.FromJson<HighscoreData>(data);
                    Highscores = highscoreData.highscores;
                    HasFetchedHighscores = true;
                    Debug.Log("HighscoreManager: Highscores fetched!");
                } catch (Exception e) {
                    Debug.LogError(e);
                }
            }
        }

        public static IEnumerator SetHighscore(string name, int score)
        {
            Debug.Log(JsonUtility.ToJson(new HighscoreEntry { name = name, score = score }));

            UnityWebRequest request = new UnityWebRequest("http://localhost:3000/api/?game=" + GameName)
            {
                method = UnityWebRequest.kHttpVerbPOST,
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(new HighscoreEntry { name = name, score = score })))
                {
                    contentType = "application/json"
                }
            };
            
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                Debug.LogError(request.error);

            yield return FetchHighscores();
        }

        public static bool IsHighscore(int score)
        {
            return IsHighscore(null, score);
        }
        
        public static bool IsHighscore(string name, int score)
        {
            if (!HasFetchedHighscores) {
                Debug.LogError("HighscoreManager: IsHighscore() appelé avant que les highscores ne soient récupérés. Appelez la coroutine FetchHighscores() avant d'utiliser IsHighscore().");
                return false;
            }

            if (name == null)
            {
                if (Highscores == null || Highscores.Count < NumHighscores || score > Highscores[NumHighscores - 1].score)
                    return true;
            } else {
                HighscoreEntry? entry = Highscores.Find(entry => entry.name == name);
                if(entry?.score < score)
                    return true;
            }
            return false;
        }
    }
}

