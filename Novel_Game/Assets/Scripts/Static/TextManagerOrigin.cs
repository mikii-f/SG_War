using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TextManagerOrigin : MonoBehaviour
{
    [SerializeField] protected GameObject imManager;
    protected List<string> _sentences = new();    //�S�����i�[
    protected List<string[]> _function = new();   //�@�\�R�[�h(�Ăяo���֐���\��)���i�[
    protected List<string> _names = new();        //���O���i�[
    protected int lineNumber = 0;                 //���݂̍s
    protected int displayWordNumber = 0;          //�\�����镶�����̊Ǘ�(1�������\������p)
    protected float readTime = 0.03f;             //�����\���X�s�[�h
    protected float timeCount = 0f;               //���ԕێ��p
    protected string tempText;                    //�\�����悤�Ƃ��Ă���e�L�X�g
    protected int textLength;                     //�\�����悤�Ƃ��Ă���e�L�X�g�̒���
    [SerializeField] private GameObject mText;
    [SerializeField] private GameObject nText;
    protected Text mainText;
    protected Text nameText;
    protected bool isAnimation = false;

    void Start()
    {
        mainText = mText.GetComponent<Text>();
        nameText = nText.GetComponent<Text>();
        GoNextLine();
        StartSet();
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
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
        //�ꕶ�����\��
        if (displayWordNumber < textLength && timeCount > readTime)
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
    }

    //�y�[�W����
    protected void GoNextLine()
    {
        SelectFunction(_function[lineNumber]);
        tempText = _sentences[lineNumber];
        textLength = tempText.Length;
        mainText.text = "";
        nameText.text = _names[lineNumber];
        displayWordNumber = 0;
        lineNumber++;
    }

    //���o�Ȃǂ̌Ăяo���p(�V�[�����ƂɈقȂ�)
    protected abstract void SelectFunction(string[] s);

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
        GoNextLine();
    }

    //�E�B���h�E��\���𔺂�Ȃ��^�C�v�̃A�j���[�V�����ɂ�鑀���~�p
    protected IEnumerator AnimationWaitSet(float f)
    {
        isAnimation = true;
        yield return new WaitForSeconds(f);
        isAnimation = false;
    }

    //�e�N���X�g���Ƃ̃X�^�[�g������
    protected abstract void StartSet();
}