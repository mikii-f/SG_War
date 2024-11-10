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
        attackBuffIcon.SetActive(false);                //�U���o�t�Ƒ��x�o�t�����̃A�C�R������̉摜�ɓ����
        shield.SetActive(false);
        buff.SetActive(false);
        specialAttackAnimation.SetActive(false);
        maxGage = 5;
        currentGage = 0;
        interval = 4;
        intervalCount = interval;
        intervalText.text = intervalCount.ToString("F2");
        attack1Name = "�a���";
        attack2Name = "������";
        attack2Angle = new(0, 0, 0);
    }
    void Update()
    {
        //�V�[���h��j�󂳂ꂽ�Ƃ�
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
            //�U�����ȊO�Ő����Ă���Ԃ̓C���^�[�o�����J�E���g
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
                //�K�E�Z���ߏ�ԂȂ�K�E�Z����
                if (specialAttackStandby)
                {
                    StartCoroutine(ChargeAttack());
                }
                else if (currentGage < maxGage)
                {
                    //�Q�[�W��4�ŕK�E�C���^�[�o�������Ȃ���΃X�L��2��3
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
                    //�X�L��3�������̓X�L��2���o�₷��
                    else if (skill3Active > 0 && UnityEngine.Random.Range(0, 10) < 3)
                    {
                        StartCoroutine(Skill2());
                    }
                    //�Q�[�W��0�܂��͊m���Œʏ�U��
                    else if (currentGage == 0 || UnityEngine.Random.Range(0, 10) < 7)
                    {
                        StartCoroutine(NormalAttack());
                    }
                    //�X�L��3�������łȂ��Ƃ��m���ŃX�L��3
                    else if (skill3Active == 0 && UnityEngine.Random.Range(0, 10) < 4)
                    {
                        StartCoroutine(Skill3());
                    }
                    //�X�L��2
                    else
                    {
                        StartCoroutine(Skill2());
                    }
                }
                else if (currentGage == maxGage)
                {
                    //�C���^�[�o�����I���Ă�����K�E�Z�����ɓ���
                    if (specialAttackCount >= specialAttackInterval)
                    {
                        intervalCount = interval * 2;
                        isShield = true;
                        shield.SetActive(true);
                        specialAttackStandby = true;
                        StartCoroutine(Buff());
                        StartCoroutine(AttackSubtitle("�K�E�Z����"));
                    }
                    //�I���Ă��Ȃ���΃X�L��2(���_��͋N����Ȃ��͂�)
                    else
                    {
                        StartCoroutine(Skill2());
                    }
                }
            }
        }
    }
    //�X�L��3(�^�_���[�W�㏸3��A���̊ԑ��x�㏸)
    protected override IEnumerator Skill3()
    {
        intervalCount = interval;
        currentGage--;
        specialAttackCount++;
        attackBuffIcon.SetActive(true);
        StartCoroutine(AttackSubtitle("�c�C���h���C�u"));
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
