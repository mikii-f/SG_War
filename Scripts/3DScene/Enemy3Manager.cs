using System.Collections;
using UnityEngine;

public class Enemy3Manager : Enemy3DOrigin
{
    private float zDistance;

    protected override void StartSet()
    {
        zDistance = _transform.position.z;
        eyesightX = 3;
        eyesightY = 3;
    }

    void Update()
    {
        //同じぐらいのx座標、少し下のy座標にプレイヤーを見つけたら攻撃
        if (Mathf.Abs(_transform.position.x - playerTransform.position.x) < eyesightX && Mathf.Abs(_transform.position.y - 1 - playerTransform.position.y) < eyesightY)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        Vector3 temp = _transform.position;
        //1/3秒でプレイヤーとの距離半分まで到達
        while (_transform.position.z > zDistance / 2)
        {
            yield return null;
            temp.z -= Time.deltaTime * zDistance * 3 / 2;
            temp.y += Time.deltaTime * 6;
            _transform.position = temp;
        }
        //1/3秒でプレイヤーのz座標に到達、そのまま通り過ぎ1/3秒後に消える
        while (_transform.position.z > -zDistance / 2)
        {
            yield return null;
            temp.z -= Time.deltaTime * zDistance * 3 / 2;
            temp.y -= Time.deltaTime * 6;
            _transform.position = temp;
        }
        Destroy(gameObject);
    }
}
