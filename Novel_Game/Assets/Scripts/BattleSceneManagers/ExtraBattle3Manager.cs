using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtraBattle3Manager : BattleSceneManagerOrigin
{
    protected override void StartSet()
    {
        
    }

    public override void SceneLoad()
    {
        SceneManager.LoadScene("AfterClear");
    }
}