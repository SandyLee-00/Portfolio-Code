using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatusScene : UI_Scene
{
    public enum GameObjects
    {
        HPBar,
        MPBar
    }
    public enum Texts
    {
        LevelText,
        PlayerNameText
    }

    public GameObject player;
    private PlayerStatusSystem playerStatusSystem;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Bind<GameObject>(typeof(GameObjects));
        BindText(typeof(Texts));

        player = GameObject.FindGameObjectsWithTag("Player")[0];
        playerStatusSystem = player.GetComponent<PlayerStatusSystem>();
        playerStatusSystem.OnChangeStatus += RefreshUI;

        return true;
    }

    private void RefreshUI()
    {
        SetHPBar();
        SetMPBar();
    }

    private void SetHPBar()
    {
        float hp = playerStatusSystem.CurrentHP;
        float maxHP = playerStatusSystem.MaxHP;

        float ratio = hp / maxHP;

        GameObject hpBar = Get<GameObject>((int)GameObjects.HPBar);
        hpBar.GetComponent<Slider>().value = ratio;
    }

    private void SetMPBar()
    {
        float mp = playerStatusSystem.CurrentMP;
        float maxMP = playerStatusSystem.MaxMP;

        float ratio = mp / maxMP;

        GameObject mpBar = Get<GameObject>((int)GameObjects.MPBar);
        mpBar.GetComponent<Slider>().value = ratio;
        
    }

}
