using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : SystemManagerOrigin
{
    [SerializeField] private TextManagerOrigin textManager;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject functions;
    [SerializeField] private RectTransform speedUpSwitchRect;
    [SerializeField] private Image speedUpSwitchImage;
    [SerializeField] private Text speedUpSwitchText;
    [SerializeField] private RectTransform skipSwitchRect;
    [SerializeField] private RectTransform logSwitchRect;
    [SerializeField] private RectTransform growSwitchRect;
    [SerializeField] private RectTransform saveSwitchRect;
    [SerializeField] private RectTransform titleSwitchRect;
    [SerializeField] private GameObject logTextObject;
    [SerializeField] private GameObject systemMessageObject;
    [SerializeField] private Text systemMessage;
    [SerializeField] private RectTransform yesSwitch;
    [SerializeField] private RectTransform noSwitch;
    [SerializeField] private GameObject saveSuccessed;
    [SerializeField] private GameObject growMask;
    private int messageNumber;
    private bool isMessageDisplay = false;
    private bool isFunctionAvailable = false;
    private bool isGoNext = false;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        functions.SetActive(false);
        logTextObject.SetActive(false);
        systemMessageObject.SetActive(false);
        saveSuccessed.SetActive(false);
        seSource = GetComponent<AudioSource>();
        seSource.volume = GameManager.instance.SeVolume;
        //1��ڂ̈琬���s���܂ł͈琬��I���ł��Ȃ�
        if (GameManager.instance.EXP != 0)
        {
            growMask.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�L�[���͂ɂ��V�X�e������(�e�L�X�g������֎~����Ă��Ȃ��Ƃ�)(UI����͎��R�Ƃł��Ȃ��Ȃ��Ă���͂�)
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
            if (Input.GetKeyDown(KeyCode.Space) && functions.activeSelf && !isMessageDisplay)
            {
                FunctionsClose();
            }
            //�e�t�@���N�V����(�t�@���N�V�������J���Ă��āA�����b�Z�[�W���\������Ă��Ȃ��Ƃ�)
            if (functions.activeSelf && !isMessageDisplay)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    SpeedUpSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SkipSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    LogSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    GrowSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    SaveSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    TitleSwitch();
                }
            }
            //���O�����
            if (logTextObject.activeSelf && Input.GetKeyDown(KeyCode.Alpha3))
            {
                LogClose();
            }
            //���b�Z�[�W�ւ̉���
            if (Input.GetKeyDown(KeyCode.Y))
            {
                //1�񂵂������Ȃ�
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

    public void MenuSwitch()
    {
        functions.SetActive(true);
        textManager.FunctionsOpen = true;
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void SpeedUpSwitch()
    {
        StartCoroutine(ButtonAnim(speedUpSwitchRect));
        if (!textManager.IsSpeedUp)
        {
            textManager.IsSpeedUp = true;
            speedUpSwitchImage.color = Color.white;
            speedUpSwitchText.color = Color.black;
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else
        {
            textManager.IsSpeedUp = false;
            speedUpSwitchImage.color = new(0, 0, 40f / 255f, 1);
            speedUpSwitchText.color = Color.white;
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    public void SkipSwitch() 
    {
        StartCoroutine(ButtonAnim(skipSwitchRect));
        systemMessage.text = "���̃V�[�����X�L�b�v���܂����H";
        messageNumber = 0;
        StartCoroutine(Delay(systemMessageObject, true));
        isMessageDisplay = true;
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void LogSwitch() 
    {
        StartCoroutine(ButtonAnim(logSwitchRect));
        StartCoroutine(LogOnOff());
    }
    public void LogClose()
    {
        StartCoroutine(LogOnOff());
    }
    //�J���Ă������Ă��܂��Ƃ����Ȃ��悤��1�t���[���󂯂�(Update����if����������Ή����ł����������c�c)
    private IEnumerator LogOnOff()
    {
        yield return null;
        if (logTextObject.activeSelf)
        {
            logTextObject.SetActive(false);
            isMessageDisplay = false;
            seSource.clip = seUIBack;
            seSource.Play();
        }
        else
        {
            logTextObject.SetActive(true);
            isMessageDisplay = true;
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    public void GrowSwitch()
    {
        if (!growMask.activeSelf)
        {
            StartCoroutine(ButtonAnim(growSwitchRect));
            systemMessage.text = "�琬�Ɍ������܂����H";
            messageNumber = 1;
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
    public void SaveSwitch()
    {
        StartCoroutine(ButtonAnim(saveSwitchRect));
        systemMessage.text = "�i�s�x��ۑ����܂����H";
        messageNumber = 2;
        StartCoroutine(Delay(systemMessageObject, true));
        isMessageDisplay = true;
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void TitleSwitch() 
    {
        StartCoroutine(ButtonAnim(titleSwitchRect));
        systemMessage.text = "�^�C�g���ɖ߂�܂����H\n(�����ŃZ�[�u����܂�)";
        messageNumber = 3;
        StartCoroutine(Delay(systemMessageObject, true));
        isMessageDisplay = true;
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void FunctionsClose()
    {
        functions.SetActive(false);
        StartCoroutine(CloseInterval());
        seSource.clip = seUIBack;
        seSource.Play();
    }
    private IEnumerator CloseInterval() //�������_�ł̓e�L�X�g���i�܂Ȃ��悤�ɂ���
    {
        yield return null;
        textManager.FunctionsOpen = false;
    }
    public void YesSwitch()
    {
        //�J�ڂ��m�肵����Ăщ�����Ȃ��悤��
        if (!isGoNext && !switchInterval)
        {
            StartCoroutine(ButtonAnim(yesSwitch));
            seSource.clip = seUIClick;
            seSource.Play();
            switch (messageNumber)
            {
                //�X�L�b�v
                case 0:
                    isGoNext = true;
                    StartCoroutine(textManager.SceneSkip());
                    break;
                //�琬��
                case 1:
                    isGoNext = true;
                    StartCoroutine(textManager.GoToGrow());
                    break;
                //�Z�[�u
                case 2:
                    StartCoroutine(SwitchInterval());
                    textManager.Save();
                    StartCoroutine(SaveSuccessed());
                    break;
                //�^�C�g����
                case 3:
                    isGoNext = true;
                    StartCoroutine(textManager.GoBackTitle());
                    break;
                default:
                    break;
            }
        }
    }
    public void NoSwitch()
    {
        if (!isGoNext && !switchInterval)
        {
            StartCoroutine(SwitchInterval());
            StartCoroutine(ButtonAnim(noSwitch));
            StartCoroutine(Delay(systemMessageObject, false));
            isMessageDisplay = false;
            seSource.clip = seUIBack;
            seSource.Play();
        }
    }
    //�Z�[�u������`���郁�b�Z�[�W
    private IEnumerator SaveSuccessed()
    {
        yield return new WaitForSeconds(0.1f);
        systemMessageObject.SetActive(false);
        isMessageDisplay= false;
        saveSuccessed.SetActive(true);
        yield return new WaitForSeconds(3);
        saveSuccessed.SetActive(false);
    }

    //�e�L�X�g�����烁�j���[�̕\����\��
    public void MenuOn()
    {
        menu.SetActive(true);
        isFunctionAvailable = true;
    }
    public void MenuOff()
    {
        menu.SetActive(false);
        functions.SetActive(false);
        logTextObject.SetActive(false);
        systemMessageObject.SetActive(false);
        isMessageDisplay = false;
        isFunctionAvailable = false;
    }
}
