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
        //�v���C���[���߂Â��Ă��瓮���o��
        if (Mathf.Abs(_transform.position.x - playerTransform.position.x) < eyesightX && Mathf.Abs(_transform.position.y - playerTransform.position.y) < eyesightY)
        {
            startMove = true;
        }
        if (startMove)
        {
            idlingTime += Time.deltaTime;

            //���g�̈ړ�
            if (!isAttack)
            {
                Vector3 pos = _transform.position;
                pos.x += speed * Time.deltaTime;
                _transform.position = pos;
            }

            //�e�̈ړ�
            if (bullet.activeSelf)
            {
                Vector3 pos_bullet = bulletTransform.position;
                pos_bullet.x += speed * Time.deltaTime;
                bulletTransform.position = pos_bullet;
            }

            //�N�[���^�C�����ƂɍU��
            if (idlingTime > coolTime && _collider.enabled)
            {
                idlingTime = 0;
                StartCoroutine(Attack());
            }

            //�v���C���[���\���E�ɂ������������
            if (playerTransform.position.x - _transform.position.x > 2 * eyesightX)
            {
                Destroy(gameObject);
            }
        }
    }

    //�U�����[�V����
    private IEnumerator Attack()
    {
        //�e�̏�Ԃ̏������Ȃ�
        idlingTime = 0;
        bullet.SetActive(false);
        bulletCollider.enabled = true;
        isAttack = true;
        float timeCount = 0;
        while (timeCount < 0.2f)
        {
            Vector3 temp = _transform.rotation.eulerAngles;
            temp.z -= 10 * Time.deltaTime * 5;      //0.2�b�ŐU��グ
            _transform.rotation = Quaternion.Euler(temp);
            timeCount += Time.deltaTime;
            yield return null;
        }
        timeCount = 0;
        yield return new WaitForSeconds(0.2f);
        while (timeCount < 0.2f)
        {
            Vector3 temp = _transform.rotation.eulerAngles;
            temp.z += 10 * Time.deltaTime * 5;      //0.2�b�ŐU�艺��
            _transform.rotation = Quaternion.Euler(temp);
            timeCount += Time.deltaTime;
            yield return null;
        }
        //����
        Vector3 pos = _transform.position;
        pos.x -= 0.5f;
        bulletTransform.position = pos;
        bullet.SetActive(true);
        isAttack = false;
    }

    //�U�����ꂽ�������
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
