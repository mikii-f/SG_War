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
    private string progress;
    public string Progress { set { progress = value; } get { return progress; } }
    private bool saveData = false;
    public bool SaveData { get { return saveData; } }
    private float bgmVolume = 0.25f;
    public float BgmVolume { get { return bgmVolume; } set { bgmVolume = value; } }
    private float seVolume = 0.25f;
    public float SeVolume { get {return seVolume; } set { seVolume = value; } }
    private int saveDataNumber = 1;
    public int SaveDataNumber { get { return saveDataNumber; } set { saveDataNumber = value; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;           //60FPS固定
            if (PlayerPrefs.GetString("sceneName") != "" || PlayerPrefs.GetString("sceneName2") != "" || PlayerPrefs.GetString("sceneName3") != "")
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
        if (saveDataNumber == 1)
        {
            PlayerPrefs.SetString("sceneName", sceneName);
            PlayerPrefs.SetInt("lineNumber", lineNumber);
            PlayerPrefs.SetInt("exp", exp);
            PlayerPrefs.SetInt("sainHP", sainHP);
            PlayerPrefs.SetInt("sainAttack", sainAttack);
            PlayerPrefs.SetInt("sainSG", sainSG);
            PlayerPrefs.SetString("progress", progress);
        }
        else if (saveDataNumber == 2)
        {
            PlayerPrefs.SetString("sceneName2", sceneName);
            PlayerPrefs.SetInt("lineNumber2", lineNumber);
            PlayerPrefs.SetInt("exp2", exp);
            PlayerPrefs.SetInt("sainHP2", sainHP);
            PlayerPrefs.SetInt("sainAttack2", sainAttack);
            PlayerPrefs.SetInt("sainSG2", sainSG);
            PlayerPrefs.SetString("progress2", progress);
        }
        else
        {
            PlayerPrefs.SetString("sceneName3", sceneName);
            PlayerPrefs.SetInt("lineNumber3", lineNumber);
            PlayerPrefs.SetInt("exp3", exp);
            PlayerPrefs.SetInt("sainHP3", sainHP);
            PlayerPrefs.SetInt("sainAttack3", sainAttack);
            PlayerPrefs.SetInt("sainSG3", sainSG);
            PlayerPrefs.SetString("progress3", progress);
        }
    }
    
    //セーブデータ削除
    public void Initialize()
    {
        if (saveDataNumber == 1)
        {
            PlayerPrefs.SetString("sceneName", "MainScene0");
            PlayerPrefs.SetInt("lineNumber", 0);
            PlayerPrefs.SetInt("exp", 0);
            PlayerPrefs.SetInt("sainHP", 3000);
            PlayerPrefs.SetInt("sainAttack", 200);
            PlayerPrefs.SetInt("sainSG", 50);
            PlayerPrefs.SetString("progress", "0章");
            Set();
        }
        else if (saveDataNumber == 2)
        {
            PlayerPrefs.SetString("sceneName2", "MainScene0");
            PlayerPrefs.SetInt("lineNumber2", 0);
            PlayerPrefs.SetInt("exp2", 0);
            PlayerPrefs.SetInt("sainHP2", 3000);
            PlayerPrefs.SetInt("sainAttack2", 200);
            PlayerPrefs.SetInt("sainSG2", 50);
            PlayerPrefs.SetString("progress2", "0章");
            Set();
        }
        else
        {
            PlayerPrefs.SetString("sceneName3", "MainScene0");
            PlayerPrefs.SetInt("lineNumber3", 0);
            PlayerPrefs.SetInt("exp3", 0);
            PlayerPrefs.SetInt("sainHP3", 3000);
            PlayerPrefs.SetInt("sainAttack3", 200);
            PlayerPrefs.SetInt("sainSG3", 50);
            PlayerPrefs.SetString("progress3", "0章");
            Set();
        }
        saveData = true;
    }

    //セーブデータと一時記憶の同期
    public void Set()
    {
        if (saveDataNumber == 1)
        {
            sceneName = PlayerPrefs.GetString("sceneName");
            lineNumber = PlayerPrefs.GetInt("lineNumber");
            exp = PlayerPrefs.GetInt("exp");
            sainHP = PlayerPrefs.GetInt("sainHP");
            sainAttack = PlayerPrefs.GetInt("sainAttack");
            sainSG = PlayerPrefs.GetInt("sainSG");
            progress = PlayerPrefs.GetString("progress");
        }
        else if (saveDataNumber == 2)
        {
            sceneName = PlayerPrefs.GetString("sceneName2");
            lineNumber = PlayerPrefs.GetInt("lineNumber2");
            exp = PlayerPrefs.GetInt("exp2");
            sainHP = PlayerPrefs.GetInt("sainHP2");
            sainAttack = PlayerPrefs.GetInt("sainAttack2");
            sainSG = PlayerPrefs.GetInt("sainSG2");
            progress = PlayerPrefs.GetString("progress2");
        }
        else
        {
            sceneName = PlayerPrefs.GetString("sceneName3");
            lineNumber = PlayerPrefs.GetInt("lineNumber3");
            exp = PlayerPrefs.GetInt("exp3");
            sainHP = PlayerPrefs.GetInt("sainHP3");
            sainAttack = PlayerPrefs.GetInt("sainAttack3");
            sainSG = PlayerPrefs.GetInt("sainSG3");
            progress = PlayerPrefs.GetString("progress3");
        }
    }
}