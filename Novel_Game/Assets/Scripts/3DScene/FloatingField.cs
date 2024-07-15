using UnityEngine;

public class FloatingField : MonoBehaviour
{
    private Collider _collider;
    private Transform _transform;
    [SerializeField] private LayerMask footLayer;
    private Transform playerTransform;
    private float playerWidth;
    private float myWidth;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _transform = GetComponent<Transform>();
        myWidth = _transform.lossyScale.x / 2;
        //プレイヤーの位置を取得(プレハブ化するとゲームオブジェクトのアタッチができないため)
        if (PlayerFootManager.instance != null)
        {
            playerTransform = PlayerFootManager.instance.transform;
            playerWidth = PlayerFootManager.instance.transform.lossyScale.x / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //下キーを押している間は透過
        if (Input.GetKey(KeyCode.S))
        {
            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }
        }
        //強攻撃中は透過
        else if (PlayerManager.instance.IsAttack2)
        {
            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }
        }
        //上にプレイヤー(の足)がきたら実体化
        else if (playerTransform.position.y > _transform.position.y)
        {
            if (_collider.isTrigger)
            {
                _collider.isTrigger = false;
            }
        }
        //実体化しているときで、プレイヤーが台の幅に収まっている間は透過しない
        else if (playerTransform.position.x+playerWidth < _transform.position.x-myWidth || playerTransform.position.x-playerWidth > _transform.position.x+myWidth)
        {
            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }
        }
    }
}
