using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ComplexEnemyManager : EnemyManagerOrigin
{
    [SerializeField] private GameObject gage1;
    [SerializeField] private GameObject gage2;
    private Image gage1Image;
    private Image gage2Image;

    // Start is called before the first frame update
    protected override void StartSet()
    {
        gage1Image = gage1.GetComponent<Image>();
        gage2Image = gage2.GetComponent<Image>();
        maxGage = 2;
        currentHP = maxHP;
        currentGage = 0;
        interval = 6;
        intervalCount = interval;
    }

    //�ʏ�U��
    protected override IEnumerator NormalAttack()
    {
        isAttack = true;
        //�ʏ�͉������Ȃ�
        yield return new WaitForSeconds(1);
        isAttack = false;
        switch (currentGage)
        {
            case 1:
                gage1Image.sprite = redGage;
                break;
            case 2:
                gage2Image.sprite = redGage;
                break;
            default:
                break;
        }
    }
    //�`���[�W�Z
    protected override IEnumerator ChargeAttack()
    {
        isAttack = true;
        //�����ŉ����ٗd��1�̑��B(�Đ�)���鏈��
        yield return new WaitForSeconds(1);
        isAttack = false;
        gage1Image.sprite = grayGage;
        gage2Image.sprite = grayGage;
    }
}
