using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //これプロパティ使う意味ある……？
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
    private bool saveData = false;
    public bool SaveData { get { return saveData; } }
    private float bgmVolume = 0.25f;
    public float BgmVolume { get { return bgmVolume; } set { bgmVolume = value; } }
    private float seVolume = 0.25f;
    public float SeVolume { get {return seVolume; } set { seVolume = value; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;           //60FPS固定
            if (PlayerPrefs.GetString("sceneName") != "")
            {
                saveData = true;
            }
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
        saveData = true;
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
