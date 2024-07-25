using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ElManager : EnemyManagerOrigin
{
    [SerializeField] private Image gage1Image;
    [SerializeField] private Image gage2Image;
    [SerializeField] private Image gage3Image;
    [SerializeField] private Image gage4Image;
    [SerializeField] private Image gage5Image;
    [SerializeField] private GameObject attackBuffIcon;  //バフアイコン
    [SerializeField] private GameObject shieldIcon;     //シールドアイコン
    [SerializeField] private GameObject attackEffect;
    private RectTransform attackRect;
    private Image attackImage;
    [SerializeField] private Sprite attack1Sprite;
    [SerializeField] private Sprite attack2Sprite;
    [SerializeField] private GameObject buff;
    private int skill3Active = 0;
    private const int skill3Interval = 6;
    private int skill3Count = 0;
    private const int specialAttackInterval = 10;
    private int specialAttackCount = 0;             //スキル3・必殺技が連発されないように攻撃回数をカウント
    private bool specialAttackStandby = false;      //必殺技の溜め
    

    protected override void StartSet()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        id = 4;
        attackRect = attackEffect.GetComponent<RectTransform>();
        attackImage = attackEffect.GetComponent<Image>();
        attackImage.color = new(1, 1, 1, 0);
        attackBuffIcon.SetActive(false);
        shieldIcon.SetActive(false);
        buff.SetActive(false);
        maxGage = 5;
        currentGage = 0;
        interval = 4;
        intervalCount = interval;
    }

    void Update()
    {
        //シールドを破壊されたとき
        if (!isShield && specialAttackStandby)
        {
            specialAttackStandby = false;
            intervalCount = interval * 2;
            currentGage = 0;
            gage1Image.sprite = grayGage;
            gage2Image.sprite = grayGage;
            gage3Image.sprite = grayGage;
            gage4Image.sprite = grayGage;
            gage5Image.sprite = grayGage;
            skill3Count = 0;
            specialAttackCount = 0;
            isShield = false;
            shieldIcon.SetActive(false);
        }
        if (!pause)
        {
            //攻撃中以外で生きている間はインターバルをカウント
            if (intervalCount > 0 && !isAttack && !isDied)
            {
                intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime);
                intervalText.text = intervalCount.ToString("F2");
            }
            else if (intervalCount == 0)
            {
                //必殺技溜め状態なら必殺技発動
                if (specialAttackStandby)
                {
                    StartCoroutine(ChargeAttack());
                }
                else if (currentGage < maxGage)
                {
                    //ゲージが0または半分の確率で通常攻撃
                    if (currentGage == 0 || UnityEngine.Random.Range(0, 10) < 5)
                    {
                        StartCoroutine(NormalAttack());
                    }
                    //ゲージが3以下で6回のインターバルを終えていたらスキル3発動
                    else if (currentGage <= 3 && skill3Count >= skill3Interval)
                    {
                        StartCoroutine(Skill3());
                    }
                    //スキル2
                    else
                    {
                        StartCoroutine(Skill2());
                    }
                }
                else if (currentGage == maxGage)
                {
                    //10回のインターバルを終えていたら必殺技準備に入る
                    if (specialAttackCount >= specialAttackInterval)
                    {
                        intervalCount = interval * 2;
                        isShield = true;
                        shieldIcon.SetActive(true);
                        specialAttackStandby = true;
                        StartCoroutine(Buff());
                        StartCoroutine(AttackSubtitle("必殺技準備"));
                    }
                    //超えていなければスキル2
                    else
                    {
                        StartCoroutine(Skill2());
                    }
                }
            }
        }
    }
    //通常攻撃
    protected override IEnumerator NormalAttack()
    {
        intervalCount = interval;
        currentGage++;
        skill3Count++;
        specialAttackCount++;
        StartCoroutine(AttackSubtitle("抜刀"));
        attackImage.sprite = attack1Sprite;
        isAttack = true;
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.8f * temp.x, 0.8f * temp.y);
        yield return new WaitForSeconds(0.5f);
        //途中で死んだ時用
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
        }
        myRect.localScale = new(1.2f * temp.x, 1.2f * temp.y);
        yield return new WaitForSeconds(0.1f);
        float size = 1.2f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.2f:(1フレームごとのサイズ変化)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 0.5f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        //途中で死んだ時用
        if (!isDied)
        {
            if (skill3Active > 0)
            {
                bSManager.EnemyToSainAttack(attack * 3 / 2);
                skill3Active--;
                if (skill3Active == 0)
                {
                    attackBuffIcon.SetActive(false);
                }
            }
            else
            {
                bSManager.EnemyToSainAttack(attack);
            }
        }
        isAttack = false;
        switch (currentGage)
        {
            case 1:
                gage1Image.sprite = redGage;
                break;
            case 2:
                gage2Image.sprite = redGage;
                break;
            case 3:
                gage3Image.sprite = redGage;
                break;
            case 4:
                gage4Image.sprite = redGage;
                break;
            case 5:
                gage5Image.sprite = redGage;
                break;
            default:
                break;
        }
    }
    //スキル2(暫定スキル1と同じ演出)(通常の2倍ダメージ)
    private IEnumerator Skill2()
    {
        intervalCount = interval;
        currentGage--;
        skill3Count++;
        specialAttackCount++;
        StartCoroutine(AttackSubtitle("剣の舞"));
        attackImage.sprite = attack2Sprite;
        isAttack = true;
        attackRect.localRotation = Quaternion.Euler(70, 30, 0);
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.8f * temp.x, 0.8f * temp.y);
        yield return new WaitForSeconds(0.5f);
        //途中で死んだ時用
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
        }
        myRect.localScale = new(1.2f * temp.x, 1.2f * temp.y);
        yield return new WaitForSeconds(0.1f);
        float size = 1.2f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.2f:(1フレームごとのサイズ変化)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 0.5f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        //途中で死んだ時用
        if (!isDied)
        {
            if (skill3Active > 0)
            {
                bSManager.EnemyToSainAttack(attack * 3);
                skill3Active--;
                if (skill3Active == 0)
                {
                    attackBuffIcon.SetActive(false);
                }
            }
            else
            {
                bSManager.EnemyToSainAttack(attack * 2);
            }
        }
        isAttack = false;
        switch (currentGage)
        {
            case 0:
                gage1Image.sprite = grayGage;
                break;
            case 1:
                gage2Image.sprite = grayGage;
                break;
            case 2:
                gage3Image.sprite = grayGage;
                break;
            case 3:
                gage4Image.sprite = grayGage;
                break;
            case 4:
                gage5Image.sprite = grayGage;
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(1);
        attackRect.localRotation = Quaternion.Euler(0, 0, 0);
    }
    //スキル3(与ダメージ上昇2回、チャージ増加)
    private IEnumerator Skill3()
    {
        intervalCount = interval;
        currentGage += 2;
        skill3Count = 0;
        specialAttackCount++;
        attackBuffIcon.SetActive(true);
        StartCoroutine(AttackSubtitle("明鏡止水"));
        StartCoroutine(Buff());
        skill3Active = 2;
        attackBuffIcon.SetActive(true);
        isAttack = true;
        switch (currentGage)
        {
            case 2:
                gage1Image.sprite = redGage;
                gage2Image.sprite = redGage;
                break;
            case 3:
                gage2Image.sprite = redGage;
                gage3Image.sprite = redGage;
                break;
            case 4:
                gage3Image.sprite = redGage;
                gage4Image.sprite = redGage;
                break;
            case 5:
                gage4Image.sprite = redGage;
                gage5Image.sprite = redGage;
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(1);
        isAttack = false;
    }
    //必殺技(暫定スキル2と同じ演出)(通常の5倍ダメージ、ガード・回避不可)
    protected override IEnumerator ChargeAttack()
    {
        intervalCount = interval;
        isAttack = true;
        specialAttackStandby = false;
        currentGage = 0;
        skill3Count = 0;
        specialAttackCount = 0;
        shieldIcon.SetActive(false);
        StartCoroutine(AttackSubtitle("黒踊聖騎・絢爛剣舞"));
        attackImage.sprite = attack2Sprite;
        isAttack = true;
        StartCoroutine(bSManager.EnemySpecialAttack());
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.8f * temp.x, 0.8f * temp.y);
        yield return new WaitForSeconds(0.5f);
        //途中で死んだ時用
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
        }
        myRect.localScale = new(1.2f * temp.x, 1.2f * temp.y);
        yield return new WaitForSeconds(0.1f);
        float size = 1.2f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.2f:(1フレームごとのサイズ変化)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 0.5f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        //途中で死んだ時用
        if (!isDied)
        {
            if (skill3Active > 0)
            {
                bSManager.EnemyToSainAttack(attack * 5 * 3 / 2);
                skill3Active--;
                if (skill3Active == 0)
                {
                    attackBuffIcon.SetActive(false);
                }
            }
            else
            {
                bSManager.EnemyToSainAttack(attack * 5);
            }
        }
        isAttack = false;
        isShield = false;
        shieldIcon.SetActive(false);
        gage1Image.sprite = grayGage;
        gage2Image.sprite = grayGage;
        gage3Image.sprite = grayGage;
        gage4Image.sprite = grayGage;
        gage5Image.sprite = grayGage;
    }

    //バフ
    private IEnumerator Buff()
    {
        buff.SetActive(true);
        yield return new WaitForSeconds(1);
        buff.SetActive(false);
    }

    //現状使う予定なし(クリア後戦闘ではありかも)
    public override void Revive()
    {
        
    }
}
