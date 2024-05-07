using System.Collections;
using UnityEngine;

public class FloatingField : MonoBehaviour
{
    private Collider _collider;
    [SerializeField] private LayerMask playerLayer;
    bool isPlayerPassing = false;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        //下キー(S)を押している間及びプレイヤー透過中は透過
        if (Input.GetKey(KeyCode.S) || isPlayerPassing)
        {
            _collider.isTrigger = true;
        }
        //真上にプレイヤーがきたら実体化
        else if (Physics.BoxCast(transform.position, new Vector3(transform.localScale.x * 0.5f, 0.01f, transform.localScale.z * 0.5f),  Vector3.up, Quaternion.identity, 0.5f, playerLayer))
        {
            _collider.isTrigger = false;
        }
        //プレイヤーが下にいる間はすり抜け
        if (Physics.BoxCast(transform.position, transform.localScale, Vector3.down, Quaternion.identity, 3.0f, playerLayer))
        {
            _collider.isTrigger = true;
        }
    }

    //プレイヤーが降りたら透過
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            _collider.isTrigger = true;
        }
    }

    //プレイヤー通過後もほんの少し実体化しないことで、下から通過時に座標がたまたま重なっても地上判定を発生させない
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            isPlayerPassing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CoolTime());
        }
    }
    IEnumerator CoolTime()
    {
        yield return null;
        isPlayerPassing = false;
    }
}
