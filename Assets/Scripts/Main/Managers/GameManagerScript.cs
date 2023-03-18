using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public HealthManager hm;
    public GameObject gameOverUi;

    void Start()
    {
        hm = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
    }
    void Update()
    {
        if (hm.dead)
        {
            gameOverUi.transform.Find("State").GetComponent<Text>().text = "You Lost!";
            gameOverUi.SetActive(true);
        }

        if (GameObject.FindGameObjectsWithTag("AI").Length < 1)
        {
            gameOverUi.transform.Find("State").GetComponent<Text>().text = "You Won!!";
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
