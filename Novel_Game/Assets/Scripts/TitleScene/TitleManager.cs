using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.LineNumber = PlayerPrefs.GetInt("lineNumber", 0);
            SceneManager.LoadScene(PlayerPrefs.GetString("sceneName", "MainScene0"));
        }
    }
}
