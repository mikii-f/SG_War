using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class TextManagerOrigin : MonoBehaviour
{
    [SerializeField] protected ImagesManagerOrigin imagesManager;
    [SerializeField] protected SystemManager systemManager;
    protected List<string> _sentences = new();    //�S�����i�[
    protected List<string[]> _function = new();   //�@�\�R�[�h(�Ăяo���֐���\��)���i�[
    protected List<string> _names = new();        //���O���i�[
    private int lineNumber = 0;                   //���݂̍s
    private int displayWordNumber = 0;            //�\�����镶�����̊Ǘ�(1�������\������p)
    private float readTime = 0.03f;               //�����\���X�s�[�h
    private float timeCount = 0f;                 //���ԕێ��p
    private float waitTime = 1f;                  //�����Đ����̑҂�����  (���̕ӂ�̓R���t�B�O���炢�����悤�ɂ�����)
    private string tempText;                      //�\�����悤�Ƃ��Ă���e�L�X�g
    private int textLength;                       //�\�����悤�Ƃ��Ă���e�L�X�g�̒���
    [SerializeField] private Text mainText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text logText;
    private bool isAnimation = false;
    private bool functionsOpen = false;
    public bool FunctionsOpen { set { functionsOpen = value; } }
    private bool isSpeedUp = false;
    public bool IsSpeedUp { set { isSpeedUp = value; TextFill(); } get { return isSpeedUp; } }
    private Coroutine slideCoroutine;
    private bool skip = false;  //�Z�[�u�f�[�^�Ȃǂ��畜�A����ۂɔC�ӂ̍s����n�߂�p

    void Start()
    {
        StartCoroutine(SaveDataCheck());
    }
    private IEnumerator SaveDataCheck()
    {
        yield return null;
        //�w�肳�ꂽ�s�܂Ői��
        if (GameManager.instance.LineNumber != 0)
        {
            skip = true;
            imagesManager.Skip = true;
            for (int i = 0; i < GameManager.instance.LineNumber - 1; i++)
            {
                GoNextLine();
                TextFill();
            }
            skip = false;
            isAnimation = false;    //�֐����g�킸�ɂ����炩��A�j���[�V�����I���ɂ��邱�Ƃ����邽��(���P����)
            imagesManager.Skip = false;
        }
        GoNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (!skip)
        {
            timeCount += Time.deltaTime;
            //�t�@���N�V�������J���Ă���Ƃ�����ю����Đ����͔������Ȃ�
            if (!functionsOpen && !isSpeedUp)
            {
                if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && lineNumber < _sentences.Count && !isAnimation)
                {
                    //�\������Ă��Ȃ���������������\��
                    if (displayWordNumber < textLength)
                    {
                        TextFill();
                    }
                    //�Ȃ���Ύ���
                    else
                    {
                        GoNextLine();
                    }
                }
            }
            //�ꕶ�����\��
            if (displayWordNumber < textLength && timeCount > readTime && !isSpeedUp)
            {
                //�S�p�X���b�V������������s
                if (tempText[displayWordNumber] == '�^')
                {
                    mainText.text += '\n';
                    displayWordNumber++;
                    timeCount = 0f;
                }
                else
                {
                    mainText.text += tempText[displayWordNumber];
                    displayWordNumber++;
                    timeCount = 0f;
                }
            }
            //�����Đ���
            if (isSpeedUp)
            {
                if (_sentences.Count > lineNumber && !isAnimation && timeCount > waitTime)
                {
                    GoNextLine();
                    TextFill();
                    timeCount = 0f;
                }
            }
        }
    }

    //�y�[�W����
    protected void GoNextLine()
    {
        if (nameText.text != "")
        {
            logText.text += nameText.text + "\n";
        }
        if (mainText.text != "")
        {
            logText.text += mainText.text + "\n";
        }
        SelectFunction(_function[lineNumber]);
        tempText = _sentences[lineNumber];
        textLength = tempText.Length;
        mainText.text = "";
        nameText.text = _names[lineNumber];
        displayWordNumber = 0;
        lineNumber++;
    }

    //�e�L�X�g�ɋL�q�����@�\�R�[�h�ɉ����Ċ֐��Ăяo��
    private void SelectFunction(string[] s)
    {
        int n = s.Length;
        for (int i = 0; i < n; i++)
        {
            switch (s[i])
            {
                case "0":
                    break;
                case "FadeOut":
                    isAnimation = true;
                    i++;
                    float fadeTime = float.Parse(s[i]);
                    i++;
                    imagesManager.FadeOutReceiver(fadeTime, s[i]);
                    break;
                case "FadeIn":
                    isAnimation = true;
                    i++;
                    fadeTime = float.Parse(s[i]);
                    i++;
                    imagesManager.FadeInReceiver(fadeTime, s[i]);
                    break;
                case "BlackOn":
                    imagesManager.BlackOn();
                    break;
                case "BlackOff":
                    imagesManager.BlackOff();
                    break;
                case "BlackOUOn":
                    imagesManager.BlackOUOn();
                    break;
                case "BlackOUOff":
                    imagesManager.BlackOUOff();
                    break;
                case "TextPanelOn":
                    imagesManager.TextPanelOn();
                    break;
                case "TextPanelOff":
                    imagesManager.TextPanelOff();
                    break;
                case "BlackHalfOpen":
                    StartCoroutine(imagesManager.BlackHalfOpen());
                    break;
                case "BlackHalfToWhite":
                    StartCoroutine(imagesManager.BlackHalfToWhite());
                    break;
                case "CharacterChange":
                    i++;
                    switch (s[i])
                    {
                        case "transparent":
                            imagesManager.CharacterChange(0);
                            break;
                        case "vier":
                            imagesManager.CharacterChange(1);
                            break;
                        case "vier_battle":
                            imagesManager.CharacterChange(21);
                            break;
                        case "el":
                            imagesManager.CharacterChange(51);
                            break;
                        case "el_battle":
                            imagesManager.CharacterChange(71);
                            break;
                        case "el_enemy":
                            imagesManager.CharacterChange(91);
                            break;
                        case "Ghost1":
                            imagesManager.CharacterChange(101);
                            break;
                        default:
                            break;
                    }
                    break;
                case "BackgroundChange":
                    i++;
                    switch (s[i])
                    {
                        case "Black":
                            imagesManager.BackgroundChange(0);
                            break;
                        case "MyRoom":
                            imagesManager.BackgroundChange(1);
                            break;
                        case "Road":
                            imagesManager.BackgroundChange(2);
                            break;
                        case "City":
                            imagesManager.BackgroundChange(3);
                            break;
                        case "Rooftop":
                            imagesManager.BackgroundChange(4);
                            break;
                        default:
                            break;
                    }
                    break;
                case "Wipe1":
                    isAnimation = true;
                    StartCoroutine(imagesManager.Wipe1());
                    break;
                case "Wipe2":
                    isAnimation = true;
                    StartCoroutine(imagesManager.Wipe2());
                    break;
                case "BackgroundSlide":
                    slideCoroutine = StartCoroutine(imagesManager.BackgroundSlide());
                    break;
                case "SlideStop":
                    if (slideCoroutine != null)
                    {
                        StopCoroutine(slideCoroutine);
                        imagesManager.SlideStop();
                    }                    break;
                case "CharacterReset":
                    imagesManager.CharacterReset();
                    break;
                case "BackgroundReset":
                    imagesManager.BackgroundReset();
                    break;
                case "AnimAndGoNext":
                    i++;
                    StartCoroutine(AnimationFinished(float.Parse(s[i])));
                    break;
                case "AnimationWaitSet":
                    i++;
                    StartCoroutine(AnimationWaitSet(float.Parse(s[i])));
                    break;
                case "MenuOn":
                    systemManager.MenuOn();
                    break;
                case "MenuOff":
                    systemManager.MenuOff();
                    break;
                case "TitleCoal":
                    isAnimation = true;
                    StartCoroutine(imagesManager.TitleAnimation());
                    break;
                case "Save":
                    Save();
                    break;
                case "ChangeScene":
                    imagesManager.ChangeScene();
                    break;
                default:
                    break;
            }
        }
    }

    //�e�L�X�g����C�ɕ\������p
    protected void TextFill()
    {
        while (displayWordNumber < textLength)
        {
            //�S�p�X���b�V������������s
            if (tempText[displayWordNumber] == '�^')
            {
                mainText.text += '\n';
                displayWordNumber++;
            }
            else
            {
                mainText.text += tempText[displayWordNumber];
                displayWordNumber++;
            }
        }
    }

    //�A�j���[�V�������I��������C�ӂ̎��ԑ҂�����1�s�i�ݑ���\��
    public IEnumerator AnimationFinished(float f)
    {
        //�X�L�b�v���͖���
        if (!skip)
        {
            isAnimation = true;
            yield return new WaitForSeconds(f);
            isAnimation = false;
            timeCount = 0;
            GoNextLine();
            if (isSpeedUp)
            {
                TextFill();
            }
        }
    }

    //�E�B���h�E��\���𔺂�Ȃ��^�C�v�̃A�j���[�V�����ɂ�鑀���~�p
    protected IEnumerator AnimationWaitSet(float f)
    {
        if (!skip)
        {
            isAnimation = true;
            yield return new WaitForSeconds(f);
            isAnimation = false;
        }
    }

    //�V�[���X�L�b�v
    public IEnumerator SceneSkip()
    {
        yield return new WaitForSeconds(0.15f);
        skip = true;
        imagesManager.Skip = true;
        while (lineNumber < _sentences.Count)
        {
            GoNextLine();
            TextFill();
        }
    }
    //�琬�Ɍ�����
    public IEnumerator GoToGrow()
    {
        yield return new WaitForSeconds(0.15f);
        GameManager.instance.SceneName = SceneManager.GetActiveScene().name;
        GameManager.instance.LineNumber = lineNumber;
        GameManager.instance.Save();
        imagesManager.FadeOutReceiver(1, "Black");
        imagesManager.TextPanelOff();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("3DGameSelectScene");
    }
    //�Z�[�u����
    public void Save()
    {
        GameManager.instance.SceneName = SceneManager.GetActiveScene().name;
        GameManager.instance.LineNumber = lineNumber;
        GameManager.instance.Save();
    }
    //�^�C�g����
    public IEnumerator GoBackTitle()
    {
        yield return new WaitForSeconds(0.15f);
        GameManager.instance.SceneName = SceneManager.GetActiveScene().name;
        GameManager.instance.LineNumber = lineNumber;
        GameManager.instance.Save();
        imagesManager.FadeOutReceiver(1, "Black");
        imagesManager.TextPanelOff();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("TitleScene");
    }
}