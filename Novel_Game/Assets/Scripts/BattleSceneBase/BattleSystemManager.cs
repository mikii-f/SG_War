using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSystemManager : SystemManagerOrigin
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
    [SerializeField] private GameObject growMask1;
    [SerializeField] private GameObject growMask2;
    [SerializeField] private Image black;
    private int messageNumber;
    private bool isMessageDisplay = false;
    private bool isFunctionAvailable = true;
    private bool isSelected = false;

    void Start()
    {
        functions.SetActive(false);
        systemMessageObject.SetActive(false);
        seSource = GetComponent<AudioSource>();
        seSource.volume = GameManager.instance.SeVolume;
        //1��ڂ̈琬���s���܂ł͈琬��I���ł��Ȃ�
        if (GameManager.instance.EXP != 0)
        {
            growMask1.SetActive(false);
        }
        growMask2.SetActive(false);
    }

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
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void FunctionsClose()
    {
        functions.SetActive(false);
        seSource.clip = seUIBack;
        seSource.Play();
    }
    //�Q�[���I�[�o�[(�o�g���V�[������Ăяo��)
    public void GameOver()
    {
        if (GameManager.instance.EXP == 0)
        {
            growMask2.SetActive(true);
            systemMessage.text = "GAME OVER\n�Ē��킵�܂����H";
        }
        else
        {
            systemMessage.text = "GAME OVER\n�Ē��킵�܂����H\n���O�̏�ʂɖ߂�琬���܂����H";
        }
        isFunctionAvailable = true;
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
        StartCoroutine(Delay(systemMessageObject, true));
        isMessageDisplay = true;
        seSource.clip = seUIClick;
        seSource.Play();
    }
    //�P��(���j���[����I��)
    public void RetreatMenuSwitch()
    {
        if (!growMask1.activeSelf)
        {
            StartCoroutine(ButtonAnim(retreatSwitchRect));
            systemMessage.text = "�P�ނ��܂����H\n(���O�̏�ʂɈڂ�琬���s���܂�)";
            yesText.text = "�P��(Y)";
            noText.text = "�߂�(N)";
            messageNumber = 2;
            StartCoroutine(Delay(systemMessageObject, true));
            isMessageDisplay = true;
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    //�f�o�b�O�p�X�L�b�v
    public void DebugMenuSwitch()
    {
        StartCoroutine(ButtonAnim(debugSwitchRect));
        systemMessage.text = "�퓬���X�L�b�v���܂����H\n(�Q�[����]�����Ă�������������̋@�\�ł��I�I)";
        yesText.text = "�X�L�b�v(Y)";
        noText.text = "�߂�(N)";
        messageNumber = 3;
        StartCoroutine(Delay(systemMessageObject, true));
        isMessageDisplay = true;
        seSource.clip = seUIClick;
        seSource.Play();
    }

    //Yes�{�^�����������Ƃ��̋@�\
    public void YesSwitch()
    {
        if (!isSelected && !switchInterval)
        {
            isSelected = true;
            StartCoroutine(ButtonAnim(yesSwitch));
            seSource.clip = seUIClick;
            seSource.Play();
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
                    break;
                default:
                    break;
            }
        }
    }
    //No�{�^�����������Ƃ��̋@�\
    public void NoSwitch()
    {
        if (!isSelected && !switchInterval)
        {
            StartCoroutine(SwitchInterval());
            seSource.clip = seUIBack;
            seSource.Play();
            switch (messageNumber)
            {
                //�Q�[���I�[�o�[����琬��
                case 0:
                    if (!growMask2.activeSelf)
                    {
                        StartCoroutine(ButtonAnim(noSwitch));
                        StartCoroutine(GoBackStory());
                        isSelected = true;
                    }
                    else
                    {
                        seSource.clip = seUIUnactive;
                        seSource.Play();
                    }
                    break;
                //�߂�
                default:
                    StartCoroutine(ButtonAnim(noSwitch));
                    StartCoroutine(Delay(systemMessageObject, false));
                    isMessageDisplay = false;
                    break;
            }
        }
    }

    //�����҂��čēǂݍ���
    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //�����҂��ăX�L�b�v
    private IEnumerator SceneSkip()
    {
        yield return new WaitForSeconds(0.1f);
        bSManager.SceneLoad();
    }
    //�����҂��Ē��O�̏�ʂ�
    private IEnumerator GoBackStory()
    {
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(1, black));
        if (GameManager.instance.SceneName == null)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(GameManager.instance.SceneName);
        }
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
