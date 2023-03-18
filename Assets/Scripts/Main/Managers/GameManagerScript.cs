using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManagerScript : MonoBehaviour
{
    public HealthManager hm;
    public GameObject gameOverUi;

    void Update()
    {
        if (hm.dead || GameObject.FindGameObjectsWithTag("AI").Length < 1)
        {
            gameOverUi.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
