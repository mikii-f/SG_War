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
        //�������炢��x���W�A��������y���W�Ƀv���C���[����������U��
        if (Mathf.Abs(_transform.position.x - playerTransform.position.x) < eyesightX && Mathf.Abs(_transform.position.y - 1 - playerTransform.position.y) < eyesightY)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        Vector3 temp = _transform.position;
        //1/3�b�Ńv���C���[�Ƃ̋��������܂œ��B
        while (_transform.position.z > zDistance / 2)
        {
            yield return null;
            temp.z -= Time.deltaTime * zDistance * 3 / 2;
            temp.y += Time.deltaTime * 6;
            _transform.position = temp;
        }
        //1/3�b�Ńv���C���[��z���W�ɓ��B�A���̂܂ܒʂ�߂�1/3�b��ɏ�����
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
