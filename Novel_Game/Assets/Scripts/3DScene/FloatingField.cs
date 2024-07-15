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
        //��Ƀv���C���[(�̑�)����������̉�
        else if (playerTransform.position.y > _transform.position.y)
        {
            if (_collider.isTrigger)
            {
                _collider.isTrigger = false;
            }
        }
        //���̉����Ă���Ƃ��ŁA�v���C���[����̕��Ɏ��܂��Ă���Ԃ͓��߂��Ȃ�
        else if (playerTransform.position.x+playerWidth < _transform.position.x-myWidth || playerTransform.position.x-playerWidth > _transform.position.x+myWidth)
        {
            if (!_collider.isTrigger)
            {
                _collider.isTrigger = true;
            }
        }
    }
}
