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
        //�v���C���[�̈ʒu���擾(�v���n�u������ƃQ�[���I�u�W�F�N�g�̃A�^�b�`���ł��Ȃ�����)
        if (PlayerFootManager.instance != null)
        {
            playerTransform = PlayerFootManager.instance.transform;
            playerWidth = PlayerFootManager.instance.transform.lossyScale.x / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //���L�[�������Ă���Ԃ͓���
        if (Input.GetKey(KeyCode.S))
        {
            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }
        }
        //���U�����͓���
        else if (PlayerManager.instance.IsAttack2)
        {
            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }
        }
        //�^��Ƀv���C���[(�̑�)����������̉�
        else if (playerTransform.position.y > _transform.position.y�@&& playerTransform.position.x + playerWidth >= _transform.position.x - myWidth && playerTransform.position.x - playerWidth <= _transform.position.x + myWidth)
        {
            if (_collider.isTrigger)
            {
                _collider.isTrigger = false;
            }
        }
        //���艡�ɂ���Ԃ͓���(�v���C���[����̕��Ɏ��܂��Ă���Ԃ͓��߂��Ȃ�)(�䂪�������߁A�������Ȃ��ƒ��n���肪�o��O�ɑ������艺�ɂ�������(?)���߂��Ă��܂�)
        else if (playerTransform.position.x+playerWidth < _transform.position.x-myWidth || playerTransform.position.x-playerWidth > _transform.position.x+myWidth)
        {
            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }
        }
        //�^���ɂ��Ă��\�����ɂ���Ȃ瓧��
        else if (_transform.position.y - playerTransform.position.y > 1)
        {
            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }
        }
    }
}
