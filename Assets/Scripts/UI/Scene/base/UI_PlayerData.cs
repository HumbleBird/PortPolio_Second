using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_PlayerData : UI_Base
{
    enum Texts
    {
        // Name
        LevelNameText,
        VigorNameText,
        AttunementNameText,
        EnduranceNameText,
        VitalityNameText,
        StrengthNameText,
        DexterityNameText,
        IntenligenceNameText,
        FaithNameText,
        LuckNameText,

        HPNameText,
        FPNameText,
        StaminaNameText,

        EquipLoadNameText,
        PoiseNameText,
        ItemDiscoveryNameText,
        AttunementSlotsNameText,

        // Value
        LevelValueText,
        VigorValueText,
        AttunementValueText,
        EnduranceValueText,
        VitalityValueText,
        StrengthValueText,
        DexterityValueText,
        IntenligenceValueText,
        FaithValueText,
        LuckValueText,

        HPValueText,
        FPValueText,
        StaminaValueText,

        EquipLoadValueText,
        PoiseValueText,
        ItemDiscoveryValueText,
        AttunementSlotsValueText,
    }

    Player _player;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));

        _player = Managers.Object.myPlayer;


        return true;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();
        if(_player == null)
            _player = Managers.Object.myPlayer;


        // 캐릭터 데이터
        TextMeshProUGUI HPValueText = GetText((int)Texts.HPValueText);
        HPValueText.text = _player.m_Stat.m_iHp.ToString() + "/  " + _player.m_Stat.m_iMaxHp.ToString();

        TextMeshProUGUI FPValueText = GetText((int)Texts.FPValueText);
        FPValueText.text = _player.m_Stat.m_iMp.ToString() + "/  " + _player.m_Stat.m_iMaxMp.ToString();

        TextMeshProUGUI StaminaValueText = GetText((int)Texts.StaminaValueText);
        StaminaValueText.text = ((int)_player.m_Stat.m_fStemina).ToString();
    }
}
