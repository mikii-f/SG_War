using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LowLevelEnemyManager : EnemyManagerOrigin
{
    [SerializeField] private Image gage1Image;
    [SerializeField] private Image gage2Image;
    [SerializeField] private Image gage3Image;
    [SerializeField] private GameObject attackEffect;
    private RectTransform attackRect;
    private Image attackImage;

    // Start is called before the first frame update
    protected override void StartSet()
    {
        id = 0;
        attackRect = attackEffect.GetComponent<RectTransform>();
        attackImage = attackEffect.GetComponent<Image>();
        attackImage.color = new(1, 1, 1, 0);
        maxGage = 3;
        currentGage = 0;
        interval = 5;
        intervalCount = interval;
        intervalText.text = intervalCount.ToString("F2");
    }

    //�ʏ�U��
    protected override IEnumerator NormalAttack()
    {
        StartCoroutine(AttackSubtitle("�d��"));
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
            bSManager.EnemyToSainAttack(attack);
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
            default:
                break;
        }
    }
    //�`���[�W�Z
    protected override IEnumerator ChargeAttack()
    {
        StartCoroutine(AttackSubtitle("�d���E��"));
        isAttack = true;
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.7f * temp.x, 0.7f * temp.y);
        yield return new WaitForSeconds(0.3f);
        myRect.localScale = new(1.5f * temp.x, 1.5f * temp.y);
        //�e�̑傫����2�{��
        attackRect.localScale = new(2, 2);
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
        }
        yield return new WaitForSeconds(0.3f);
        float size = 1.5f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.5f:(1�t���[�����Ƃ̃T�C�Y�ω�)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 1.25f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        if (!isDied)
        {
            bSManager.EnemyToSainAttack(attack * 2);
        }
        isAttack = false;
        gage1Image.sprite = grayGage;
        gage2Image.sprite = grayGage;
        gage3Image.sprite = grayGage;
    }
    public override void Revive()
    {
        myImage.color = Color.white;
        currentHP = maxHP;
        currentGage = 0;
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        gage1Image.sprite = grayGage;
        gage2Image.sprite = grayGage;
        gage3Image.sprite = grayGage;
        intervalCount = interval;
        isDied = false;
        myAllObject.SetActive(true);
    }
}