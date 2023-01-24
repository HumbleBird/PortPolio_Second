using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerData : UI_Base
{
    enum Texts
    {
        LevelValueText,
        MaxHpValueText,
        CurrentHpValueText,
        MaxMpValueText,
        CurrentMpValueText,
        AtkValueText,
        DefenceValueText,
        AttackSpeedValueText,
        MoveSpeedValueText
    }

    Player _player;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));

        return true;
    }

    public void SetInfo(GameObject player)
    {
        _player = player.GetComponent<Player>();

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        // 캐릭터 데이터
        TextMeshProUGUI LevelValueText = GetText((int)Texts.LevelValueText);
        LevelValueText.text = _player.m_strStat.m_iLevel.ToString();

        TextMeshProUGUI MaxHpValueText = GetText((int)Texts.MaxHpValueText);
        MaxHpValueText.text = _player.m_strStat.m_fMaxHp.ToString();

        TextMeshProUGUI CurrentHpValueText = GetText((int)Texts.CurrentHpValueText);
        CurrentHpValueText.text = _player.m_strStat.m_fHp.ToString();

        TextMeshProUGUI MaxMpValueText = GetText((int)Texts.MaxMpValueText);
        MaxMpValueText.text = _player.m_strStat.m_fMaxMp.ToString();

        TextMeshProUGUI CurrentMpValueText = GetText((int)Texts.CurrentMpValueText);
        CurrentMpValueText.text = _player.m_strStat.m_fMp.ToString();

        TextMeshProUGUI AtkValueText = GetText((int)Texts.AtkValueText);
        AtkValueText.text = _player.m_strStat.m_fAtk.ToString();

        TextMeshProUGUI DefenceValueText = GetText((int)Texts.DefenceValueText);
        DefenceValueText.text = _player.m_strStat.m_fDef.ToString();

        TextMeshProUGUI AttackSpeedValueText = GetText((int)Texts.AttackSpeedValueText);
        AttackSpeedValueText.text = _player.m_strStat.m_fAtk.ToString();

        TextMeshProUGUI MoveSpeedValueText = GetText((int)Texts.MoveSpeedValueText);
        MoveSpeedValueText.text = _player.m_strStat.m_fMoveSpeed.ToString();

    }
}
