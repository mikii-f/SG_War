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
        //左クリックで通常攻撃(本来はどちらもプレイヤー本体から動かす予定)
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("Attack1");
            _effectAnimator.SetTrigger("NormalAttack");
        }
    }

    //攻撃入力を攻撃中に受け付けない
    public void Attacked()
    {
        _animator.ResetTrigger("Attack1");
        _effectAnimator.ResetTrigger("NormalAttack");
    }

    //攻撃中のみ当たり判定
    public void StartAttack()
    {
        _collider.enabled = true;
    }

    public void EndAttack()
    {
        _collider.enabled = false;
    }

    //ヒットストップ
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
