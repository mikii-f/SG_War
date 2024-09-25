using System.Collections;
using UnityEngine;

public abstract class Enemy3DOrigin : MonoBehaviour
{
    protected Transform _transform;
    protected Collider _collider;
    protected float eyesightY = 20f;
    protected float eyesightX = 20f;
    private SpriteRenderer spriteRenderer;
    protected Transform playerTransform;
    private AudioSource seSource;
    [SerializeField] private AudioClip seDamage;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        seSource = GetComponent<AudioSource>();
        seSource.volume = GameManager.instance.SeVolume;
        //プレイヤーの位置を取得
        if (PlayerFootManager.instance != null)
        {
            playerTransform = PlayerFootManager.instance.transform;
        }
        StartSet();
    }

    protected abstract void StartSet();

    protected IEnumerator Destroied()
    {
        StartCoroutine(FadeOut());
        seSource.clip = seDamage;
        seSource.Play();
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
        float fadeTime = 1;
        Color temp = spriteRenderer.color;
        temp.a = 1;
        spriteRenderer.color = temp;
        while (spriteRenderer.color.a > 0)
        {
            yield return new WaitForSeconds(0.1f);
            temp = spriteRenderer.color;
            temp.a = Mathf.Max(0, temp.a - 0.1f / fadeTime);
            spriteRenderer.color = temp;
        }
    }
}
