using System.IO;
using UnityEngine;

public class TextManager : TextManagerOrigin
{
    private ImagesManager imagesManager;
    private Coroutine slideCoroutine;

    void Awake()
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
    protected override void StartSet()
    {
        imagesManager = imManager.GetComponent<ImagesManager>();
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
                case "FadeOut":
                    isAnimation = true;
                    i++;
                    float fadeTime = float.Parse(s[i]);
                    i++;
                    imagesManager.FadeOutReceiver(fadeTime, s[i]);
                    break;
                case "BlackOnOff":
                    i++;
                    if (s[i] == "On")
                    {
                        imagesManager.BlackOnOff(true);
                    }
                    else if (s[i] == "Off")
                    {
                        imagesManager.BlackOnOff(false);
                    }
                    break;
                case "TextPanelOnOff":
                    i++;
                    if (s[i] == "On")
                    {
                        imagesManager.TextPanelOnOff(true);
                    }
                    else if (s[i] == "Off")
                    {
                        imagesManager.TextPanelOnOff(false);
                    }
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
                case "BackgroundReset":
                    imagesManager.BackgroundReset();
                    break;
                case "BlackReset":
                    imagesManager.BlackReset();
                    break;
                case "AnimAndGoNext":
                    i++;
                    StartCoroutine(AnimationFinished(float.Parse(s[i])));
                    break;
                case "AnimationWaitSet":
                    i++;
                    StartCoroutine(AnimationWaitSet(float.Parse(s[i])));
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
}
