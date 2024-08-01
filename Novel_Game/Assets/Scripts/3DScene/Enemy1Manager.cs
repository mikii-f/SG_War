using System.Collections;
using UnityEngine;

public class Enemy1Manager : MonoBehaviour
{
    private Transform _transform;
    private Collider _collider;
    private SpriteRenderer spriteRenderer;
    private GameObject bullet;
    private Transform bulletTransform;
    private Collider bulletCollider;
    private bool startMove = false;
    private const float eyesightY = 20f;
    private const float eyesightX = 20f;
    private const float speed = -1f;
    private const float coolTime = 5f;
    private float idlingTime = 0f;
    private bool isAttack = false;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bullet = _transform.GetChild(0).gameObject;
        bulletTransform = bullet.GetComponent<Transform>();
        bulletCollider = bullet.GetComponent<Collider>();
        bullet.SetActive(false);
        //プレイヤーの位置を取得
        if (PlayerFootManager.instance != null)
        {
            playerTransform = PlayerFootManager.instance.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが近づいてから動き出す
        if (Mathf.Abs(_transform.position.x - playerTransform.position.x) < eyesightX && Mathf.Abs(_transform.position.y - playerTransform.position.y) < eyesightY)
        {
            startMove = true;
        }
        if (startMove)
        {
            idlingTime += Time.deltaTime;

            //自身の移動
            if (!isAttack)
            {
                Vector3 pos = _transform.position;
                pos.x += speed * Time.deltaTime;
                _transform.position = pos;
            }

            //弾の移動
            if (bullet.activeSelf)
            {
                Vector3 pos_bullet = bulletTransform.position;
                pos_bullet.x += speed * Time.deltaTime;
                bulletTransform.position = pos_bullet;
            }

            //クールタイムごとに攻撃
            if (idlingTime > coolTime && _collider.enabled)
            {
                idlingTime = 0;
                StartCoroutine(Attack());
            }

            //プレイヤーが十分右にいったら消える
            if (playerTransform.position.x - _transform.position.x > 2 * eyesightX)
            {
                Destroy(gameObject);
            }
        }
    }

    //攻撃モーション
    private IEnumerator Attack()
    {
        //弾の状態の初期化など
        idlingTime = 0;
        bullet.SetActive(false);
        bulletCollider.enabled = true;
        isAttack = true;
        float timeCount = 0;
        while (timeCount < 0.2f)
        {
            Vector3 temp = _transform.rotation.eulerAngles;
            temp.z -= 10 * Time.deltaTime * 5;      //0.2秒で振り上げ
            _transform.rotation = Quaternion.Euler(temp);
            timeCount += Time.deltaTime;
            yield return null;
        }
        timeCount = 0;
        yield return new WaitForSeconds(0.2f);
        while (timeCount < 0.2f)
        {
            Vector3 temp = _transform.rotation.eulerAngles;
            temp.z += 10 * Time.deltaTime * 5;      //0.2秒で振り下げ
            _transform.rotation = Quaternion.Euler(temp);
            timeCount += Time.deltaTime;
            yield return null;
        }
        //発射
        Vector3 pos = _transform.position;
        pos.x -= 0.5f;
        bulletTransform.position = pos;
        bullet.SetActive(true);
        isAttack = false;
    }

    //攻撃されたら消える
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            _collider.enabled = false;
            bullet.SetActive(false);
            StartCoroutine(Destroied());
        }
    }

    private IEnumerator Destroied()
    {
        StartCoroutine(FadeOut());
        Vector3 temp1 = _transform.position, temp2 = _transform.rotation.eulerAngles;
        temp2.z = -10;
        _transform.rotation = Quaternion.Euler(temp2);
        temp1.x += 0.2f;
        _transform.position = temp1;
        yield return new WaitForSeconds(0.2f);
        temp1.x -= 0.4f;
        _transform.position = temp1;
        yield return new WaitForSeconds(0.2f);
        temp1.x += 0.4f;
        _transform.position = temp1;
        yield return new WaitForSeconds(0.2f);
        temp1.x -= 0.4f;
        _transform.position = temp1;
        yield return new WaitForSeconds(0.2f);
        temp1.x += 0.2f;
        _transform.position = temp1;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

    //扱うのがImageでないため共通化FadeOutは使えない
    private IEnumerator FadeOut()
    {
        float waitTime = 0.1f;
        float fadeTime = 1;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = spriteRenderer.color;
            newColor.a = alpha / 255.0f;
            spriteRenderer.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
