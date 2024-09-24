using UnityEngine;

public class Enemy2Manager : Enemy3DOrigin
{
    [SerializeField] private GameObject enemy1Prefab;
    private bool startMove = false;
    private Vector3 point1;
    private Vector3 point2;
    private const float coolTime = 10f;
    private float timeCount = 8f;

    protected override void StartSet()
    {
        point1 = _transform.position + new Vector3(0, 2, 0);
        point2 = _transform.position + new Vector3(0, -2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[���߂Â��Ă��瓮���o��
        if (Mathf.Abs(_transform.position.x - playerTransform.position.x) < eyesightX && Mathf.Abs(_transform.position.y - playerTransform.position.y) < eyesightY)
        {
            startMove = true;
        }
        //���B
        if (startMove)
        {
            timeCount += Time.deltaTime;
            if (timeCount > coolTime)
            {
                Instantiate(enemy1Prefab, point1, Quaternion.identity);
                Instantiate(enemy1Prefab, point2, Quaternion.identity);
                timeCount = 0;
            }
        }
        //�v���C���[���\���E�ɂ������������
        if (playerTransform.position.x - _transform.position.x > 2 * eyesightX)
        {
            Destroy(gameObject);
        }
    }

    //�U�����ꂽ�������
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            _collider.enabled = false;
            StartCoroutine(Destroied());
        }
    }
}
