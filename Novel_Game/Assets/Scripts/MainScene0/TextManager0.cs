using System.IO;
using UnityEngine;

public class TextManager0 : TextManagerOrigin
{
    //���̂Ƃ���ǂ�TextManager�������͂��炫�����邽�߁A�t�@�C�����ƃV�[������R�t����΃V�[�����Ƃɕ�����K�v�͂Ȃ�
    //���A����ς������Ȃ�\�������邽�߂Ƃ肠�������̂܂܂�
    private void Awake()
    {
        StreamReader reader = new(Application.dataPath + "/StreamingAssets/Script0.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(reader.ReadLine().Split(','));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
}
