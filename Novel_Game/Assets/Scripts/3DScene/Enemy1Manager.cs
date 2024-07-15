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
    [SerializeField] private LayerMask playerLayer;
    private bool startMove = false;
    private const float eyesightY = 20f;
    private const float eyesightX = 40f;
    private const float speed = -1f;
    private const float coolTime = 5f;
    private float idlingTime = 0f;
    private bool isAttack = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        //ÉvÉåÉCÉÑÅ[Ç™ãﬂÇ√Ç¢ÇƒÇ©ÇÁìÆÇ´èoÇ∑
        if (Physics.BoxCast(_transform.position, new Vector3(1, eyesightY, 1), Vector3.left, Quaternion.identity, eyesightX, playerLayer))
        {
            startMove = true;
        }
        if (startMove)
        {
            idlingTime += Time.deltaTime;

            //é©êgÇÃà⁄ìÆ
            if (!isAttack)
            {
                Vector3 pos = _transform.position;
                pos.x += speed * Time.deltaTime;
                _transform.position = pos;
            }

            //íeÇÃà⁄ìÆ
            if (bullet.activeSelf)
            {
                Vector3 pos_bullet = bulletTransform.position;
                pos_bullet.x += speed * Time.deltaTime;
                bulletTransform.position = pos_bullet;
            }

            //ÉNÅ[ÉãÉ^ÉCÉÄÇ≤Ç∆Ç…çUåÇ
            if (idlingTime > coolTime && _collider.enabled)
            {
                idlingTime = 0;
                StartCoroutine(Attack());
            }
        }
    }

    //çUåÇÉÇÅ[ÉVÉáÉì
    private IEnumerator Attack()
    {
        //íeÇÃèÛë‘ÇÃèâä˙âªÇ»Ç«
        idlingTime = 0;
        bullet.SetActive(false);
        bulletCollider.enabled = true;
        isAttack = true;
        float timeCount = 0;
        while (timeCount < 0.2f)
        {
            Vector3 temp = _transform.rotation.eulerAngles;
            temp.z -= 10 * Time.deltaTime * 5;      //0.2ïbÇ≈êUÇËè„Ç∞
            _transform.rotation = Quaternion.Euler(temp);
            timeCount += Time.deltaTime;
            yield return null;
        }
        timeCount = 0;
        yield return new WaitForSeconds(0.2f);
        while (timeCount < 0.2f)
        {
            Vector3 temp = _transform.rotation.eulerAngles;
            temp.z += 10 * Time.deltaTime * 5;      //0.2ïbÇ≈êUÇËâ∫Ç∞
            _transform.rotation = Quaternion.Euler(temp);
            timeCount += Time.deltaTime;
            yield return null;
        }
        //î≠éÀ
        Vector3 pos = _transform.position;
        pos.x -= 0.5f;
        bulletTransform.position = pos;
        bullet.SetActive(true);
        isAttack = false;
    }

    //çUåÇÇ≥ÇÍÇΩÇÁè¡Ç¶ÇÈ
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
