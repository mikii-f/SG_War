using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystemManager : MonoBehaviour
{
    [SerializeField] private BattleSceneManagerOrigin bSManager;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject gameOverMessageObject;
    public bool GameOverMessageObject { set { gameOverMessageObject.SetActive(value); } }
    [SerializeField] private RectTransform gOYesSwitchRect;
    [SerializeField] private RectTransform gONoSwitchRect;
    [SerializeField] private GameObject functions;
    [SerializeField] private RectTransform restartSwitchRect;
    [SerializeField] private GameObject restartMessageObject;
    [SerializeField] private RectTransform restartYesSwitchRect;
    [SerializeField] private RectTransform restartNoSwitchRect;
    [SerializeField] private RectTransform retreatSwitchRect;
    [SerializeField] private GameObject retreatMessageObject;
    [SerializeField] private RectTransform retreatYesSwitchRect;
    [SerializeField] private RectTransform retreatNoSwitchRect;
    [SerializeField] private RectTransform debugSwitchRect;
    [SerializeField] private GameObject debugMessageObject;
    [SerializeField] private RectTransform debugYesSwitchRect;
    [SerializeField] private RectTransform debugNoSwitchRect;
    private bool isMessageDisplay = false;
    private bool isFunctionAvailable = true;

    // Start is called before the first frame update
    void Start()
    {
        gameOverMessageObject.SetActive(false);
        functions.SetActive(false);
        restartMessageObject.SetActive(false);
        retreatMessageObject.SetActive(false);
        debugMessageObject.SetActive(false);
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
                if (gameOverMessageObject.activeSelf)
                {
                    GORestartSwitch();
                }
                else if (restartMessageObject.activeSelf)
                {
                    RestartYesSwitch();
                }
                else if (retreatMessageObject.activeSelf)
                {
                    RetreatYesSwitch();
                }
                else if (debugMessageObject.activeSelf)
                {
                    DebugYesSwitch();
                }
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                if (gameOverMessageObject.activeSelf)
                {
                    GOGrowSwitch();
                }
                else if (restartMessageObject.activeSelf)
                {
                    RestartNoSwitch();
                }
                else if (retreatMessageObject.activeSelf)
                {
                    RetreatNoSwitch();
                }
                else if (debugMessageObject.activeSelf)
                {
                    DebugNoSwitch();
                }
            }
        }
    }

    //�Ē���{�^��(�Q�[���I�[�o�[��)
    public void GORestartSwitch()
    {
        StartCoroutine(ButtonAnim(gOYesSwitchRect));
        StartCoroutine(ReloadScene());
    }
    //�琬�Ɍ�����(�Q�[���I�[�o�[��)
    public void GOGrowSwitch()
    {
        StartCoroutine(ButtonAnim(gONoSwitchRect));
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
    //�Ē���(���j���[����I��)
    public void RestartMenuSwitch()
    {
        StartCoroutine(ButtonAnim(restartSwitchRect));
        restartMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void RestartYesSwitch()
    {
        StartCoroutine(ButtonAnim(restartYesSwitchRect));
        StartCoroutine(ReloadScene());
    }
    public void RestartNoSwitch()
    {
        StartCoroutine(ButtonAnim((restartNoSwitchRect)));
        StartCoroutine(Back(restartMessageObject));
    }
    //�P��(���j���[����I��)
    public void RetreatMenuSwitch()
    {
        StartCoroutine(ButtonAnim(retreatSwitchRect));
        retreatMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void RetreatYesSwitch()
    {
        StartCoroutine(ButtonAnim(retreatYesSwitchRect));
        //���O�̏�ʂɖ߂鏈��
    }
    public void RetreatNoSwitch()
    {
        StartCoroutine(ButtonAnim((retreatNoSwitchRect)));
        StartCoroutine(Back(retreatMessageObject));
    }
    //�f�o�b�O�p�X�L�b�v
    public void DebugMenuSwitch()
    {
        StartCoroutine(ButtonAnim(debugSwitchRect));
        debugMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void DebugYesSwitch()
    {
        StartCoroutine(ButtonAnim(debugYesSwitchRect));
        StartCoroutine(SceneSkip());
        debugMessageObject.SetActive(false);
    }
    public void DebugNoSwitch()
    {
        StartCoroutine(ButtonAnim((debugNoSwitchRect)));
        StartCoroutine(Back(debugMessageObject));
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
        restartMessageObject.SetActive(false);
        retreatMessageObject.SetActive(false);
        debugMessageObject.SetActive(false);
        isMessageDisplay = false;
        isFunctionAvailable = false;
    }
}
