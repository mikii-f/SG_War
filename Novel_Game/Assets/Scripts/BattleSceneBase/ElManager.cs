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
    [SerializeField] private GameObject attackBuffIcon;  //�o�t�A�C�R��
    [SerializeField] private GameObject shieldIcon;     //�V�[���h�A�C�R��
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
    private int specialAttackCount = 0;             //�X�L��3�E�K�E�Z���A������Ȃ��悤�ɍU���񐔂��J�E���g
    private bool specialAttackStandby = false;      //�K�E�Z�̗���
    

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
            skill3Count = 0;
            specialAttackCount = 0;
            isShield = false;
            shieldIcon.SetActive(false);
        }
        if (!pause)
        {
            //�U�����ȊO�Ő����Ă���Ԃ̓C���^�[�o�����J�E���g
            if (intervalCount > 0 && !isAttack && !isDied)
            {
                intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime);
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
                    //�Q�[�W��0�܂��͔����̊m���Œʏ�U��
                    if (currentGage == 0 || UnityEngine.Random.Range(0, 10) < 5)
                    {
                        StartCoroutine(NormalAttack());
                    }
                    //�Q�[�W��3�ȉ���6��̃C���^�[�o�����I���Ă�����X�L��3����
                    else if (currentGage <= 3 && skill3Count >= skill3Interval)
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
                    //10��̃C���^�[�o�����I���Ă�����K�E�Z�����ɓ���
                    if (specialAttackCount >= specialAttackInterval)
                    {
                        intervalCount = interval * 2;
                        isShield = true;
                        shieldIcon.SetActive(true);
                        specialAttackStandby = true;
                        StartCoroutine(Buff());
                        StartCoroutine(AttackSubtitle("�K�E�Z����"));
                    }
                    //�����Ă��Ȃ���΃X�L��2
                    else
                    {
                        StartCoroutine(Skill2());
                    }
                }
            }
        }
    }
    //�ʏ�U��
    protected override IEnumerator NormalAttack()
    {
        intervalCount = interval;
        currentGage++;
        skill3Count++;
        specialAttackCount++;
        StartCoroutine(AttackSubtitle("����"));
        attackImage.sprite = attack1Sprite;
        isAttack = true;
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.8f * temp.x, 0.8f * temp.y);
        yield return new WaitForSeconds(0.5f);
        //�r���Ŏ��񂾎��p
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
        }
        myRect.localScale = new(1.2f * temp.x, 1.2f * temp.y);
        yield return new WaitForSeconds(0.1f);
        float size = 1.2f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.2f:(1�t���[�����Ƃ̃T�C�Y�ω�)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 0.5f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        //�r���Ŏ��񂾎��p
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
    //�X�L��2(�b��X�L��1�Ɠ������o)(�ʏ��2�{�_���[�W)
    private IEnumerator Skill2()
    {
        intervalCount = interval;
        currentGage--;
        skill3Count++;
        specialAttackCount++;
        StartCoroutine(AttackSubtitle("���̕�"));
        attackImage.sprite = attack2Sprite;
        isAttack = true;
        attackRect.localRotation = Quaternion.Euler(70, 30, 0);
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.8f * temp.x, 0.8f * temp.y);
        yield return new WaitForSeconds(0.5f);
        //�r���Ŏ��񂾎��p
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
        }
        myRect.localScale = new(1.2f * temp.x, 1.2f * temp.y);
        yield return new WaitForSeconds(0.1f);
        float size = 1.2f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.2f:(1�t���[�����Ƃ̃T�C�Y�ω�)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 0.5f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        //�r���Ŏ��񂾎��p
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
    //�X�L��3(�^�_���[�W�㏸2��A�`���[�W����)
    private IEnumerator Skill3()
    {
        intervalCount = interval;
        currentGage += 2;
        skill3Count = 0;
        specialAttackCount++;
        attackBuffIcon.SetActive(true);
        StartCoroutine(AttackSubtitle("�����~��"));
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
    //�K�E�Z(�b��X�L��2�Ɠ������o)(�ʏ��5�{�_���[�W�A�K�[�h�E���s��)
    protected override IEnumerator ChargeAttack()
    {
        intervalCount = interval;
        isAttack = true;
        specialAttackStandby = false;
        currentGage = 0;
        skill3Count = 0;
        specialAttackCount = 0;
        shieldIcon.SetActive(false);
        StartCoroutine(AttackSubtitle("���x���R�E��࣌���"));
        attackImage.sprite = attack2Sprite;
        isAttack = true;
        StartCoroutine(bSManager.EnemySpecialAttack());
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.8f * temp.x, 0.8f * temp.y);
        yield return new WaitForSeconds(0.5f);
        //�r���Ŏ��񂾎��p
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
        }
        myRect.localScale = new(1.2f * temp.x, 1.2f * temp.y);
        yield return new WaitForSeconds(0.1f);
        float size = 1.2f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.2f:(1�t���[�����Ƃ̃T�C�Y�ω�)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 0.5f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        //�r���Ŏ��񂾎��p
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

    //�o�t
    private IEnumerator Buff()
    {
        buff.SetActive(true);
        yield return new WaitForSeconds(1);
        buff.SetActive(false);
    }

    //����g���\��Ȃ�(�N���A��퓬�ł͂��肩��)
    public override void Revive()
    {
        
    }
}
