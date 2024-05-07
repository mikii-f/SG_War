using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Transform _transform;
    private Collider _collider;
    private Animator _animeator;
    private GameObject _bullet;
    private Transform _bulletTransform;
    private Collider _bulletCollider;
    [SerializeField] private LayerMask playerLayer;
    private bool startMove = false;
    private const float eyesightY = 10f;
    private const float eyesightX = 40f;
    private const float speed = -2f;
    private const float coolTime = 4f;
    private float idlingTime = 0f;
    private bool isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _animeator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _bullet = _transform.GetChild(0).gameObject;
        _bulletTransform = _bullet.GetComponent<Transform>();
        _bulletCollider = _bullet.GetComponent <Collider>();
        _bullet.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[���߂Â��Ă��瓮���o��
        if (Physics.BoxCast(_transform.position, new Vector3(1,eyesightY,1), Vector3.left, Quaternion.identity, eyesightX, playerLayer))
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
            if (_bullet.activeSelf)
            {
                Vector3 pos_bullet = _bulletTransform.position;
                pos_bullet.x += speed * Time.deltaTime;
                _bulletTransform.position = pos_bullet;
            }

            //�N�[���^�C�����ƂɍU��
            if (idlingTime > coolTime && _collider.enabled)
            {
                idlingTime = 0;
                _animeator.SetTrigger("Attack");
            }
        }
    }

    //�U�����ꂽ�������
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            _animeator.SetBool("Destroied", true);
            _collider.enabled = false;
            _bullet.SetActive(false);
            StartCoroutine(Destroied());
        }
    }

    private IEnumerator Destroied()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    //�U������
    public void AttackSet()
    {
        _bullet.SetActive(false);
        _bulletCollider.enabled = true;
        isAttack = true;
    }
    //�U������
    public void Attack()
    {
        Vector3 pos = _transform.position;
        pos.x -= 1.5f;
        _bulletTransform.position = pos;
        _bullet.SetActive(true);
        isAttack = false;
    }
}
