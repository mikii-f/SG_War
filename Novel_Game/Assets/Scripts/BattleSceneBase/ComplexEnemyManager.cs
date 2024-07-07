using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComplexEnemyManager : EnemyManagerOrigin
{
    [SerializeField] private GameObject gage1;
    [SerializeField] private GameObject gage2;
    private Image gage1Image;
    private Image gage2Image;

    // Start is called before the first frame update
    protected override void StartSet()
    {
        gage1Image = gage1.GetComponent<Image>();
        gage2Image = gage2.GetComponent<Image>();
        maxGage = 2;
        currentHP = maxHP;
        currentGage = 0;
        interval = 6;
        intervalCount = interval;
    }

    //通常攻撃
    protected override IEnumerator NormalAttack()
    {
        StartCoroutine(AttackSubtitle("蠢いている"));
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
    //チャージ技
    protected override IEnumerator ChargeAttack()
    {
        StartCoroutine(AttackSubtitle("増殖再生"));
        isAttack = true;
        bSManager.ReviveEnemy();
        yield return new WaitForSeconds(1);
        isAttack = false;
        gage1Image.sprite = grayGage;
        gage2Image.sprite = grayGage;
    }

    public override void Revive()
    {
        //現状復活させる予定なし
    }
}
