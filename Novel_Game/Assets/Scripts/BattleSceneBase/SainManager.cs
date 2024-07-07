using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SainManager : MonoBehaviour
{
    [SerializeField] private GameObject bSManagerObject;
    private BattleSceneManagerOrigin bSManager;
    [SerializeField] private GameObject myObject;
    protected RectTransform myRect;
    [SerializeField] private GameObject HPbar;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private GameObject SGbar;
    [SerializeField] private TMP_Text SGText;
    private Slider HPslider;
    private Slider SGslider;
    private int maxHP = 1000;
    private const int maxSG = 100;
    private int currentHP = 1000;
    private int currentSG = 20;
    [SerializeField] private GameObject battleSkill1;
    [SerializeField] private GameObject battleSkill2;
    [SerializeField] private GameObject battleSkill3;
    [SerializeField] private GameObject specialAttack;
    private RectTransform bS1Rect;
    private RectTransform bS2Rect;
    private RectTransform bS3Rect;
    private RectTransform sARect;
    private Image sAImage;
    [SerializeField] private GameObject mask;
    [SerializeField] private GameObject mask2;
    [SerializeField] private GameObject mask3;
    [SerializeField] private GameObject attack1Effect;
    protected RectTransform attack1Rect;
    protected Image attack1Image;
    [SerializeField] private GameObject attack2Effect;
    protected RectTransform attack2Rect;
    protected Image attack2Image;
    [SerializeField] private GameObject buffEffect;
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject guardEffect;
    [SerializeField] private GameObject intervalDisplay;
    private Text intervalText;
    private const float interval = 4f;
    private float intervalCount = 0;
    private bool isGuard = false;
    private bool isCannotGuard = false;
    private bool isInvincible = false;
    //�����l�ŌW�����������ƂŊȒP��(float�͐F�X�Ƃ߂�ǂ�)(/10�̏����ɖ�肪�Ȃ��悤�C������)
    private int attackFactor = 10;
    private int speedFactor = 10;
    private float buffTimer = 0;
    private bool pause = true;
    public bool Pause { set { pause = value; } }

    // Start is called before the first frame update
    void Start()
    {
        bSManager = bSManagerObject.GetComponent<BattleSceneManagerOrigin>();
        myRect = myObject.GetComponent<RectTransform>();
        HPslider = HPbar.GetComponent<Slider>();
        SGslider = SGbar.GetComponent<Slider>();
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        SGText.text = currentSG.ToString() + "/" + maxSG.ToString();
        bS1Rect = battleSkill1.GetComponent<RectTransform>();
        bS2Rect = battleSkill2.GetComponent<RectTransform>();
        bS3Rect = battleSkill3.GetComponent<RectTransform>();
        sARect = specialAttack.GetComponent<RectTransform>();
        sAImage = specialAttack.GetComponent<Image>();
        attack1Rect = attack1Effect.GetComponent<RectTransform>();
        attack1Image = attack1Effect.GetComponent<Image>();
        attack1Image.color = new(1, 1, 1, 0);
        attack2Rect = attack2Effect.GetComponent<RectTransform>();
        attack2Image = attack2Effect.GetComponent<Image>();
        attack2Image.color = new(1, 1, 1, 0);
        intervalText = intervalDisplay.GetComponent<Text>();
        sAImage.color = new(0.4f, 0.4f, 0.4f, 1);
        mask.SetActive(false);
        mask2.SetActive(false);
        mask3.SetActive(false);
        buffEffect.SetActive(false);
        healEffect.SetActive(false);
        guardEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            //�o�t�̎������Ԍv���p
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
    }

    public void BattleSkill1Click()
    {
        //�C���^�[�o�����I����Ă��邩�̊m�F
        if (intervalCount == 0�@ && !pause)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS1Rect));
            mask.SetActive(true);
            //�ʏ�U��(�U���� SG��10)
            StartCoroutine(bSManager.SainSkill1(50*attackFactor/10, attack1Rect, attack1Image));
            currentSG = Mathf.Min(100, currentSG + 10);
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
            //���U��(�U���� SG����10 �U���Ԑ��ɓ���O�̓G�̍s���x��)
            StartCoroutine(bSManager.SainSkill2(100*attackFactor/10, attack2Rect, attack2Image));
            currentSG -= 10;
            SGCheck();
        }
    }
    //�N���b�N�̎󂯎��̓R���[�`���ɂł��Ȃ����ۂ�
    public void BattleSkill3Click()
    {
        //�C���^�[�o���̊m�F��SG>=20���K�[�h�s�łȂ�(���ɃK�[�h�s��t�^������ʂ���������ɂ͕ʃt���O��p��)
        if (intervalCount == 0 && currentSG >= 20 && !pause && !isCannotGuard)
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
        //���ȋ���(SG����20 �C���^�[�o������ �K�[�h���s�\�ɂȂ� ���30%�㏸(��Ŏ���) �U���͏㏸) �d�ˊ|���s��
        currentSG -= 20;
        SGCheck();
        speedFactor += 10;
        isCannotGuard = true;
        attackFactor += 4;
        float tempTimer = buffTimer;
        yield return new WaitUntil(() => buffTimer - tempTimer >= 10);
        speedFactor -= 10;
        isCannotGuard = false;
        attackFactor -= 4;
        if (currentSG >= 20)
        {
            mask3.SetActive(false);
        }
    }

    public void SpecialAttackClick()
    {
        //SG>=100�Ȃ炢�ł�
        if (currentSG == maxSG && !pause)
        {
            StartCoroutine(ButtonAnim(sARect));
            StartCoroutine(Invincible(1));
            //�G�S�̂ɑ�_���[�W(SG����100 ���Ԃ��~�߂Đ�p���o �K�E�����̓G�̃K�[�h������)
            StartCoroutine(bSManager.SainToAllAttack(150 * attackFactor / 10));
            currentSG = 0;
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
        if (currentSG >= 20 && !isCannotGuard)
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
        //���G���ԂȂ�m�[�_��
        if (isInvincible)
        {
            damage = 0;
        }
        //�K�[�h���Ȃ�9���J�b�g
        else if (isGuard)
        {
            damage /= 10;
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
        currentHP = Mathf.Max(0, currentHP - damage);
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        if (currentHP == 0)
        {
            StartCoroutine(bSManager.GameOver());
        }
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
        if (!isCannotGuard)
        {
            isGuard = true;
            guardEffect.SetActive(true);
            float tempTimer = buffTimer;
            yield return new WaitUntil(() => buffTimer - tempTimer >= 0.5f);
            isGuard = false;
            guardEffect.SetActive(false);
        }
    }
    //�̗̓A�V�X�g���󂯂�(�b��400��)
    public void ReceiveHPAssist()
    {
        currentHP = Mathf.Min(currentHP + 400, maxHP);
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        StartCoroutine(EffectOnandOff(healEffect));
    }
    //�U���A�V�X�g���󂯂�(�b��10�b�ԍU��+1.5�{)
    public IEnumerator ReceiveAttackAssist()
    {
        StartCoroutine(EffectOnandOff(buffEffect));
        attackFactor += 5;
        float tempTimer = buffTimer;
        yield return new WaitUntil(() => buffTimer - tempTimer >= 10);
        attackFactor -= 5;
    }
    //���x�A�V�X�g���󂯂�(�b��10�b��30%�㏸)
    public IEnumerator ReceiveSpeedAssist()
    {
        StartCoroutine(EffectOnandOff(buffEffect));
        speedFactor += 5;
        float tempTimer = buffTimer;
        yield return new WaitUntil(() => buffTimer - tempTimer >= 10);
        speedFactor -= 5;
    }

    //�{�^���̃A�j���[�V����
    private IEnumerator ButtonAnim(RectTransform rect)
    {
        Vector2 temp = rect.localScale;
        rect.localScale = new(0.9f*temp.x, 0.9f*temp.y);
        yield return new WaitForSeconds(0.1f);
        rect.localScale = temp;
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
}
