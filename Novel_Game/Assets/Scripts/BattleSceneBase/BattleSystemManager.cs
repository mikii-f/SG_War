using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSystemManager : MonoBehaviour
{
    [SerializeField] private BattleSceneManagerOrigin bSManager;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject functions;
    [SerializeField] private RectTransform restartSwitchRect;
    [SerializeField] private RectTransform retreatSwitchRect;
    [SerializeField] private RectTransform debugSwitchRect;
    [SerializeField] private GameObject systemMessageObject;
    [SerializeField] private Text systemMessage;
    [SerializeField] private RectTransform yesSwitch;
    [SerializeField] private RectTransform noSwitch;
    [SerializeField] private Text yesText;
    [SerializeField] private Text noText;
    private int messageNumber;
    private bool isMessageDisplay = false;
    private bool isFunctionAvailable = true;

    // Start is called before the first frame update
    void Start()
    {
        functions.SetActive(false);
        systemMessageObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isFunctionAvailable)
        {
            //���j���[�I���I�t
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (!functions.activeSelf)
                {
                    MenuSwitch();
                }
                else if (!isMessageDisplay)
                {
                    FunctionsClose();
                }
            }
            //�e�t�@���N�V����(�t�@���N�V�������J���Ă��āA�����b�Z�[�W���\������Ă��Ȃ��Ƃ�)
            if (functions.activeSelf && !isMessageDisplay)
            {
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    RestartMenuSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    RetreatMenuSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    DebugMenuSwitch();
                }
            }
            //���b�Z�[�W�ւ̉���
            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (systemMessageObject.activeSelf)
                {
                    YesSwitch();
                }
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                if (systemMessageObject.activeSelf)
                {
                    NoSwitch();
                }
            }
        }
    }

    //���j���[�J��
    public void MenuSwitch()
    {
        functions.SetActive(true);
    }
    public void FunctionsClose()
    {
        functions.SetActive(false);
    }
    //�Q�[���I�[�o�[(�o�g���V�[������Ăяo��)
    public void GameOver()
    {
        isFunctionAvailable = true;
        systemMessage.text = "GAME OVER\n�Ē��킵�܂����H";
        yesText.text = "�Ē���(Y)";
        noText.text = "�琬��(N)";
        messageNumber = 0;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    //�Ē���(���j���[����I��)
    public void RestartMenuSwitch()
    {
        StartCoroutine(ButtonAnim(restartSwitchRect));
        systemMessage.text = "�퓬���n�߂����蒼���܂����H";
        yesText.text = "�Ē���(Y)";
        noText.text = "�߂�(N)";
        messageNumber = 1;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    //�P��(���j���[����I��)
    public void RetreatMenuSwitch()
    {
        StartCoroutine(ButtonAnim(retreatSwitchRect));
        systemMessage.text = "�P�ނ��܂����H\n(���O�̏�ʂɈڂ�琬���s���܂�)";
        yesText.text = "�P��(Y)";
        noText.text = "�߂�(N)";
        messageNumber = 2;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    //�f�o�b�O�p�X�L�b�v
    public void DebugMenuSwitch()
    {
        StartCoroutine(ButtonAnim(debugSwitchRect));
        systemMessage.text = "�퓬���X�L�b�v���܂����H\n(�Q�[����]�����Ă�������������̋@�\�ł��I�I)";
        yesText.text = "�X�L�b�v(Y)";
        noText.text = "�߂�(N)";
        messageNumber = 3;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }

    //Yes�{�^�����������Ƃ��̋@�\
    public void YesSwitch()
    {
        StartCoroutine(ButtonAnim(yesSwitch));
        switch (messageNumber)
        {
            //�Q�[���I�[�o�[����̍Ē���
            case 0:
                StartCoroutine(ReloadScene());
                break;
            //���j���[����̍Ē���
            case 1:
                StartCoroutine(ReloadScene());
                break;
            //�P�ނ����O�̏�ʂɖ߂�
            case 2:
                StartCoroutine(GoBackStory());
                break;
            //�f�o�b�O�p�X�L�b�v
            case 3:
                StartCoroutine(SceneSkip());
                systemMessageObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    //No�{�^�����������Ƃ��̋@�\
    public void NoSwitch()
    {
        StartCoroutine(ButtonAnim(noSwitch));
        switch (messageNumber)
        {
            //�Q�[���I�[�o�[����琬��
            case 0:
                StartCoroutine(GoBackStory());
                break;
            //�߂�
            default:
                StartCoroutine(Back(systemMessageObject));
                break;
        }
    }

    //�����҂��čēǂݍ���
    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //�����҂��ĕ���
    private IEnumerator Back(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.15f);
        gameObject.SetActive(false);
        isMessageDisplay = false;
    }
    //�����҂��ăX�L�b�v
    private IEnumerator SceneSkip()
    {
        yield return new WaitForSeconds(0.15f);
        bSManager.SceneLoad();
    }
    //�����҂��Ē��O�̏�ʂ�
    private IEnumerator GoBackStory()
    {
        yield return new WaitForSeconds(0.15f);
        if (GameManager.instance.SceneName == null)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(GameManager.instance.SceneName);
        }
    }

    //�{�^���̃A�j���[�V����
    private IEnumerator ButtonAnim(RectTransform rect)
    {
        Vector2 temp = rect.localScale;
        rect.localScale = new(0.9f * temp.x, 0.9f * temp.y);
        yield return new WaitForSeconds(0.1f);
        rect.localScale = temp;
    }

    public void MenuOn()
    {
        menu.SetActive(true);
        isFunctionAvailable = true;
    }
    public void MenuOff()
    {
        menu.SetActive(false);
        functions.SetActive(false);
        systemMessageObject.SetActive(false);
        isMessageDisplay = false;
        isFunctionAvailable = false;
    }
}
