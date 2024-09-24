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
        //プレイヤーが近づいてから動き出す
        if (Mathf.Abs(_transform.position.x - playerTransform.position.x) < eyesightX && Mathf.Abs(_transform.position.y - playerTransform.position.y) < eyesightY)
        {
            startMove = true;
        }
        //増殖
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
        //プレイヤーが十分右にいったら消える
        if (playerTransform.position.x - _transform.position.x > 2 * eyesightX)
        {
            Destroy(gameObject);
        }
    }

    //攻撃されたら消える
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            _collider.enabled = false;
            StartCoroutine(Destroied());
        }
    }
}
