using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    private const string privateKey = "";
    private const string SubmitScoreUrl = "http://localhost/Kovalent/SubmitScore.php";
    private const string GetScoreUrl = "http://localhost/Kovalent/GetScore.php";
    private const string TopScoresUrl = "http://localhost/Kovalent/TopScores.php";

    private const int requestTimeout = 1;

    public int acquiredScore = 0; // php GetScore

    public LeaderboardUser[] topUsers;
    public LeaderboardUser currentUser;

    public MenuManager menuManager;

    public bool appFirstLaunched_menuManager = true;

    void Start()
    {
        //  StartCoroutine(SubmitScore("   ", 5000));
        // StartCoroutine(GetTopScores());

      //  StartCoroutine(SubmitScore("Can", 123));
    }

    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
        }

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        currentUser = new LeaderboardUser("Kullanıcı", 0);
        StartCoroutine(GetTopScores());
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        appFirstLaunched_menuManager = false;
        if (arg0.name == "MenuScene")
        {
            menuManager = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
        }
        else
        {
            //menuManager = null;
        }
    }

    private void SubmitScore(string name, int score)
    {
        StartCoroutine(SubmitScoreCoroutine(name, score));
    }

    private void GetScore(string name)
    {
        StartCoroutine(GetScoreCoroutine(name));
    }

    public void SetCurrentUserName(string name)
    {
        currentUser.Name = name;
        GetScore(currentUser.Name);
    }

    public void SetCurrentUserPoints(int points)
    {
        currentUser.Points = points;
        SubmitScore(currentUser.Name, points);
    }

    void OnScoreAcquired(string name)
    {
        if (name.Equals(currentUser.Name))
        {
            currentUser.Points = acquiredScore;
        }

        if(menuManager != null)
        {
            menuManager.updateScoreboard();
        }

    }

    void OnTopScoresAcquired()
    {
        if (menuManager != null)
        {
            menuManager.updateScoreboard();
        }
    }

    IEnumerator GetTopScores()
    {
        Debug.Log("Getting top scores...");
        using (UnityWebRequest request = UnityWebRequest.Get(TopScoresUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("SubmitScore | " + request.error);
            }
            else
            {

                float time = Time.realtimeSinceStartup;

                while (!request.downloadHandler.isDone)
                {
                    if ((Time.realtimeSinceStartup - time) > requestTimeout)
                    {
                        break;
                    }
                }

                string scoresText = request.downloadHandler.text;

                if (scoresText.Length > 0)
                {

                    string[] textlist = scoresText.Split(new string[] { "\n", "\t" }, System.StringSplitOptions.RemoveEmptyEntries);

                    string[] Names = new string[Mathf.FloorToInt(textlist.Length / 2)];
                    string[] Scores = new string[Names.Length];
                    for (int i = 0; i < textlist.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            Names[Mathf.FloorToInt(i / 2)] = textlist[i];
                        }
                        else Scores[Mathf.FloorToInt(i / 2)] = textlist[i];
                    }

                    topUsers = new LeaderboardUser[Names.Length];

                    for (int i = 0; i < Names.Length; i++)
                    {
                        if(Names[i] != null && Scores[i] != null)
                        {
                            string name = Names[i];
                            int score = 0;

                            if(int.TryParse(Scores[i], out score))
                            {
                                topUsers[i] = new LeaderboardUser(name, score);
                            }

                        }
                            
                    }

                    Debug.Log("Got top scores.");
                    OnTopScoresAcquired();
                }


            }

        }
    }

    IEnumerator GetScoreCoroutine(string name)
    {
        Debug.Log("Getting score of " + name);
        WWWForm form = new WWWForm();
        form.AddField("playerName", name);

        using (UnityWebRequest request = UnityWebRequest.Post(GetScoreUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("SubmitScore | " + request.error);
            }
            else
            {

                float time = Time.realtimeSinceStartup;

                while (!request.downloadHandler.isDone)
                {
                    if ((Time.realtimeSinceStartup - time) > requestTimeout)
                    {
                        break;
                    }
                }

                string receivedScore = request.downloadHandler.text;

                int.TryParse(receivedScore, out acquiredScore);

                Debug.Log("Got score of " + name);

                OnScoreAcquired(name);
            }
            
        }
    }

    IEnumerator SubmitScoreCoroutine(string name, int score)
    {
        Debug.Log("Submitting " + name + " with a score of " + score);
        string hash = Md5Sum(name + score + privateKey);

        WWWForm form = new WWWForm();
        form.AddField("playerName", name);
        form.AddField("playerScore", score);
        form.AddField("hash", hash);

        using (UnityWebRequest request = UnityWebRequest.Post(SubmitScoreUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("SubmitScore | " + request.error);
            }
            else
            {
                Debug.Log("Submitted score");
            }
        }

    }

    [System.Serializable]
    public struct LeaderboardUser
    {
        public string Name;
        public int Points;

        public LeaderboardUser(string name, int points)
        {
            Name = name;
            Points = points;
        }
    }

    private static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

}
