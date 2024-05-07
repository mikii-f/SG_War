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
        //���L�[(S)�������Ă���ԋy�уv���C���[���ߒ��͓���
        if (Input.GetKey(KeyCode.S) || isPlayerPassing)
        {
            _collider.isTrigger = true;
        }
        //�^��Ƀv���C���[����������̉�
        else if (Physics.BoxCast(transform.position, new Vector3(transform.localScale.x * 0.5f, 0.01f, transform.localScale.z * 0.5f),  Vector3.up, Quaternion.identity, 0.5f, playerLayer))
        {
            _collider.isTrigger = false;
        }
        //�v���C���[�����ɂ���Ԃ͂��蔲��
        if (Physics.BoxCast(transform.position, transform.localScale, Vector3.down, Quaternion.identity, 3.0f, playerLayer))
        {
            _collider.isTrigger = true;
        }
    }

    //�v���C���[���~�肽�瓧��
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            _collider.isTrigger = true;
        }
    }

    //�v���C���[�ʉߌ���ق�̏������̉����Ȃ����ƂŁA������ʉߎ��ɍ��W�����܂��܏d�Ȃ��Ă��n�㔻��𔭐������Ȃ�
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
