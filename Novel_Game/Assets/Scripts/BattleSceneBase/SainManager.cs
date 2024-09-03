using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class SainManager : SystemManagerOrigin
{
    [SerializeField] private BattleSceneManagerOrigin bSManager;
    [SerializeField] private RectTransform myRect;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private TMP_Text SGText;
    [SerializeField] private Slider HPslider;
    [SerializeField] private Slider SGslider;
    private int maxHP = 1000;
    private const int maxSG = 120;
    private int currentHP = 1000;
    private int currentSG = 20;
    private int attack = 50;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private GameObject specialAttack;
    [SerializeField] private RectTransform bS1Rect;
    [SerializeField] private RectTransform bS2Rect;
    [SerializeField] private RectTransform bS3Rect;
    private RectTransform sARect;
    private Image sAImage;
    [SerializeField] private GameObject mask;
    [SerializeField] private GameObject mask2;
    [SerializeField] private GameObject mask3;
    [SerializeField] private GameObject attack1Effect;
    private RectTransform attack1Rect;
    private Image attack1Image;
    [SerializeField] private GameObject attack2Effect;
    private RectTransform attack2Rect;
    private Image attack2Image;
    [SerializeField] private GameObject buffEffect;
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject guardEffect;
    [SerializeField] private Text intervalText;
    private const float interval = 4f;
    private float intervalCount = 0;
    [SerializeField] private GameObject buffDebuffFolder;
    private int buffDebuffNumber;
    private Image[] buffAndDebuffs;
    private int[] buffIndex;
    [SerializeField] private Sprite noneSprite;
    [SerializeField] private Sprite attackBuffIcon;
    [SerializeField] private Sprite speedBuffIcon;
    [SerializeField] private Sprite avoidBuffIcon;
    //[SerializeField] private Sprite cannotGuardIcon;
    [SerializeField] private GameObject commentPanel;
    private RectTransform commentPanelRect;
    [SerializeField] private Text comment;
    [SerializeField] private Sprite specialAttackNameSprite;
    private bool isGuard = false;
    //private bool isCannotGuard = false;
    //public bool IsCannotGuard { get { return isCannotGuard; } }   �K�[�h�s�̓f�����b�g�Ƃ��đ傫������̂ł�������O��
    private bool isSkill3 = false;
    private bool isInvincible = false;
    //�����l�ŌW�����������ƂŊȒP��(float�͐F�X�Ƃ߂�ǂ�)(/10�̏����ɖ�肪�Ȃ��悤�C������)
    private int attackFactor = 10;
    private int speedFactor = 10;
    private int avoidFactor = 0;
    private float buffTimer = 0;
    private bool pause = true;
    public bool Pause { set { pause = value; } }
    private bool auto = false;
    public bool Auto { set { auto = value; } get { return auto; } }
    private Coroutine commentCoroutine;
    private Coroutine damageCoroutine;

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
            }
        }
    }

    public void BattleSkill1Click()
    {
        //�C���^�[�o�����I����Ă��邩�̊m�F
        if (intervalCount == 0�@ && !pause)
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
        }
    }
    public void BattleSkill2Click()
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
        }
    }
    //�N���b�N�̎󂯎��̓R���[�`���ɂł��Ȃ����ۂ�
    public void BattleSkill3Click()
    {
        //�C���^�[�o���̊m�F��SG>=20���X�L��3�g�p���łȂ�
        if (intervalCount == 0 && currentSG >= 20 && !pause && !isSkill3)
        {
            StartCoroutine(BattleSkill3());
        }
    }
    private IEnumerator BattleSkill3()
    {
        //�����ɍčs���\
        StartCoroutine(ButtonAnim(bS3Rect));
        mask.SetActive(true);
        StartCoroutine(EffectOnandOff(buffEffect));
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

    public void SpecialAttackClick()
    {
        //SG>=100�Ȃ炢�ł�
        if (currentSG >= 100 && !pause)
        {
            StartCoroutine(ButtonAnim(sARect));
            StartCoroutine(Invincible(1));
            StartCoroutine(bSManager.SpecialAttackName(specialAttackNameSprite));
            //�G�S�̂ɍU����500%(SG����100 ���Ԃ��~�߂Đ�p���o �K�E�����̓G�̃K�[�h������)
            StartCoroutine(bSManager.SainToAllAttack(5*attack * attackFactor / 10));
            currentSG -= 100;
            SGCheck();
        }
    }
    
    //��ɕK�E�Z��������̂��߂̖��G����
    private IEnumerator Invincible(float invincibleTime)
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    //SG�l�ɂ���ă}�X�N�؂�ւ�
    private void SGCheck()
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
    public void ReceiveDamage(int damage)
    {
        //�|�[�Y��(������)�̓m�[�_��
        if (pause)
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
        }
        //���G���ԂȂ�m�[�_��
        else if (isInvincible)
        {
            damage = 0;
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
        }
        //�U���ҋ@���Ȃ�5���J�b�g
        else if (intervalCount == 0)
        {
            damage /= 2;
        }
        //�K�[�h�Ȃǂ����Ă��Ȃ��������͐U��
        else
        {
            StartCoroutine(DamageVibration());
        }
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(DamageDisplay(damage));
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
        StartCoroutine(DamageDisplay(damage));
        currentHP = Mathf.Max(0, currentHP - damage);
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        if (currentHP == 0)
        {
            StartCoroutine(bSManager.GameOver());
        }
    }

    //�_���[�W�\��(������2�̍U�����󂯂�����������₷���悤�ɂ�����)
    private IEnumerator DamageDisplay(int damage)
    {
        damageText.text = damage.ToString();
        yield return new WaitForSeconds(0.35f);
        damageText.text = "";
    }
    //�K�[�h�E��𐬌����̃R�����g
    private IEnumerator Comment(string text)
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
    private void ResetComment()
    {
        commentPanel.SetActive(false);
        Vector2 temp = commentPanelRect.anchoredPosition;
        temp.x = 250;
        commentPanelRect.anchoredPosition = temp;
    }
    //�_���[�W�󂯎�莞�̗h��
    private IEnumerator DamageVibration()
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
    }
    //�U���A�V�X�g���󂯂�(�b��15�b�ԍU��+1.5�{)
    public IEnumerator ReceiveAttackAssist()
    {
        StartCoroutine(EffectOnandOff(buffEffect));
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
    private IEnumerator EffectOnandOff(GameObject effect)
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
    private void BuffIndexCheck(int n, int m)
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