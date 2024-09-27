using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarnivoreEnemyManager : EnemyManagerOrigin
{
    [SerializeField] private Image gage1Image;
    [SerializeField] private GameObject attackEffect;
    private RectTransform attackRect;
    private Image attackImage;
    [SerializeField] private GameObject attackEffect2;
    private RectTransform attackRect2;
    private Image attackImage2;

    protected override void StartSet()
    {
        id = 2;
        attackRect = attackEffect.GetComponent<RectTransform>();
        attackImage = attackEffect.GetComponent<Image>();
        attackImage.color = new(1, 1, 1, 0);
        attackRect2 = attackEffect2.GetComponent<RectTransform>();
        attackImage2 = attackEffect2.GetComponent<Image>();
        attackImage2.color = new(1, 1, 1, 0);
        maxGage = 1;
        currentHP = maxHP;
        currentGage = 0;
        interval = 3;
        intervalCount = interval;
        intervalText.text = intervalCount.ToString("F2");
    }

    //通常攻撃
    protected override IEnumerator NormalAttack()
    {
        StartCoroutine(AttackSubtitle("異爪"));
        isAttack = true;
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.8f * temp.x, 0.8f * temp.y);
        yield return new WaitForSeconds(0.5f);
        myRect.localScale = new(1.2f * temp.x, 1.2f * temp.y);
        //途中で死んだ時用
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
        }
        yield return new WaitForSeconds(0.1f);
        float size = 1.2f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.2f:(1フレームごとのサイズ変化)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 0.5f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        if (!isDied)
        {
            bSManager.EnemyToSainAttack(attack);
        }
        isAttack = false;
        switch (currentGage)
        {
            case 1:
                gage1Image.sprite = redGage;
                break;
            default:
                break;
        }
    }
    //チャージ技
    protected override IEnumerator ChargeAttack()
    {
        StartCoroutine(AttackSubtitle("異爪連"));
        isAttack = true;
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.7f * temp.x, 0.7f * temp.y);
        yield return new WaitForSeconds(0.5f);
        myRect.localScale = new(1.5f * temp.x, 1.5f * temp.y);
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
            StartCoroutine(AttackEffect(attackRect2, attackImage2));
        }
        yield return new WaitForSeconds(0.1f);
        float size = 1.5f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.5f:(1フレームごとのサイズ変化)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 1.25f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        if (!isDied)
        {
            bSManager.EnemyToSainAttack(attack * 2);
        }
        isAttack = false;
        gage1Image.sprite = grayGage;
    }
    public override void Revive()
    {
        //現状復活させる予定なし
    }
}
