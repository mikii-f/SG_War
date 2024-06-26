using System.IO;
using UnityEngine;

public class TextManager2 : TextManagerOrigin
{
    private ImagesManager2 imagesManager;

    void Awake()
    {
        StreamReader reader = new(@"Assets/Scripts/MainScene2/Script2.txt");
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
        imagesManager = imManager.GetComponent<ImagesManager2>();
    }

    // Update is called once per frame
    protected override void SelectFunction(string[] s)
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
