using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInfo : UI_Scene
{
    enum Images
    {
        HPBarBG,
        HPBarBGHit,
        HPBar,
        MPBarBG,
        MPBar,
        STAMINABarBG,
        STAMINABar,

        QuickSlotIcon,
        SpellSlotIcon,
        RightHandSlotIcon,
        LeftHandSlotIcon,


    }

    Player _player;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));

        return true;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        _player = Managers.Object.myPlayer;

        // HP, MP, Stemina
        Image HpBariamge = GetImage((int)Images.HPBar);
        HpBariamge.fillAmount = _player.m_Stat.m_iHp / (float)_player.m_Stat.m_iMaxHp;
        Image StaminaBariamge = GetImage((int)Images.STAMINABar);
        StaminaBariamge.fillAmount = _player.m_Stat.m_fStemina / (float)_player.m_Stat.m_fMaxStemina;
        Image MpBariamge = GetImage((int)Images.MPBar);
        MpBariamge.fillAmount = _player.m_Stat.m_iMp / (float)_player.m_Stat.m_iMaxMp;
    }

    public void RefreshItem()
    {
        _player = Managers.Object.myPlayer;

        // Quick Slots
        // 플레이어가 현재 장착한 무기의 아이콘을 여기에 업로드
        Image rightWeaponIcon = GetImage((int)Images.RightHandSlotIcon);
        Image LeftWeaponIcon = GetImage((int)Images.LeftHandSlotIcon);

        if (_player.m_RightWeapon.Name == null)
        {
            rightWeaponIcon.sprite = null;
            rightWeaponIcon.enabled = false;
        }
        else
        {
            rightWeaponIcon.sprite = Managers.Resource.Load<Sprite>(_player.m_RightWeapon.iconPath);
            rightWeaponIcon.enabled = true;
        }

        if (_player.m_LeftWeapon.Name == null)
        {
            LeftWeaponIcon.sprite = null;
            LeftWeaponIcon.enabled = false;
        }
        else
        {
            LeftWeaponIcon.sprite = Managers.Resource.Load<Sprite>(_player.m_LeftWeapon.iconPath);
            LeftWeaponIcon.enabled = true;
        }
    }

    public IEnumerator DownHP()
    {
        yield return new WaitForSeconds(0.5f);

        Image HpBarBGHitiamge = GetImage((int)Images.HPBarBGHit);

        while (true)
        {
            HpBarBGHitiamge.fillAmount -= Time.deltaTime;
            if (HpBarBGHitiamge.fillAmount <= _player.m_Stat.m_iHp / _player.m_Stat.m_iMaxHp)
            {
                HpBarBGHitiamge.fillAmount = _player.m_Stat.m_iHp / _player.m_Stat.m_iMaxHp;
                break;
            }

            yield return null;
        }
    }
}
