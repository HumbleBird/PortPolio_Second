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
        STAMINABarBG,
        STAMINABar,
    }

    enum Texts
    {
        Name
    }

    Player _player;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        return true;
    }

    public void RefreshUI()
    {
        if (_init == false)
            return;

        GameObject player = Managers.Object.Find(1);
        _player = player.GetComponent<Player>();

        Image HpBariamge = GetImage((int)Images.HPBar);
        HpBariamge.fillAmount = _player.m_strStat.m_iHp / _player.m_strStat.m_iMaxHp;
        Image StaminaBariamge = GetImage((int)Images.STAMINABar);
        StaminaBariamge.fillAmount = _player.m_strStat.m_fStemina / _player.m_strStat.m_fMaxStemina;

        TextMeshProUGUI nameText = GetText((int)Texts.Name);
        nameText.text = _player.name;
    }

    public IEnumerator DownHP()
    {
        yield return new WaitForSeconds(0.5f);

        Image HpBarBGHitiamge = GetImage((int)Images.HPBarBGHit);

        while (true)
        {
            HpBarBGHitiamge.fillAmount -= Time.deltaTime;
            if (HpBarBGHitiamge.fillAmount <= _player.m_strStat.m_iHp / _player.m_strStat.m_iMaxHp)
            {
                HpBarBGHitiamge.fillAmount = _player.m_strStat.m_iHp / _player.m_strStat.m_iMaxHp;
                break;
            }

            yield return null;
        }
    }
}
