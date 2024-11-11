using UnityEngine;

public class SainManager2 : SainManager
{
    private int attackFactor2 = 0;
    private bool avoid = false;
    private int attackIndex = -1;
    private int avoidIndex = -1;
    protected override void AutoAttack()
    {
        //�o�t���������Ă��Ȃ���������xSG������΃X�L��3���g�p
        if (attackFactor2 == 0 && currentSG >= 30)
        {
            BattleSkill3Click();
        }
        //�o�t���������Ă���Ƃ��̓X�L��2��D��g�p
        else if (attackFactor2 > 0 && currentSG >= 10)
        {
            BattleSkill2Click();
        }
        //SG�����Ȃ��Ƃ��͒ʏ�U��(S3������S2��5��S1��3��̃��[�v�ɂȂ�͂�)
        else
        {
            BattleSkill1Click();
        }
    }
    public override void BattleSkill1Click()
    {
        //�C���^�[�o�����I����Ă��邩�̊m�F
        if (intervalCount == 0 && !pause)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS1Rect));
            mask.SetActive(true);
            //�ʏ�U��(�U���͂�100% SG��10 �X�L��3�̃o�t���؂��)
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
        //�C���^�[�o���̊m�F��SG>=10
        if (intervalCount == 0 && currentSG >= 10 && !pause)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS2Rect));
            mask.SetActive(true);
            //���U��(�U���͂�200% SG����10 �X�L��3�̃o�t���ꕔ�ێ�)
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
        //�C���^�[�o���̊m�F&�X�L��3�ɂ��U���o�t���؂�Ă���
        if (intervalCount == 0 && !pause && attackFactor2 == 0)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS3Rect));
            mask.SetActive(true);
            mask3.SetActive(true);
            StartCoroutine(EffectOnandOff(buffEffect));
            seSource.clip = seBuff;
            seSource.Play();
            //�񐔐��o�t�̂��߃C���f�b�N�X�𕪂��ĊǗ�����K�v������
            for (int i = 0; i < buffDebuffNumber; i++)
            {
                if (buffAndDebuffs[i].sprite == noneSprite)
                {
                    //�X�L��3�ɂ�������������Ă���Ƃ��̓A�C�R����ǉ����Ȃ�
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
            //�U����+50%1��&���1��(�d�ˊ|���s��)&SG+20
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
        //SG>=100�Ȃ炢�ł�
        if (currentSG >= 100 && !pause)
        {
            StartCoroutine(ButtonAnim(sARect));
            StartCoroutine(Invincible(1));
            StartCoroutine(bSManager.SpecialAttackName(specialAttackNameSprite));
            //�G�S�̂ɍU����500%(SG����100 ���Ԃ��~�߂Đ�p���o �K�E�����̓G�̃K�[�h������) �X�L��3�̃o�t��ێ�
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
        //�|�[�Y��(������)�̓m�[�_��
        if (pause)
        {
            damage = 0;
        }
        //���G���ԂȂ�m�[�_��
        else if (isInvincible)
        {
            damage = 0;
        }
        //���������m�[�_��
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
            commentCoroutine = StartCoroutine(Comment("��𐬌�"));
            damageSeSource.clip = seAvoid;
            damageSeSource.Play();
        }
        //�K�[�h���Ȃ�9���J�b�g
        else if (isGuard)
        {
            damage /= 10;
            if (commentCoroutine != null)
            {
                StopCoroutine(commentCoroutine);
                ResetComment();
            }
            commentCoroutine = StartCoroutine(Comment("�K�[�h����"));
            damageSeSource.clip = seGuard;
            damageSeSource.Play();
        }
        //�U���ҋ@���Ȃ�5���J�b�g
        else if (intervalCount == 0)
        {
            damage /= 2;
            damageSeSource.clip = seDamage;
            damageSeSource.Play();
        }
        //�K�[�h�Ȃǂ����Ă��Ȃ��������͐U��
        else
        {
            StartCoroutine(DamageVibration());
            damageSeSource.clip = seDamage;
            damageSeSource.Play();
        }
        //�\���Ɏg���Ă��Ȃ��e�L�X�g�{�b�N�X���g��
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
