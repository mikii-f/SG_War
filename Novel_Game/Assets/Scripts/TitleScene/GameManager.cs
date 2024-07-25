using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�����ɂ����Βl��null�ɂȂ��Ė�肪�N���邱�Ƃ͂Ȃ��͂������A�������ɂ��Ă͐[���l����ׂ��Ȃ̂��ǂ���
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
    //���̂����X�e�[�^�X��
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;           //60FPS�Œ�
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
    
    //�Z�[�u�f�[�^�폜(�Z�[�u�f�[�^�𕡐��ɂ����ꍇ�͑I���ł���悤�ɂ���)
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

    //�Z�[�u�f�[�^�ƈꎞ�L���̓���
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
