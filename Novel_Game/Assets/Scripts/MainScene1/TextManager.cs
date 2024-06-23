using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : TextManagerOrigin
{
    public GameObject imManager;
    private ImagesManager imagesManager;
    private Coroutine slideCoroutine;

    private void Awake()
    {
        StreamReader reader = new(@"Assets/Scripts/MainScene1/Script.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(reader.ReadLine().Split(','));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mainText = GameObject.Find("Text").GetComponent<Text>();
        nameText = GameObject.Find("Name").GetComponent<Text>();
        imagesManager = imManager.GetComponent<ImagesManager>();
        GoNextLine();
    }

    //テキストに記述した機能コードに応じて関数呼び出し
    protected override void SelectFunction(string[] s)
    {
        int n = s.Length;
        for (int i=0; i<n; i++)
        {
            switch (s[i])
            {
                case "0":
                    break;
                case "BlackOnOff":
                    imagesManager.BlackOnOff();
                    break;
                case "BlackHalfOpen":
                    StartCoroutine(AnimationWaitSet(1f));
                    StartCoroutine(imagesManager.BlackHalfOpen());
                    break;
                case "BlackHalfToWhite":
                    StartCoroutine(AnimationWaitSet(3f));
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
                        case "Buildings":
                            imagesManager.BackgroundChange(3);
                            break;
                        case "City":
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
                    StopCoroutine(slideCoroutine);
                    imagesManager.SlideStop();
                    break;
                case "BackgroundReset":
                    imagesManager.BackgroundReset();
                    break;
                case "TitleCoal":
                    isAnimation = true;
                    StartCoroutine(imagesManager.TitleAnimation());
                    break;
                default:
                    break;
            }
        }
    }
}
