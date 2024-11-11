using UnityEngine;

public class SainManager2 : SainManager
{
    private int attackFactor2 = 0;
    private bool avoid = false;
    private int attackIndex = -1;
    private int avoidIndex = -1;
    protected override void AutoAttack()
    {
        //バフがかかっていないかつある程度SGがあればスキル3を使用
        if (attackFactor2 == 0 && currentSG >= 30)
        {
            BattleSkill3Click();
        }
        //バフがかかっているときはスキル2を優先使用
        else if (attackFactor2 > 0 && currentSG >= 10)
        {
            BattleSkill2Click();
        }
        //SGが少ないときは通常攻撃(S3発動→S2を5回→S1を3回のループになるはず)
        else
        {
            BattleSkill1Click();
        }
    }
    public override void BattleSkill1Click()
    {
        //インターバルが終わっているかの確認
        if (intervalCount == 0 && !pause)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS1Rect));
            mask.SetActive(true);
            //通常攻撃(攻撃力の100% SG回復10 スキル3のバフが切れる)
            StartCoroutine(bSManager.SainSkill1(attack * (attackFactor + attackFactor2) / 10, attack1Rect, attack1Image));
            if (attackFactor2 > 0)
            {
                attackFactor2 = 0;
                BuffIndexCheck(attackIndex, 1);
                attackIndex = -1;
                mask3.SetActive(false);
            }
            currentSG = Mathf.Min(maxSG, currentSG + 10);
            SGCheck();
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else if (!pause)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    public override void BattleSkill2Click()
    {
        //インターバルの確認＆SG>=10
        if (intervalCount == 0 && currentSG >= 10 && !pause)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS2Rect));
            mask.SetActive(true);
            //強攻撃(攻撃力の200% SG消費10 スキル3のバフを一部保持)
            StartCoroutine(bSManager.SainSkill2_2(2 * attack * (attackFactor + attackFactor2) / 10, attack2Rect, attack2Image));
            currentSG -= 10;
            if (attackFactor2 > 0)
            {
                attackFactor2--;
                if (attackFactor2 == 0)
                {
                    BuffIndexCheck(attackIndex, 1);
                    attackIndex = -1;
                    mask3.SetActive(false);
                }
            }
            SGCheck();
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else if (!pause)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    public override void BattleSkill3Click()
    {
        //インターバルの確認&スキル3による攻撃バフが切れている
        if (intervalCount == 0 && !pause && attackFactor2 == 0)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS3Rect));
            mask.SetActive(true);
            mask3.SetActive(true);
            StartCoroutine(EffectOnandOff(buffEffect));
            seSource.clip = seBuff;
            seSource.Play();
            //回数制バフのためインデックスを分けて管理する必要がある
            for (int i = 0; i < buffDebuffNumber; i++)
            {
                if (buffAndDebuffs[i].sprite == noneSprite)
                {
                    //スキル3による回避がかかっているときはアイコンを追加しない
                    if (!avoid)
                    {
                        buffAndDebuffs[i].sprite = attackBuffIcon;
                        buffAndDebuffs[i + 1].sprite = avoidBuffIcon;
                        for (int j = 0; j < buffDebuffNumber; j++)
                        {
                            if (buffIndex[j] == -1 && attackIndex == -1)
                            {
                                attackIndex = j;
                                buffIndex[j] = i;
                            }
                            else if (buffIndex[j] == -1)
                            {
                                avoidIndex = j;
                                buffIndex[j] = i + 1;
                                break;
                            }
                        }
                    }
                    else
                    {
                        buffAndDebuffs[i].sprite = attackBuffIcon;
                        for (int j = 0; j < buffDebuffNumber; j++)
                        {
                            if (buffIndex[j] == -1)
                            {
                                attackIndex = j;
                                buffIndex[j] = i;
                                break;
                            }
                        }
                    }
                    break;
                }
            }
            //攻撃力+50%1回&回避1回(重ね掛け不可)&SG+20
            currentSG = Mathf.Min(maxSG, currentSG + 20);
            SGCheck();
            attackFactor2 = Mathf.Min(10, attackFactor2 + 5);
            avoid = true;
        }
        else if (!pause)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    public override void SpecialAttackClick()
    {
        //SG>=100ならいつでも
        if (currentSG >= 100 && !pause)
        {
            StartCoroutine(ButtonAnim(sARect));
            StartCoroutine(Invincible(1));
            StartCoroutine(bSManager.SpecialAttackName(specialAttackNameSprite));
            //敵全体に攻撃力500%(SG消費100 時間を止めて専用演出 必殺持ちの敵のガードを割る) スキル3のバフを保持
            StartCoroutine(bSManager.SainToAllAttack(5 * attack * (attackFactor + attackFactor2) / 10, true));
            StartCoroutine(SESpecialFinish());
            currentSG -= 100;
            SGCheck();
        }
        else if (!pause)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    protected override void SGCheck()
    {
        SGslider.value = (float)currentSG / maxSG;
        SGText.text = currentSG.ToString() + "/" + maxSG.ToString();
        if (currentSG >= 100)
        {
            sAImage.color = Color.white;
        }
        else
        {
            sAImage.color = new(0.4f, 0.4f, 0.4f, 1);
        }
        if (currentSG >= 10)
        {
            mask2.SetActive(false);
        }
        else
        {
            mask2.SetActive(true);
        }
    }
    public override void ReceiveDamage(int damage)
    {
        //ポーズ中(勝利後)はノーダメ
        if (pause)
        {
            damage = 0;
        }
        //無敵時間ならノーダメ
        else if (isInvincible)
        {
            damage = 0;
        }
        //回避したらノーダメ
        else if (avoid)
        {
            damage = 0;
            avoid = false;
            BuffIndexCheck(avoidIndex, 1);
            avoidIndex = -1;
            if (commentCoroutine != null)
            {
                StopCoroutine(commentCoroutine);
                ResetComment();
            }
            commentCoroutine = StartCoroutine(Comment("回避成功"));
            damageSeSource.clip = seAvoid;
            damageSeSource.Play();
        }
        //ガード中なら9割カット
        else if (isGuard)
        {
            damage /= 10;
            if (commentCoroutine != null)
            {
                StopCoroutine(commentCoroutine);
                ResetComment();
            }
            commentCoroutine = StartCoroutine(Comment("ガード成功"));
            damageSeSource.clip = seGuard;
            damageSeSource.Play();
        }
        //攻撃待機中なら5割カット
        else if (intervalCount == 0)
        {
            damage /= 2;
            damageSeSource.clip = seDamage;
            damageSeSource.Play();
        }
        //ガードなどをしていなかった時は振動
        else
        {
            StartCoroutine(DamageVibration());
            damageSeSource.clip = seDamage;
            damageSeSource.Play();
        }
        //表示に使われていないテキストボックスを使う
        if (damageText.text == "")
        {
            StartCoroutine(DamageDisplay(damage, damageText));
        }
        else if (damageText2.text == "")
        {
            StartCoroutine(DamageDisplay(damage, damageText2));
        }
        else
        {
            StartCoroutine(DamageDisplay(damage, damageText3));
        }
        currentHP = Mathf.Max(0, currentHP - damage);
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        if (currentHP == 0)
        {
            StartCoroutine(bSManager.GameOver());
        }
    }
}
