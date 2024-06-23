using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TextManagerOrigin : MonoBehaviour
{
    protected List<string> _sentences = new();    //全文を格納
    protected List<string[]> _function = new();   //機能コード(呼び出す関数を表す)を格納
    protected List<string> _names = new();        //名前を格納
    protected int lineNumber = 0;                 //現在の行
    protected int displayWordNumber = 0;          //表示する文字数の管理(1文字ずつ表示する用)
    protected float readTime = 0.03f;             //文字表示スピード
    protected float timeCount = 0f;               //時間保持用
    protected string tempText;                    //表示しようとしているテキスト
    protected int textLength;                     //表示しようとしているテキストの長さ
    protected Text mainText;
    protected Text nameText;
    protected bool isAnimation = false;

    // Update is called once per frame
    protected void Update()
    {
        timeCount += Time.deltaTime;
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && _sentences.Count > lineNumber && !isAnimation)
        {
            //表示されていない部分があったら表示
            if (displayWordNumber < textLength)
            {
                TextFill();
            }
            //なければ次へ
            else
            {
                GoNextLine();
            }
        }
        //一文字ずつ表示
        if (displayWordNumber < textLength && timeCount > readTime)
        {
            //全角スラッシュがきたら改行
            if (tempText[displayWordNumber] == '／')
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

    //ページ送り
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

    //演出などの呼び出し用(シーンごとに異なる)
    protected abstract void SelectFunction(string[] s);

    //テキストを一気に表示する用
    protected void TextFill()
    {
        while (displayWordNumber < textLength)
        {
            //全角スラッシュがきたら改行
            if (tempText[displayWordNumber] == '／')
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

    //アニメーションが終了したら1行進み操作可能に
    public void AnimationFinished()
    {
        isAnimation = false;
        GoNextLine();
    }

    //ウィンドウ非表示を伴わないタイプのアニメーションによる操作停止用
    protected IEnumerator AnimationWaitSet(float f)
    {
        isAnimation = true;
        yield return new WaitForSeconds(f);
        isAnimation = false;
    } 
}