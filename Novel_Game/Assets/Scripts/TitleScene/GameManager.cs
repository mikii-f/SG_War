using UnityEngine;

public class GameManager : MonoBehaviour
{
    //順当にいけば値がnullになって問題が起きることはないはずだが、初期化については深く考えるべきなのかどうか
    public static GameManager instance;
    private static string sceneName;
    public string SceneName { set { sceneName = value; } get { return sceneName; } }
    private static int lineNumber;
    public int LineNumber { set { lineNumber = value; } get { return lineNumber; } }
    //そのうちステータスも
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Save()
    {
        PlayerPrefs.SetString("sceneName", sceneName);
        PlayerPrefs.SetInt("lineNumber", lineNumber);
    }
}
