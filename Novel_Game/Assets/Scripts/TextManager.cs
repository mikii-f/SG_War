using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private List<string> _sentences = new();    //�S�����i�[
    private List<int> _function = new();        //�@�\�R�[�h(�Ăяo���֐���\��)���i�[
    private List<string> _names = new();        //���O���i�[
    private int lineNumber = 0;                 //���݂̍s
    private int displayWordNumber = 0;          //�\�����镶�����̊Ǘ�(1�������\������p)
    private float readTime = 0.03f;             //�����\���X�s�[�h
    private float timeCount = 0f;               //���ԕێ��p
    private string tempText;                    //�\�����悤�Ƃ��Ă���e�L�X�g
    private int textLength;                     //�\�����悤�Ƃ��Ă���e�L�X�g�̒���
    private Text _text;
    public Text nameText;
    public GameObject imManager;
    private ImagesManager imagesManager;
    private bool isAnimation = true; 

    private void Awake()
    {
        StreamReader reader = new(@"Assets/Scripts/Script.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(int.Parse(reader.ReadLine()));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        
        imagesManager = imManager.GetComponent<ImagesManager>();
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
                SelectFunction(_function[lineNumber]);
                tempText = _sentences[lineNumber];
                textLength = tempText.Length;
                _text.text = "";
                nameText.text = _names[lineNumber];
                displayWordNumber = 0;
                lineNumber++;
            }
        }
        //�ꕶ�����\��
        if (displayWordNumber < textLength && timeCount > readTime)
        {
            //�S�p�X���b�V������������s
            if (tempText[displayWordNumber] == '�^')
            {
                _text.text += '\n';
                displayWordNumber++;
                timeCount = 0f;
            }
            else
            {
                _text.text += tempText[displayWordNumber];
                displayWordNumber++;
                timeCount = 0f;
            }
        }
    }

    //�e�L�X�g�ɋL�q�����@�\�R�[�h�ɉ����Ċ֐��Ăяo��
    private void SelectFunction(int n)
    {
        switch (n){
            case 0:
                break;
            case 1:
                StartCoroutine(imagesManager.BlackHalfOpen());
                break;
            case 2:
                StartCoroutine(imagesManager.BlackHalfToWhite());
                break;
            case 10:
                imagesManager.CharacterHide();
                break;
            case 21:
                imagesManager.BackGroundChange(1);
                break;
            case 22:
                imagesManager.BackGroundChange(2);
                break;
            default:
                break;
        }
    }

    //�e�L�X�g����C�ɕ\������p
    private void TextFill()
    {
        while (displayWordNumber < textLength)
        {
            //�S�p�X���b�V������������s
            if (tempText[displayWordNumber] == '�^')
            {
                _text.text += '\n';
                displayWordNumber++;
            }
            else
            {
                _text.text += tempText[displayWordNumber];
                displayWordNumber++;
            }
        }
    }

    public void AnimationFinished()
    {
        isAnimation = false;
    }
}
