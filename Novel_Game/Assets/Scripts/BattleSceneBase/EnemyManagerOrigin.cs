using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyManagerOrigin : MonoBehaviour
{
    protected int id;
    public int ID { get { return id; } }
    [SerializeField] protected BattleSceneManagerOrigin bSManager;
    [SerializeField] protected GameObject myAllObject;
    public bool AllObject { set { myAllObject.SetActive(value); } }
    [SerializeField] private GameObject myObject;
    protected RectTransform myRect;
    protected Image myImage;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject attackPanel;
    private RectTransform attackPanelRect;
    [SerializeField] private Text attackSubtitle;
    [SerializeField] private RectTransform gageRect;
    [SerializeField] protected Slider HPslider;
    [SerializeField] protected TMP_Text HPText;
    [SerializeField] protected Sprite grayGage;
    [SerializeField] protected Sprite redGage;
    [SerializeField] protected int maxHP;
    [SerializeField] protected int attack;
    protected int maxGage;
    protected int currentHP;
    protected int currentGage;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private Text intervalText;
    protected float interval;
    protected float intervalCount;
    protected bool isAttack = false;
    protected bool isDied = false;
    public bool Dead { get { return isDied; } }
    private bool pause = true;
    public bool Pause { set { pause = value; } }

    // Start is called before the first frame update
    void Start()
    {
        myRect = myObject.GetComponent<RectTransform>();
        myImage = myObject.GetComponent<Image>();
        attackPanelRect = attackPanel.GetComponent<RectTransform>();
        attackPanel.SetActive(false);
        currentHP = maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        StartSet();
    }
    protected abstract void StartSet();

    //�ʏ�ƃ`���[�W��2��ނ����U���������Ȃ��G���G�l�~�[�p �{�X�̓I�[�o�[���C�h����
    void Update()
    {
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
                if (currentGage < maxGage)
                {
                    intervalCount = interval;
                    //�ʏ�U��
                    currentGage++;
                    StartCoroutine(NormalAttack());
                }
                else if (currentGage == maxGage)
                {
                    intervalCount = interval;
                    //�`���[�W�Z
                    currentGage = 0;
                    StartCoroutine(ChargeAttack());
                }
            }
        }
    }
    protected abstract IEnumerator NormalAttack();
    protected abstract IEnumerator ChargeAttack();

    //�f�t�H���g�̍U�����o((1-0.5)�b�Œ��e)
    protected IEnumerator AttackEffect(RectTransform attackRect, Image attackImage)
    {
        attackImage.color = Color.white;
        float diffX = -attackRect.anchoredPosition.x;
        float diffY = -100;
        float diffScale = attackRect.localScale.x * 2;
        while (true)
        {
            Vector2 temp = attackRect.anchoredPosition;
            Vector2 temp2 = attackRect.localScale;
            //0.5�b�ō��W(0,-100)��
            temp.x += diffX * Time.deltaTime * 2;
            temp.y += diffY * Time.deltaTime * 2;
            //0.5�b��3�{�̑傫����
            temp2.x += diffScale * Time.deltaTime * 2;
            temp2.y += diffScale * Time.deltaTime * 2;
            attackRect.anchoredPosition = temp;
            attackRect.localScale = temp2;
            if (temp.y <= -100)
            {
                break;
            }
            yield return null;
        }
        //���e�E����
        float waitTime = 0.05f;
        float fadeTime = 0.5f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = attackImage.color;
            newColor.a = alpha / 255.0f;
            attackImage.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
        //������
        attackRect.anchoredPosition = new (-diffX, 0);
        attackRect.localScale = new Vector2(1, 1);
    }
    //�U�����̎���
    protected IEnumerator AttackSubtitle(string attackName)
    {
        attackSubtitle.text = attackName;
        attackPanel.SetActive(true);
        while (attackPanelRect.anchoredPosition.x < 100)
        {
            yield return null;
            Vector2 pos = attackPanelRect.anchoredPosition;
            pos.x += 1000 * Time.deltaTime;
            attackPanelRect.anchoredPosition = pos;
        }
        yield return new WaitForSeconds(1);
        attackPanel.SetActive(false);
        Vector2 temp = attackPanelRect.anchoredPosition;
        temp.x = -100;
        attackPanelRect.anchoredPosition = temp;
    }

    //�_���[�W�󂯎��
    public void ReceiveDamage(int damage)
    {
        //���͎��񂾓G�����S�ɏ��ł����Ă��Ȃ����߁A������t���Ȃ��ƕK�E��2��ڂ̏��Ŕ��肪�N����(�K�E���ł��Ή��ς݂����O�̂���)
        if (!isDied)
        {
            StartCoroutine(DamageDisplay(damage));
            currentHP = Mathf.Max(0, currentHP - damage);
            HPslider.value = (float)currentHP / maxHP;
            HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
            StartCoroutine(DamageVibration());
            if (currentHP == 0)
            {
                StartCoroutine(Died());
            }
        }
    }
    //�_���[�W�\��(��̊֐����R���[�`�������Ă�����)
    private IEnumerator DamageDisplay(int damage)
    {
        damageText.text = damage.ToString();
        yield return new WaitForSeconds(0.35f);
        damageText.text = "";
    }
    //�_���[�W�󂯎�莞�̗h��
    private IEnumerator DamageVibration()
    {
        Vector2 temp = myRect.anchoredPosition;
        //float�v�Z�̌덷�΍�
        float defaultX = temp.x;
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
        temp.x = defaultX;
        myRect.anchoredPosition = temp;
    }
    public void ReceiveDelay()
    {
        if (!isAttack)
        {
            intervalCount += 1;
        }
    }
    //����
    private IEnumerator Died()
    {
        isDied = true;
        bSManager.EnemyDied();
        float waitTime = 0.1f;
        float fadeTime = 1;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = myImage.color;
            newColor.a = alpha / 255.0f;
            myImage.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
        myAllObject.SetActive(false);
    }
    //�U���ΏۂƂ��đI�����ꂽ�Ƃ���UI�g��k��
    public void Select()
    {
        namePanel.SetActive(true);
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        gageRect.localScale = Vector3.one;
    }
    public void DisSelect()
    {
        namePanel.SetActive(false);
        HPText.text = "";
        gageRect.localScale = new(0.5f, 0.5f);
    }

    //����
    public abstract void Revive();
}