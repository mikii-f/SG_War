using System.Collections;
using UnityEngine;

public class Enemy1Manager : Enemy3DOrigin
{
    private GameObject bullet;
    private Transform bulletTransform;
    private Collider bulletCollider;
    private bool startMove = false;
    private const float speed = -1f;
    private const float coolTime = 5f;
    private float idlingTime = 0f;
    private bool isAttack = false;

    protected override void StartSet()
    {
        bullet = _transform.GetChild(0).gameObject;
        bulletTransform = bullet.GetComponent<Transform>();
        bulletCollider = bullet.GetComponent<Collider>();
        bullet.SetActive(false);
    }

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
}
