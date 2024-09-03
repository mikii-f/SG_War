using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CommandManager : EnemyManagerOrigin
{
    [SerializeField] private Image gage1Image;
    [SerializeField] private Image gage2Image;
    [SerializeField] private Image gage3Image;
    [SerializeField] private GameObject attackEffect;
    private RectTransform attackRect;
    private Image attackImage;
    [SerializeField] private Sprite attack1Sprite;
    [SerializeField] private Sprite attack2Sprite;
    [SerializeField] private GameObject attackEffect2;
    private RectTransform attackRect2;

    protected override void StartSet()
    {
        id = 3;
        attackRect = attackEffect.GetComponent<RectTransform>();
        attackImage = attackEffect.GetComponent<Image>();
        attackRect2 = attackEffect2.GetComponent<RectTransform>();
        attackImage.color = new(1, 1, 1, 0);
        attackEffect2.SetActive(false);
        maxGage = 3;
        currentGage = 0;
        interval = 5;
        intervalCount = interval;
        intervalText.text = intervalCount.ToString("F2");
    }

    //í èÌçUåÇ
    protected override IEnumerator NormalAttack()
    {
        StartCoroutine(AttackSubtitle("ódíe"));
        attackImage.sprite = attack1Sprite;
        isAttack = true;
        Vector2 temp = myRect.localScale;
        myRect.localScale = new(0.8f * temp.x, 0.8f * temp.y);
        yield return new WaitForSeconds(0.5f);
        //ìríÜÇ≈éÄÇÒÇæéûóp
        if (!isDied)
        {
            StartCoroutine(AttackEffect(attackRect, attackImage));
            StartCoroutine(Rotate(360, 0.5f));
        }
        myRect.localScale = new(1.2f * temp.x, 1.2f * temp.y);
        yield return new WaitForSeconds(0.1f);
        float size = 1.2f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.2f:(1ÉtÉåÅ[ÉÄÇ≤Ç∆ÇÃÉTÉCÉYïœâª)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 0.5f);
            myRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        //ìríÜÇ≈éÄÇÒÇæéûóp
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
    //É`ÉÉÅ[ÉWãZ
    protected override IEnumerator ChargeAttack()
    {
        StartCoroutine(AttackSubtitle("âêÕÅEÉKÉgÉäÉìÉO"));
        attackImage.sprite = attack2Sprite;
        attackImage.color = Color.white;
        attackRect.localScale = new(1.5f, 1.5f);
        isAttack = true;
        yield return new WaitForSeconds(0.3f);
        float timeCount = 0;
        if (!isDied)
        {
            StartCoroutine(Rotate(360, 0.3f));
            attackRect2.anchoredPosition = new(60, 90);
            attackRect2.localScale = new(1, 1);
            while (timeCount < 0.3f)
            {
                attackEffect2.SetActive(true);
                yield return null;
                timeCount += Time.deltaTime;
                yield return null;
                timeCount += Time.deltaTime;
                attackEffect2.SetActive(false);
                yield return null;
                timeCount += Time.deltaTime;
            }
        }
        attackImage.color = new(1, 1, 1, 0);
        attackRect.localScale = new(1, 1);
        timeCount = 0;
        yield return new WaitForSeconds(0.4f);
        if (!isDied)
        {
            bSManager.EnemyToSainAttack(attack * 2);
            attackRect2.localPosition = new(0, -100);
            attackRect2.localScale = new(4, 4);
            while (timeCount < 0.3f)
            {
                attackEffect2.SetActive(true);
                yield return null;
                timeCount += Time.deltaTime;
                yield return null;
                timeCount += Time.deltaTime;
                attackEffect2.SetActive(false);
                yield return null;
                timeCount += Time.deltaTime;
            }
        }
        isAttack = false;
        gage1Image.sprite = grayGage;
        gage2Image.sprite = grayGage;
        gage3Image.sprite = grayGage;
    }
    private IEnumerator Rotate(int angle, float time)
    {
        float timeCount = 0;
        while (timeCount <= time)
        {
            Vector3 temp = attackRect.localEulerAngles;
            //timeïbÇ≈angleìxâÒì]
            temp.z += (angle/time) * Time.deltaTime;
            attackRect.localEulerAngles = temp;
            yield return null;
            timeCount += Time.deltaTime;
        }
        attackRect.localEulerAngles = Vector3.zero;
    }
    //ëΩï™égÇÌÇ»Ç¢
    public override void Revive()
    {
        
    }
}