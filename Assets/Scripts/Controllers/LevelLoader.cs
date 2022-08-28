using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float animationTime = 1;
    [SerializeField] private float preAnimationTime = 1;

    public void ReloadLevel()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex));
    }

    public void NextLevel()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex+1));
    }

    private IEnumerator LoadScene(int buildIndex)
    {
        yield return new WaitForSeconds(preAnimationTime);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(animationTime);

        SceneManager.LoadScene(buildIndex);
    }

    public void InstantLoadNextScene()
    {
        StartCoroutine(InstantLoadScene(SceneManager.GetActiveScene().buildIndex+1));
    }

    private IEnumerator InstantLoadScene(int buildIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(animationTime);

        SceneManager.LoadScene(buildIndex);
    }
}
