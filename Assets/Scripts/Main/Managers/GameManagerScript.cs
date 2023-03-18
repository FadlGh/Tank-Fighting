using UnityEngine;
using UnityEngine.SceneManagement;

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
