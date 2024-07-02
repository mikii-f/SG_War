using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LowLevelEnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject bSManagerObject;
    private BattleSceneManager1 bSManager;
    [SerializeField] private GameObject myAllObject;
    [SerializeField] private GameObject lowLevelEnemy;
    private RectTransform lLEnemyRect;
    private Image lLEnemyImage;
    [SerializeField] private GameObject HPbar;
    private Slider HPslider;
    [SerializeField] private GameObject gage1;
    [SerializeField] private GameObject gage2;
    [SerializeField] private GameObject gage3;
    private Image gage1Image;
    private Image gage2Image;
    private Image gage3Image;
    [SerializeField] private Sprite grayGage;
    [SerializeField] private Sprite redGage;
    private int maxHP = 300;
    private int maxGage = 3;
    private int currentHP = 300;
    private int currentGage = 0;
    [SerializeField] private GameObject intervalDisplay;
    private Text intervalText;
    private const float interval = 5f;
    private float intervalCount = 5f;
    private bool isAttack = false;
    private bool isDied = false;

    // Start is called before the first frame update
    void Start()
    {
        bSManager = bSManagerObject.GetComponent<BattleSceneManager1>();
        lLEnemyRect = lowLevelEnemy.GetComponent<RectTransform>();
        lLEnemyImage = lowLevelEnemy.GetComponent<Image>();
        HPslider = HPbar.GetComponent<Slider>();
        gage1Image = gage1.GetComponent<Image>();
        gage2Image = gage2.GetComponent<Image>();
        gage3Image = gage3.GetComponent<Image>();
        intervalText = intervalDisplay.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (intervalCount > 0 && !isAttack && !isDied)
        {
            intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime);
            intervalText.text = intervalCount.ToString("F2");
        }
        else if (intervalCount == 0 && currentGage < maxGage)
        {
            intervalCount = interval;
            //通常攻撃
            currentGage++;
            StartCoroutine(NormalAttack());
        }
        else if (intervalCount == 0 &&  currentGage == maxGage)
        {
            intervalCount = interval;
            //チャージ技
            currentGage = 0;
            StartCoroutine(ChargeAttack());
        }
    }

    //通常攻撃
    private IEnumerator NormalAttack()
    {
        isAttack = true;
        Vector2 temp = lLEnemyRect.localScale;
        lLEnemyRect.localScale = new(0.8f * temp.x, 0.8f * temp.y);
        yield return new WaitForSeconds(0.3f);
        lLEnemyRect.localScale = new(1.2f * temp.x, 1.2f * temp.y);
        yield return new WaitForSeconds(0.3f);
        float size = 1.2f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.2f:(1フレームごとのサイズ変化)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 0.5f);
            lLEnemyRect.localScale = new(size*temp.x, size*temp.y);
            yield return null;
        }
        CauseDamage(50);
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
    //チャージ技
    private IEnumerator ChargeAttack()
    {
        isAttack = true;
        Vector2 temp = lLEnemyRect.localScale;
        lLEnemyRect.localScale = new(0.7f * temp.x, 0.7f * temp.y);
        yield return new WaitForSeconds(0.3f);
        lLEnemyRect.localScale = new(1.5f * temp.x, 1.5f * temp.y);
        yield return new WaitForSeconds(0.3f);
        float size = 1.5f;
        while (size != 1.0f)
        {
            //0.4f:deltaTime=0.5f:(1フレームごとのサイズ変化)
            size = Mathf.Max(1.0f, size - Time.deltaTime * 1.25f);
            lLEnemyRect.localScale = new(size * temp.x, size * temp.y);
            yield return null;
        }
        CauseDamage(100);
        isAttack = false;
        gage1Image.sprite = grayGage;
        gage2Image.sprite = grayGage;
        gage3Image.sprite = grayGage;
    }

    //ダメージの値を転送
    private void CauseDamage(int damege)
    {
        bSManager.EnemyToSainAttack(damege);
    }
    //ダメージ受け取り
    public void ReceiveDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        HPslider.value = (float)currentHP / maxHP;
        if (currentHP == 0)
        {
            StartCoroutine(Died());
        }
    }

    //消滅
    private IEnumerator Died()
    {
        isDied = true;
        float waitTime = 0.1f;
        float fadeTime = 1;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = lLEnemyImage.color;
            newColor.a = alpha / 255.0f;
            lLEnemyImage.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
        myAllObject.SetActive(false);
        bSManager.EnemyDied();
    }
}
