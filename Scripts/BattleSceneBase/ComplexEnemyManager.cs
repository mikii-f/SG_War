using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComplexEnemyManager : EnemyManagerOrigin
{
    [SerializeField] private Image gage1Image;
    [SerializeField] private Image gage2Image;

    protected override void StartSet()
    {
        id = 1;
        maxGage = 2;
        currentHP = maxHP;
        currentGage = 0;
        interval = 6;
        intervalCount = interval;
        intervalText.text = intervalCount.ToString("F2");
    }

    //�ʏ�U��
    protected override IEnumerator NormalAttack()
    {
        StartCoroutine(AttackSubtitle("忂��Ă���"));
        isAttack = true;
        yield return new WaitForSeconds(1);
        isAttack = false;
        switch (currentGage)
        {
            case 1:
                gage1Image.sprite = redGage;
                break;
            case 2:
                gage2Image.sprite = redGage;
                break;
            default:
                break;
        }
    }
    //�`���[�W�Z
    protected override IEnumerator ChargeAttack()
    {
        StartCoroutine(AttackSubtitle("���B�Đ�"));
        isAttack = true;
        bSManager.ReviveEnemy();
        yield return new WaitForSeconds(1);
        isAttack = false;
        gage1Image.sprite = grayGage;
        gage2Image.sprite = grayGage;
    }

    public override void Revive()
    {
        //���󕜊�������\��Ȃ�
    }
}
