using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float resX = 1179;
    public float resY = 2556;

    public string phone;
    public string password;

    public GameObject arPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
