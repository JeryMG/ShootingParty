using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Image FadePlane;
    public GameObject gameOverUI;
    
    void Start()
    {
        FindObjectOfType<Player>().OnDeath += OnGameOver;
    }

    void OnGameOver()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1f));
        gameOverUI.SetActive(true);
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Game");
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent <1)
        {
            percent += Time.deltaTime * speed;
            FadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }
}
