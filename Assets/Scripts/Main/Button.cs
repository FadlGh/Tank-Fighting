using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Level" + gameObject.name);
    }
}
