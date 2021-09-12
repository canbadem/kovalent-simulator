using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    public float transitionTime = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);

        transition.SetTrigger("End");
    }

}
