using UnityEngine;

public class GameManager : MonoBehaviour
{
    //順当にいけば値がnullになって問題が起きることはないはずだが、初期化については深く考えるべきなのかどうか
    public static GameManager instance;
    private static string sceneName;
    public string SceneName { set { sceneName = value; } get { return sceneName; } }
    private static int lineNumber;
    public int LineNumber { set { lineNumber = value; } get { return lineNumber; } }
    private static int exp;
    public int EXP { set { exp = value; } get { return exp; } }
    private static int sainHP;
    public int SainHP { set { sainHP = value; } get {return sainHP; } }
    private static int sainAttack;
    public int SainAttack { set { sainAttack = value; } get { return sainAttack; } }
    private int sainSG;
    public int SainSG { set { sainSG = value; } get { return sainSG; } }
    //そのうちステータスも
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;           //60FPS固定
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
        PlayerPrefs.SetInt("exp", exp);
        PlayerPrefs.SetInt("sainHP", sainHP);
        PlayerPrefs.SetInt("sainAttack", sainAttack);
        PlayerPrefs.SetInt("sainSG", sainSG);
    }
    
    //セーブデータ削除(セーブデータを複数にした場合は選択できるようにする)
    public void Initialize()
    {
        PlayerPrefs.SetString("sceneName", "MainScene0");
        PlayerPrefs.SetInt("lineNumber", 0);
        PlayerPrefs.SetInt("exp", 0);
        PlayerPrefs.SetInt("sainHP", 3000);
        PlayerPrefs.SetInt("sainAttack", 200);
        PlayerPrefs.SetInt("sainSG", 50);
        Set();
    }

    //セーブデータと一時記憶の同期
    public void Set()
    {
        sceneName = PlayerPrefs.GetString("sceneName");
        lineNumber = PlayerPrefs.GetInt("lineNumber");
        exp = PlayerPrefs.GetInt("exp");
        sainHP = PlayerPrefs.GetInt("sainHP");
        sainAttack = PlayerPrefs.GetInt("sainAttack");
        sainSG = PlayerPrefs.GetInt("sainSG");
    }
}
