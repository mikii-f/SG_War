using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SainManager : MonoBehaviour
{
    [SerializeField] private GameObject bSManagerObject;
    private BattleSceneManager1 bSManager;
    [SerializeField] private GameObject HPbar;
    [SerializeField] private GameObject SGbar;
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
    [SerializeField] private GameObject intervalDisplay;
    private Text intervalText;
    private const float interval = 4f;
    private float intervalCount = 0;
    private bool isGuard = false;
    private float attackFactor = 1;
    private float speedFactor = 1;
    private bool pause = false;
    public bool Pause { set { pause = value; } }

    // Start is called before the first frame update
    void Start()
    {
        bSManager = bSManagerObject.GetComponent<BattleSceneManager1>();
        HPslider = HPbar.GetComponent<Slider>();
        SGslider = SGbar.GetComponent<Slider>();
        bS1Rect = battleSkill1.GetComponent<RectTransform>();
        bS2Rect = battleSkill2.GetComponent<RectTransform>();
        bS3Rect = battleSkill3.GetComponent<RectTransform>();
        sARect = specialAttack.GetComponent<RectTransform>();
        sAImage = specialAttack.GetComponent<Image>();
        intervalText = intervalDisplay.GetComponent<Text>();
        sAImage.color = new(0.4f, 0.4f, 0.4f, 1);
        mask.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�C���^�[�o���Ǘ�
        if (intervalCount > 0 && !pause)
        {
            intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime);
            intervalText.text = intervalCount.ToString("F2");
        }
        //�U���\��ԂȂ�Ó]�p�}�X�N������
        else if (!pause && mask.activeSelf)
        {
            mask.SetActive(false);
        }
    }

    public void BattleSkill1Click()
    {
        //�C���^�[�o�����I����Ă��邩�̊m�F
        if (intervalCount == 0�@ && !pause)
        {
            intervalCount = interval*speedFactor;
            StartCoroutine(ButtonAnim(bS1Rect));
            mask.SetActive(true);
            //�ʏ�U��(�U���� SG��10)
            CauseDamage((int)(50*attackFactor));
            currentSG += 10;
            if (currentSG == 100)
            {
                sAImage.color = Color.white;
            }
            SGslider.value = (float)currentSG / maxSG;
        }
    }
    public void BattleSkill2Click()
    {
        //�C���^�[�o���̊m�F��SG>=10
        if (intervalCount == 0 && currentSG >= 10 && !pause)
        {
            intervalCount = interval*speedFactor;
            StartCoroutine(ButtonAnim(bS2Rect));
            mask.SetActive(true);
            //���U��(�U���� SG����10 �����ǉ�����)�@�āF�G�̍s���x��(�Z�H) ���g�̑��x�㏸ ���̍U�����
            CauseDamage(100);
            currentSG -= 10;
            SGslider.value = (float)currentSG / maxSG;
        }
    }
    public void BattleSkill3Click()
    {
        //�C���^�[�o���̊m�F��SG>=20
        if (interval == 0 && currentSG >= 20 && !pause)
        {
            intervalCount = interval * speedFactor;
            StartCoroutine(ButtonAnim(bS3Rect));
            mask.SetActive(true);
            //���ȋ���(SG����20 �C���^�[�o������ �K�[�h���s�\�ɂȂ� ���30%�㏸ �U���͏㏸)
            currentSG -= 20;
            SGslider.value = (float)currentSG / maxSG;
        }
    }

    public void SpecialAttackClick()
    {
        //SG>=100�Ȃ炢�ł�
        if (currentSG == maxSG && !pause)
        {
            StartCoroutine(ButtonAnim(sARect));
            sAImage.color = new(0.4f, 0.4f, 0.4f, 1);
            //�G�S�̂ɑ�_���[�W(SG����100 ���Ԃ��~�߂Đ�p���o �K�E�����̓G�̃K�[�h������)
            currentSG = 0;
            SGslider.value = 0;
        }
    }

    //�_���[�W�̒l��]��
    private void CauseDamage(int damege)
    {
        bSManager.SainToEnemyAttack(damege);
    }
    //�_���[�W���󂯂�
    public void ReceiveDamage(int damage)
    {
        //�K�[�h���Ȃ�9���J�b�g
        if (isGuard)
        {
            damage /= 10;
        }
        //�U���ҋ@���Ȃ�5���J�b�g
        else if (intervalCount == 0)
        {
            damage /= 2;
        }
        currentHP = Mathf.Max(0, currentHP - damage);
        HPslider.value = (float)currentHP / maxHP;
        if (currentHP == 0)
        {
            //�Q�[���I�[�o�[����
        }
    }

    //�K�[�h��ԂɂȂ�
    public IEnumerator ReceiveGuard()
    {
        isGuard = true;
        yield return new WaitForSeconds(0.5f);
        isGuard = false;
    }
    //�̗̓A�V�X�g���󂯂�(�b��400��)
    public void ReceiveHPAssist()
    {
        currentHP = Mathf.Min(currentHP + 400, maxHP);
        HPslider.value = (float)currentHP / maxHP;
    }
    //�U���A�V�X�g���󂯂�(�b��10�b�ԍU��+1.5�{)
    public IEnumerator ReceiveAttackAssist()
    {
        attackFactor += 0.5f;
        yield return new WaitForSeconds(10);
        attackFactor -= 0.5f;
        //float�v�Z�̌덷�ݐϑ΍�
        if ((int)attackFactor == 1)
        {
            attackFactor = 1;
        }
    }
    //���x�A�V�X�g���󂯂�(�b��10�b��30%�㏸)
    public IEnumerator ReceiveSpeedAssist()
    {
        speedFactor *= 0.7f;
        yield return new WaitForSeconds(10);
        speedFactor /= 0.7f;
        //float�v�Z�̌덷�ݐϑ΍�
        if ((int)speedFactor == 1)
        {
            speedFactor = 1;
        }
    }

    //�{�^���̃A�j���[�V����
    private IEnumerator ButtonAnim(RectTransform rect)
    {
        Vector2 temp = rect.localScale;
        rect.localScale = new(0.9f*temp.x, 0.9f*temp.y);
        yield return new WaitForSeconds(0.1f);
        rect.localScale = temp;
    }
}
