using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TextManagerOrigin : MonoBehaviour
{
    [SerializeField] protected ImagesManagerOrigin imagesManager;
    [SerializeField] protected SystemManager systemManager;
    protected List<string> _sentences = new();    //�S�����i�[
    protected List<string[]> _function = new();   //�@�\�R�[�h(�Ăяo���֐���\��)���i�[
    protected List<string> _names = new();        //���O���i�[
    private int lineNumber = 0;                 //���݂̍s
    private int displayWordNumber = 0;          //�\�����镶�����̊Ǘ�(1�������\������p)
    private float readTime = 0.03f;             //�����\���X�s�[�h
    private float timeCount = 0f;               //���ԕێ��p
    private float waitTime = 1f;                  //�����Đ����̑҂�����
    private string tempText;                    //�\�����悤�Ƃ��Ă���e�L�X�g
    private int textLength;                     //�\�����悤�Ƃ��Ă���e�L�X�g�̒���
    [SerializeField] private Text mainText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text logText;
    protected bool isAnimation = false;
    private bool functionsOpen = false;
    public bool FunctionsOpen { set { functionsOpen = value; } }
    private bool isSpeedUp = false;
    public bool IsSpeedUp { set { isSpeedUp = value; TextFill(); } get { return isSpeedUp; } }
    private Coroutine slideCoroutine;

    void Start()
    {
        GoNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        //�t�@���N�V�������J���Ă���Ƃ�����ю����Đ����͔������Ȃ�
        if (!functionsOpen && !isSpeedUp)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && _sentences.Count > lineNumber && !isAnimation)
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
                        case "el":
                            imagesManager.CharacterChange(2);
                            break;
                        case "Ghost1":
                            imagesManager.CharacterChange(11);
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
                    StopCoroutine(slideCoroutine);
                    imagesManager.SlideStop();
                    break;
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
                case "ChangeScene":
                    i++;
                    imagesManager.ChangeScene(s[i]);
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

    //�E�B���h�E��\���𔺂�Ȃ��^�C�v�̃A�j���[�V�����ɂ�鑀���~�p
    protected IEnumerator AnimationWaitSet(float f)
    {
        isAnimation = true;
        yield return new WaitForSeconds(f);
        isAnimation = false;
    }

    //�V�[���X�L�b�v
    public IEnumerator SceneSkip()
    {
        yield return new WaitForSeconds(0.15f);
        SelectFunction(_function[_sentences.Count-1]);
    }
}