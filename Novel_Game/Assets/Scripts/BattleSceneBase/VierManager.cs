using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VierManager : ElManager
{
    [SerializeField] private AudioClip seSword;
    [SerializeField] private AudioClip seWind;
    protected override void StartSet()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        id = 4;
        attackRect = attackEffect.GetComponent<RectTransform>();
        attackImage = attackEffect.GetComponent<Image>();
        attackImage.color = new(1, 1, 1, 0);
        attackBuffIcon.SetActive(false);                //攻撃バフと速度バフ両方のアイコンを一つの画像に入れる
        shield.SetActive(false);
        buff.SetActive(false);
        specialAttackAnimation.SetActive(false);
        maxGage = 5;
        currentGage = 0;
        interval = 4;
        intervalCount = interval;
        intervalText.text = intervalCount.ToString("F2");
        attack1Name = "斬りつけ";
        attack2Name = "旋風剣";
        attack2Angle = new(0, 0, 0);
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
            specialAttackCount = 0;
            isShield = false;
            shield.SetActive(false);
        }
        if (!pause)
        {
            //攻撃中以外で生きている間はインターバルをカウント
            if (intervalCount > 0 && !isAttack && !isDied)
            {
                if (skill3Active > 0)
                {
                    intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime * 1.5f);
                }
                else
                {
                    intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime);
                }
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
                    //ゲージが4で必殺インターバルが少なければスキル2か3
                    if (currentGage == 4 && specialAttackCount < specialAttackInterval - 1)
                    {
                        if (skill3Active > 0)
                        {
                            StartCoroutine(Skill2());
                        }
                        else
                        {
                            StartCoroutine(Skill3());
                        }
                    }
                    //スキル3発動中はスキル2が出やすい
                    else if (skill3Active > 0 && UnityEngine.Random.Range(0, 10) < 3)
                    {
                        StartCoroutine(Skill2());
                    }
                    //ゲージが0または確率で通常攻撃
                    else if (currentGage == 0 || UnityEngine.Random.Range(0, 10) < 7)
                    {
                        StartCoroutine(NormalAttack());
                    }
                    //スキル3発動中でないとき確率でスキル3
                    else if (skill3Active == 0 && UnityEngine.Random.Range(0, 10) < 4)
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
                    //インターバルを終えていたら必殺技準備に入る
                    if (specialAttackCount >= specialAttackInterval)
                    {
                        intervalCount = interval * 2;
                        isShield = true;
                        shield.SetActive(true);
                        specialAttackStandby = true;
                        StartCoroutine(Buff());
                        StartCoroutine(AttackSubtitle("必殺技準備"));
                    }
                    //終えていなければスキル2(理論上は起こらないはず)
                    else
                    {
                        StartCoroutine(Skill2());
                    }
                }
            }
        }
    }
    //スキル3(与ダメージ上昇3回、その間速度上昇)
    protected override IEnumerator Skill3()
    {
        intervalCount = interval;
        currentGage--;
        specialAttackCount++;
        attackBuffIcon.SetActive(true);
        StartCoroutine(AttackSubtitle("ツインドライブ"));
        StartCoroutine(Buff());
        skill3Active = 3;
        attackBuffIcon.SetActive(true);
        isAttack = true;
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
        isAttack = false;
    }
    protected override IEnumerator SpecialSE()
    {
        seSource.clip = seWind;
        seSource.Play();
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < 4; i++)
        {
            seSource.clip = seSword;
            seSource.Play();
            yield return new WaitForSeconds(0.15f);
        }
    }
}
