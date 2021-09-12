using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public LeaderboardManager leaderboardManager;
    public LevelLoader levelLoader;

    public ParticleSystem explosionParticleSystem;
    public GraphicRaycaster graphicRaycaster;

    public TMP_InputField nameInputField;

    public GameObject scoreboardTextPrefab;

    public RectTransform scoreboardPanel;
    public GameObject namePanel;
    public GameObject menuPanel;
    public GameObject gamemodeSelectPanel;

    public RectTransform accountPanel;

    public Button playButton;
    public TMP_Text hello;
    public TMP_Text accountPanelTextName;
    public TMP_Text accountPanelTextPoints;

    public bool tutorialCompleted;
    public int highscore;
    public int score;

    void Awake()
    {
        leaderboardManager = GameObject.FindGameObjectWithTag("LeaderboardManager").GetComponent<LeaderboardManager>();

        if (!PlayerPrefs.HasKey("tutorialCompleted"))
        {
            PlayerPrefs.SetInt("tutorialCompleted", 0);
            tutorialCompleted = false;

        }
        else
        {
            if(PlayerPrefs.GetInt("tutorialCompleted") == 1)
            {
                tutorialCompleted = true;
            }
            else
            {
                tutorialCompleted = false;
            }


        }

        playButton.interactable = tutorialCompleted;
        
        menuPanel.SetActive(false);
        namePanel.SetActive(false);
        gamemodeSelectPanel.SetActive(false);

        hello.text = "Merhaba " + leaderboardManager.currentUser.Name;

        if (!leaderboardManager.appFirstLaunched_menuManager)
            menuPanel.SetActive(true);
        else
            namePanel.SetActive(true);

    }

    void Start()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        updateScoreboard();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D h = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (h.transform == null)
            {
                if (this.graphicRaycast().Count == 0)
                    explodeAtCursor(5.0f, 16000f, 5.0f);
            }
        }
    }

    public void updateScoreboard()
    {
        if (leaderboardManager.topUsers.Length == 0)
        {
            scoreboardPanel.sizeDelta = new Vector2(0, 0);
            return;
        }
        scoreboardPanel.sizeDelta = new Vector2(120, 10 + (leaderboardManager.topUsers.Length * 30));

        for(int i = 0; i < scoreboardPanel.childCount; i++)
        {
            Destroy(scoreboardPanel.GetChild(i).gameObject);
        }

        for(int i = 0; i < leaderboardManager.topUsers.Length; i++)
        {
            LeaderboardManager.LeaderboardUser user = leaderboardManager.topUsers[i];

            GameObject go = Instantiate(scoreboardTextPrefab, scoreboardPanel.transform);
            
            go.transform.Find("NameText").GetComponent<TMP_Text>().text = user.Name;
            go.transform.Find("PointsText").GetComponent<TMP_Text>().text = user.Points+"";
        }

        scoreboardPanel.GetComponent<BoxCollider2D>().size = scoreboardPanel.sizeDelta;
        scoreboardPanel.GetComponent<BoxCollider2D>().offset = -scoreboardPanel.sizeDelta/2;


        if (leaderboardManager.currentUser.Name != null)
        {
            accountPanel.gameObject.SetActive(true);

            accountPanel.GetComponent<BoxCollider2D>().size = accountPanel.sizeDelta;
            accountPanel.GetComponent<BoxCollider2D>().offset = -accountPanel.sizeDelta / 2;

            accountPanelTextName.text = leaderboardManager.currentUser.Name;
            accountPanelTextPoints.text = leaderboardManager.currentUser.Points+"";

            accountPanel.anchoredPosition = new Vector2(scoreboardPanel.anchoredPosition.x, scoreboardPanel.anchoredPosition.y - scoreboardPanel.sizeDelta.y - 8f);
        }
        else
        {
            accountPanel.gameObject.SetActive(false);
        }
    }

   /* public void onDeleteAccountButtonClicked()
    {
        GlobalPlayerManager.DeleteAccount(GlobalPlayerManager.currentAccountName);
        GlobalPlayerManager.currentAccountName = null;
        this.updateScoreboard();
        onChangeAccountButtonClicked();
    }*/

    public void onChangeAccountButtonClicked()
    {
        this.menuPanel.SetActive(false);
        this.namePanel.SetActive(true);
    }

    public void onQuitButtonClicked()
    {
        Application.Quit(0);
    }

    public void onNameButtonClicked()
    {
        string name = nameInputField.text.Trim();
        if(name != string.Empty)
        {
            namePanel.SetActive(false);
            gamemodeSelectPanel.SetActive(false);
            menuPanel.SetActive(true);
        }

        updateScoreboard();
        leaderboardManager.SetCurrentUserName(name);
        hello.text = "Merhaba " + leaderboardManager.currentUser.Name;
    }

    public void onNameSkipButtonClicked()
    {
        namePanel.SetActive(false);
        gamemodeSelectPanel.SetActive(false);
        menuPanel.SetActive(true);

        updateScoreboard();
        hello.text = "Merhaba " + leaderboardManager.currentUser.Name;
    }

    public void onPlayClicked()
    {
        gamemodeSelectPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void onReturnToMenuClicked()
    {
        gamemodeSelectPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void onNormalPlayClicked()
    {
        levelLoader.LoadScene("BuildScene");
    }

    public void onSandboxPlayClicked()
    {
        levelLoader.LoadScene("SandboxScene");
    }

    public void onTutorialClicked()
    {
        levelLoader.LoadScene("TutorialScene");
    }

    public List<RaycastResult> graphicRaycast()
    {
        PointerEventData ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(ped, results);

        return results;
    }

    public void explodeAtCursor(float radius, float power,float upliftModifier)
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 explosionPos_ = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 explosionPos = new Vector2(explosionPos_.x, explosionPos_.y);

        explosionParticleSystem.transform.position = explosionPos;
        explosionParticleSystem.Play(false);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null && hit.gameObject.tag == "Atom")
            {

                this.AddExplosionForce(rb, power, explosionPos, radius, upliftModifier);
            }
        }
    }

    public void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        Vector3 baseForce = dir.normalized * explosionForce * wearoff;
        body.AddForce(baseForce);

        float upliftWearoff = 1 - upliftModifier / explosionRadius;
        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
        body.AddForce(upliftForce);
    }
}
