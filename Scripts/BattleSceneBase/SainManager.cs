using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class SainManager : SystemManagerOrigin
{
    [SerializeField] protected BattleSceneManagerOrigin bSManager;
    [SerializeField] private RectTransform myRect;
    [SerializeField] protected TMP_Text HPText;
    [SerializeField] protected TMP_Text SGText;
    [SerializeField] protected Slider HPslider;
    [SerializeField] protected Slider SGslider;
    protected int maxHP = 1000;
    protected const int maxSG = 120;
    protected int currentHP = 1000;
    protected int currentSG = 20;
    protected int attack = 50;
    [SerializeField] protected TMP_Text damageText;
    [SerializeField] protected TMP_Text damageText2;
    [SerializeField] protected TMP_Text damageText3;
    [SerializeField] private GameObject specialAttack;
    [SerializeField] protected RectTransform bS1Rect;
    [SerializeField] protected RectTransform bS2Rect;
    [SerializeField] protected RectTransform bS3Rect;
    protected RectTransform sARect;
    protected Image sAImage;
    [SerializeField] protected GameObject mask;
    [SerializeField] protected GameObject mask2;
    [SerializeField] protected GameObject mask3;
    [SerializeField] private GameObject attack1Effect;
    protected RectTransform attack1Rect;
    protected Image attack1Image;
    [SerializeField] private GameObject attack2Effect;
    protected RectTransform attack2Rect;
    protected Image attack2Image;
    [SerializeField] protected GameObject buffEffect;
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject guardEffect;
    [SerializeField] private Text intervalText;
    protected const float interval = 4f;
    protected float intervalCount = 0;
    [SerializeField] private GameObject buffDebuffFolder;
    protected int buffDebuffNumber;
    protected Image[] buffAndDebuffs;
    protected int[] buffIndex;
    [SerializeField] protected Sprite noneSprite;
    [SerializeField] protected Sprite attackBuffIcon;
    [SerializeField] private Sprite speedBuffIcon;
    [SerializeField] protected Sprite avoidBuffIcon;
    //[SerializeField] private Sprite cannotGuardIcon;
    [SerializeField] private GameObject commentPanel;
    private RectTransform commentPanelRect;
    [SerializeField] private Text comment;
    [SerializeField] protected Sprite specialAttackNameSprite;
    protected bool isGuard = false;
    //private bool isCannotGuard = false;
    //public bool IsCannotGuard { get { return isCannotGuard; } }   �K�[�h�s�̓f�����b�g�Ƃ��đ傫������̂ł�������O��
    private bool isSkill3 = false;
    protected bool isInvincible = false;
    //�����l�ŌW�����������ƂŊȒP��(float�͐F�X�Ƃ߂�ǂ�)(/10�̏����ɖ�肪�Ȃ��悤�C������)
    protected int attackFactor = 10;
    private int speedFactor = 10;
    private int avoidFactor = 0;
    private float buffTimer = 0;
    protected bool pause = true;
    public bool Pause { set { pause = value; } get { return pause; } }
    private bool auto = false;
    public bool Auto { set { auto = value; } get { return auto; } }
    protected Coroutine commentCoroutine;
    [SerializeField] protected AudioSource damageSeSource;
    [SerializeField] private AudioSource lSkillSeSource;
    [SerializeField] protected AudioClip seDamage;
    [SerializeField] private AudioClip seSpecialDamage;
    [SerializeField] protected AudioClip seGuard;
    [SerializeField] protected AudioClip seBuff;
    [SerializeField] private AudioClip seHeal;
    [SerializeField] private AudioClip seSpecialFinish;
    [SerializeField] protected AudioClip seAvoid;

    void Start()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        maxHP = Mathf.Max(GameManager.instance.SainHP, 1000);       //�J�����ɂ�����GameManager�̒l���ݒ肳��Ă��Ȃ��󋵗p
        attack = Mathf.Max(GameManager.instance.SainAttack, 50);
        currentSG = Mathf.Max(GameManager.instance.SainSG, 20);
        currentHP = maxHP;
        HPslider.value = 1;
        SGslider.value = (float)currentSG / maxSG;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        SGText.text = currentSG.ToString() + "/" + maxSG.ToString();
        sARect = specialAttack.GetComponent<RectTransform>();
        sAImage = specialAttack.GetComponent<Image>();
        attack1Rect = attack1Effect.GetComponent<RectTransform>();
        attack1Image = attack1Effect.GetComponent<Image>();
        attack1Image.color = new(1, 1, 1, 0);
        attack2Rect = attack2Effect.GetComponent<RectTransform>();
        attack2Image = attack2Effect.GetComponent<Image>();
        attack2Image.color = new(1, 1, 1, 0);
        buffDebuffNumber = buffDebuffFolder.transform.childCount;
        buffAndDebuffs = new Image[buffDebuffNumber];
        buffIndex = new int[buffDebuffNumber];
        for (int i = 0; i < buffDebuffNumber; i++)
        {
            buffAndDebuffs[i] = buffDebuffFolder.transform.GetChild(i).GetComponent<Image>();
            buffIndex[i] = -1;
        }
        commentPanelRect = commentPanel.GetComponent<RectTransform>();
        commentPanel.SetActive(false);
        sAImage.color = new(0.4f, 0.4f, 0.4f, 1);
        mask.SetActive(false);
        mask2.SetActive(false);
        mask3.SetActive(false);
        //1��ڂ̓X�L���Q�[�W�������Ȃ�
        if (SceneManager.GetActiveScene().name == "BattleScene1")
        {
            mask2.SetActive(true);
            mask3.SetActive(true);
            currentSG = 0;
            SGText.text = currentSG.ToString() + "/" + maxSG.ToString();
            SGslider.value = 0;
        }
        buffEffect.SetActive(false);
        healEffect.SetActive(false);
        guardEffect.SetActive(false);
        seSource = GetComponent<AudioSource>();
        seSource.volume = GameManager.instance.SeVolume;
        damageSeSource.volume = GameManager.instance.SeVolume;
        lSkillSeSource.volume = GameManager.instance.SeVolume;
    }

    void Update()
    {
        if (!pause)
        {
            //�o�t�̎������Ԍv���p(���΂�float�̕\���͈͒�����\���͍l���Ȃ��Ă����͂��c�c)
            buffTimer += Time.deltaTime;
            //�C���^�[�o���Ǘ�
            if (intervalCount > 0)
            {
                intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime * speedFactor / 10);
                intervalText.text = intervalCount.ToString("F2");
            }
            //�U���\��ԂȂ�Ó]�p�}�X�N������
            else if (mask.activeSelf)
            {
                mask.SetActive(false);
            }
            //�퓬�X�L������ѕK�E�̑I��
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                BattleSkill1Click();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                BattleSkill2Click();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                BattleSkill3Click();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SpecialAttackClick();
            }
            //�I�[�g��(�v���C���[������͂ł���)(���������͂��Ȃ��͂��c�c)
            if (auto)
            {
                if (intervalCount == 0)
                {
                    AutoAttack();
                }
            }
        }
    }
    protected virtual void AutoAttack()
    {
        //SG>=50��S3���g�p
        if (currentSG >= 50 && !isSkill3)
        {
            BattleSkill3Click();
        }
        //S3�g�p���̓X�L��2��D�悵�Ďg�p
        else if (isSkill3 && currentSG >= 10)
        {
            BattleSkill2Click();
        }
        else
        {
            BattleSkill1Click();
        }
    }

    public virtual void BattleSkill1Click()
    {
        //�C���^�[�o�����I����Ă��邩�̊m�F
        if (intervalCount == 0 && !pause)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS1Rect));
            mask.SetActive(true);
            //�ʏ�U��(�U���͂�100% SG��10)
            StartCoroutine(bSManager.SainSkill1(attack*attackFactor/10, attack1Rect, attack1Image));
            //1��ڂ͒ʏ�U�������ł��Ȃ�
            if (SceneManager.GetActiveScene().name != "BattleScene1")
            {
                currentSG = Mathf.Min(maxSG, currentSG + 10);
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
    public virtual void BattleSkill2Click()
    {
        //�C���^�[�o���̊m�F��SG>=10
        if (intervalCount == 0 && currentSG >= 10 && !pause)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS2Rect));
            mask.SetActive(true);
            //���U��(�U���͂�200% SG����10 �U���Ԑ��ɓ���O�̓G�̍s���x��)
            StartCoroutine(bSManager.SainSkill2(2*attack*attackFactor/10, attack2Rect, attack2Image));
            currentSG -= 10;
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
    //�N���b�N�̎󂯎��̓R���[�`���ɂł��Ȃ����ۂ�
    public virtual void BattleSkill3Click()
    {
        //�C���^�[�o���̊m�F��SG>=20���X�L��3�g�p���łȂ�
        if (intervalCount == 0 && currentSG >= 20 && !pause && !isSkill3)
        {
            StartCoroutine(BattleSkill3());
        }
        else if (!pause)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    private IEnumerator BattleSkill3()
    {
        //�����ɍčs���\
        StartCoroutine(ButtonAnim(bS3Rect));
        StartCoroutine(EffectOnandOff(buffEffect));
        seSource.clip = seBuff;
        seSource.Play();
        //�o�t�󋵂̊Ǘ�
        int index = -1;
        for (int i=0; i<buffDebuffNumber; i++)
        {
            if (buffAndDebuffs[i].sprite == noneSprite)
            {
                buffAndDebuffs[i].sprite = speedBuffIcon;
                buffAndDebuffs[i + 1].sprite = avoidBuffIcon;
                buffAndDebuffs[i + 2].sprite = attackBuffIcon;
                //buffAndDebuffs[i + 3].sprite = cannotGuardIcon;
                for (int j=0; j<buffDebuffNumber; j++)
                {
                    if (buffIndex[j] == -1)
                    {
                        index = j;
                        buffIndex[j] = i;
                        break;
                    }
                }
                break;
            }
        }
        //���ȋ���(SG����20 �C���^�[�o������ ���30%�㏸ �U���͏㏸) �d�ˊ|���s��
        currentSG -= 20;
        speedFactor += 10;
        avoidFactor += 3;
        attackFactor += 4;
        isSkill3 = true;
        SGCheck();
        float tempTimer = buffTimer;
        yield return new WaitUntil(() => buffTimer - tempTimer >= 10);
        //�o�t�󋵂̊Ǘ�
        BuffIndexCheck(index, 3);
        speedFactor -= 10;
        avoidFactor -= 3;
        attackFactor -= 4;
        isSkill3 = false;
        if (currentSG >= 20)
        {
            mask3.SetActive(false);
        }
    }

    public virtual void SpecialAttackClick()
    {
        //SG>=100�Ȃ炢�ł�
        if (currentSG >= 100 && !pause)
        {
            StartCoroutine(ButtonAnim(sARect));
            StartCoroutine(Invincible(1));
            StartCoroutine(bSManager.SpecialAttackName(specialAttackNameSprite));
            //�G�S�̂ɍU����500%(SG����100 ���Ԃ��~�߂Đ�p���o �K�E�����̓G�̃K�[�h������)
            StartCoroutine(bSManager.SainToAllAttack(5*attack * attackFactor / 10, false));
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
    //�K�E�Z�̍Ō�̕����̌��ʉ���Ɨ�������
    protected IEnumerator SESpecialFinish()
    {
        yield return new WaitForSeconds(3);
        seSource.clip = seSpecialFinish;
        seSource.Play();
    }
    
    //��ɕK�E�Z��������̂��߂̖��G����
    protected IEnumerator Invincible(float invincibleTime)
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    //SG�l�ɂ���ă}�X�N�؂�ւ�
    protected virtual void SGCheck()
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
        if (currentSG >= 20 && !isSkill3)
        {
            mask3.SetActive(false);
        }
        else
        {
            mask3.SetActive(true);
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

    //�_���[�W���󂯂�
    public virtual void ReceiveDamage(int damage)
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
        //����ɐ���������m�[�_��
        else if (UnityEngine.Random.Range(0, 10) < avoidFactor)
        {
            damage = 0;
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
    //�K�E�Z�_���[�W���󂯂�(�K�[�h������)
    public void ReceiveSpecialDamage(int damage)
    {
        StartCoroutine(DamageVibration());
        StartCoroutine(DamageDisplay(damage, damageText));
        damageSeSource.clip = seSpecialDamage;
        damageSeSource.Play();
        currentHP = Mathf.Max(0, currentHP - damage);
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        if (currentHP == 0)
        {
            StartCoroutine(bSManager.GameOver());
        }
    }

    //�_���[�W�\��
    protected IEnumerator DamageDisplay(int damage, TMP_Text damageText)
    {
        damageText.text = damage.ToString();
        yield return new WaitForSeconds(0.35f);
        damageText.text = "";
    }
    //�K�[�h�E��𐬌����̃R�����g
    protected IEnumerator Comment(string text)
    {
        comment.text = text;
        commentPanel.SetActive(true);
        while (commentPanelRect.anchoredPosition.x < 450)
        {
            yield return null;
            Vector2 pos = commentPanelRect.anchoredPosition;
            pos.x += 1000 * Time.deltaTime;
            commentPanelRect.anchoredPosition = pos;
        }
        yield return new WaitForSeconds(1);
        commentPanel.SetActive(false);
        Vector2 temp = commentPanelRect.anchoredPosition;
        temp.x = 250;
        commentPanelRect.anchoredPosition = temp;
    }
    //0.5�b�̊Ԃ�2��Ăяo���ꂽ���p
    protected void ResetComment()
    {
        commentPanel.SetActive(false);
        Vector2 temp = commentPanelRect.anchoredPosition;
        temp.x = 250;
        commentPanelRect.anchoredPosition = temp;
    }
    //�_���[�W�󂯎�莞�̗h��
    protected IEnumerator DamageVibration()
    {
        Vector2 temp = myRect.anchoredPosition;
        temp.x += 10;
        myRect.anchoredPosition = temp;
        yield return new WaitForSeconds(0.1f);
        temp.x -= 20;
        myRect.anchoredPosition = temp;
        yield return new WaitForSeconds(0.1f);
        temp.x += 20;
        myRect.anchoredPosition = temp;
        yield return new WaitForSeconds(0.1f);
        temp.x -= 20;
        myRect.anchoredPosition = temp;
        yield return new WaitForSeconds(0.1f);
        temp.x = 0;
        myRect.anchoredPosition = temp;
    }

    //�K�[�h��ԂɂȂ�
    public IEnumerator ReceiveGuard()
    {
        isGuard = true;
        guardEffect.SetActive(true);
        float tempTimer = buffTimer;
        yield return new WaitUntil(() => buffTimer - tempTimer >= 0.5f);
        isGuard = false;
        guardEffect.SetActive(false);    
    }
    //�̗̓A�V�X�g���󂯂�(�b��5����)
    public void ReceiveHPAssist()
    {
        currentHP = Mathf.Min(currentHP + maxHP/2, maxHP);
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        StartCoroutine(EffectOnandOff(healEffect));
        lSkillSeSource.clip = seHeal;
        lSkillSeSource.Play();
    }
    //�U���A�V�X�g���󂯂�(�b��15�b�ԍU��+1.5�{)
    public IEnumerator ReceiveAttackAssist()
    {
        StartCoroutine(EffectOnandOff(buffEffect));
        lSkillSeSource.clip = seBuff;
        lSkillSeSource.Play();
        attackFactor += 5;
        float tempTimer = buffTimer;
        //�o�t�󋵂̊Ǘ�
        int index = -1;
        for (int i = 0; i < buffDebuffNumber; i++)
        {
            if (buffAndDebuffs[i].sprite == noneSprite)
            {
                buffAndDebuffs[i].sprite = attackBuffIcon;
                for (int j = 0; j < buffDebuffNumber; j++)
                {
                    if (buffIndex[j] == -1)
                    {
                        index = j;
                        buffIndex[j] = i;
                        break;
                    }
                }
                break;
            }
        }
        yield return new WaitUntil(() => buffTimer - tempTimer >= 15);
        BuffIndexCheck(index, 1);
        attackFactor -= 5;
    }
    //���x�A�V�X�g���󂯂�(�b��20�b��5�㏸)
    public IEnumerator ReceiveSpeedAssist()
    {
        StartCoroutine(EffectOnandOff(buffEffect));
        lSkillSeSource.clip = seBuff;
        lSkillSeSource.Play();
        speedFactor += 5;
        float tempTimer = buffTimer;
        //�o�t�󋵂̊Ǘ�
        int index = -1;
        for (int i = 0; i < buffDebuffNumber; i++)
        {
            if (buffAndDebuffs[i].sprite == noneSprite)
            {
                buffAndDebuffs[i].sprite = speedBuffIcon;
                for (int j = 0; j < buffDebuffNumber; j++)
                {
                    if (buffIndex[j] == -1)
                    {
                        index = j;
                        buffIndex[j] = i;
                        break;
                    }
                }
                break;
            }
        }
        yield return new WaitUntil(() => buffTimer - tempTimer >= 20);
        BuffIndexCheck(index, 1);
        speedFactor -= 5;
    }

    //�o�t�Ȃǂ̃A�j���[�V����(�t�F�[�h����H)
    protected IEnumerator EffectOnandOff(GameObject effect)
    {
        //�d�ˊ|�����͂Ƃ肠�����ŏ��̂����\��
        if (!effect.activeSelf)
        {
            effect.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            effect.SetActive(false);
        }
    }
    //�o�t�󋵂̊Ǘ�
    //�o�t�������Ɏ擾�����C���f�b�N�X���K�X�ύX����Ă����`
    //(��F0�ԖځE1�Ԗڂ̃A�C�R���g��null�łȂ�2�ԖڂɃA�C�R����u�������C���f�b�N�X�ێ��z��̒l��-1�̂Ƃ�����������̃C���f�b�N�X���ꎞ�I�ɋL����2���i�[��
    //���̊֐����K�؂ɃC���f�b�N�X���Ǘ����o�t�I�����ɃC���f�b�N�X�ێ��z�񂪎w���ꏊ�̃A�C�R����null�ɂ��z��̒l��-1�ɖ߂�)
    //��1�����͈ꎞ�I�ɋL�������C���f�b�N�X�A��2�����͏����o�t�̐�
    protected void BuffIndexCheck(int n, int m)
    {
        //�A�C�R����������
        for (int i = buffIndex[n]; i < buffIndex[n]+m; i++)
        {
            buffAndDebuffs[i].sprite = noneSprite;
        }
        for (int i= buffIndex[n] + m; i<buffDebuffNumber; i++)
        {
            buffAndDebuffs[i - m].sprite = buffAndDebuffs[i].sprite;
            buffAndDebuffs[i].sprite = noneSprite;
        }
        //���̑��̃A�C�R���̃C���f�b�N�X�Ǘ�
        for (int i=0; i<buffDebuffNumber; i++)
        {
            if (buffIndex[i] > buffIndex[n])
            {
                buffIndex[i] -= m;
            }
        }
        buffIndex[n] = -1;
    }
}