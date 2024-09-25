using UnityEngine;

public class Stage2Manager : Stage1Manager
{
    protected override void ScoreText()
    {
        score = Mathf.Min((int)(4000 * medalCount / 100 * 60 / time), 4000);
        if (medalCount == 100)
        {
            scoreText.text = "メダル獲得：" + medalCount.ToString() + "枚\n\nタイム：" + time.ToString("F2") + "s\n\nメダルコンプリート\nスコア：" + score.ToString() + "+1000\n累計EXP：" + (GameManager.instance.EXP + score + 1000).ToString();
            score += 1000;
        }
        else
        {
            scoreText.text = "メダル獲得：" + medalCount.ToString() + "枚\n\nタイム：" + time.ToString("F2") + "s\n\n\nスコア：" + score.ToString() + "\n累計EXP：" + (GameManager.instance.EXP + score).ToString();
        }
    }
}
