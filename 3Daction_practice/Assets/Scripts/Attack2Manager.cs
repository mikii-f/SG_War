using UnityEngine;

public class Attack2Manager : MonoBehaviour
{
    private Collider _collider;
    private Animator _animator;
    private bool isAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //右クリックで強攻撃
        if (Input.GetMouseButtonDown(1) && !isAttacked){
            _animator.SetTrigger("Attack2");
            isAttacked = true;
        }
    }

    //攻撃判定の開始と終わり
    public void Attack2Start()
    {
        _collider.enabled = true;
    }
    public void Attack2Finish()
    {
        _collider.enabled = false;
        isAttacked= false;
    }
}
