using System.Collections;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    private Animator _animator;
    private Animator _effectAnimator;
    private Collider _collider;
    private const float hitStopTime = 0.2f;
    public GameObject normalAttackEffect;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _effectAnimator = normalAttackEffect.GetComponent<Animator>();
        _collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //���N���b�N�Œʏ�U��(�{���͂ǂ�����v���C���[�{�̂��瓮�����\��)
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("Attack1");
            _effectAnimator.SetTrigger("NormalAttack");
        }
    }

    //�U�����͂��U�����Ɏ󂯕t���Ȃ�
    public void Attacked()
    {
        _animator.ResetTrigger("Attack1");
        _effectAnimator.ResetTrigger("NormalAttack");
    }

    //�U�����̂ݓ����蔻��
    public void StartAttack()
    {
        _collider.enabled = true;
    }

    public void EndAttack()
    {
        _collider.enabled = false;
    }

    //�q�b�g�X�g�b�v
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _animator.speed = 0f;
            StartCoroutine(HitStopEnd());
        }
    }
    private IEnumerator HitStopEnd()
    {
        yield return new WaitForSeconds(hitStopTime);
        _animator.speed = 1f;
    }
}
