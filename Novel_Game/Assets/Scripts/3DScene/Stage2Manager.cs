using UnityEngine;

public class Stage2Manager : Stage1Manager
{
    protected override void ScoreText()
    {
        score = Mathf.Min((int)(4000 * medalCount / 100 * 60 / time), 4000);
        if (medalCount == 100)
        {
            scoreText.text = "���_���l���F" + medalCount.ToString() + "��\n\n�^�C���F" + time.ToString("F2") + "s\n\n���_���R���v���[�g\n�X�R�A�F" + score.ToString() + "+1000\n�݌vEXP�F" + (GameManager.instance.EXP + score + 1000).ToString();
            score += 1000;
        }
        else
        {
            scoreText.text = "���_���l���F" + medalCount.ToString() + "��\n\n�^�C���F" + time.ToString("F2") + "s\n\n\n�X�R�A�F" + score.ToString() + "\n�݌vEXP�F" + (GameManager.instance.EXP + score).ToString();
        }
    }
}
