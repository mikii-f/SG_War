using UnityEngine;

public class GameManager : MonoBehaviour
{
    //�����ɂ����Βl��null�ɂȂ��Ė�肪�N���邱�Ƃ͂Ȃ��͂������A�������ɂ��Ă͐[���l����ׂ��Ȃ̂��ǂ���
    public static GameManager instance;
    private static string sceneName;
    public string SceneName { set { sceneName = value; } get { return sceneName; } }
    private static int lineNumber;
    public int LineNumber { set { lineNumber = value; } get { return lineNumber; } }
    //���̂����X�e�[�^�X��
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
