using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Base
{
    enum Images
    {
        HPBarBG,
        HPBarBGHit,
        HPBar,
    }

    enum Texts
    {
        DamageValue
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        HitDamageValue = GetText((int)Texts.DamageValue);
        HitDamageValue.enabled = false;

        return true;
    }

    Stat m_Stat;
    TextMeshProUGUI HitDamageValue;
    int preHp;

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;
    }

    public override void RefreshUI()
    {
        base.RefreshUI();

        Character character = GetComponentInParent<Character>();
        m_Stat = character.m_Stat;
        preHp = m_Stat.m_iHp;

        Image HpBariamge = GetImage((int)Images.HPBar);

        HpBariamge.fillAmount = m_Stat.m_iHp / (float)m_Stat.m_iMaxHp;

        StartCoroutine(DownHP());
        StartCoroutine(DamageValueUp());
    }

    IEnumerator DownHP()
    {
        yield return new WaitForSeconds(0.5f);

        Image HpBarBGHitiamge = GetImage((int)Images.HPBarBGHit);

        while (true)
        {
            HpBarBGHitiamge.fillAmount -= Time.deltaTime;
            if (HpBarBGHitiamge.fillAmount <= m_Stat.m_iHp / m_Stat.m_iMaxHp)
            {
                HpBarBGHitiamge.fillAmount = m_Stat.m_iHp / m_Stat.m_iMaxHp;
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator DamageValueUp()
    {
        HitDamageValue.enabled = true;

        float currentdamageValue = 0;
        float damage = 0;

        // 데미지 값 = 이전에 저장해 뒀던 체력 값 - 현재 체력 값
        if (preHp != m_Stat.m_iHp)
        {
            damage = preHp - m_Stat.m_iHp;
            preHp = m_Stat.m_iHp;
        }

        // 데미지 수치UI 올려 버리기
        while (true)
        {
            currentdamageValue += 100*Time.deltaTime;
            HitDamageValue.text = ((int)currentdamageValue).ToString();
            if (currentdamageValue >= damage)
            {
                yield return new WaitForSeconds(3f);
                HitDamageValue.enabled = false;
                yield break;
            }

            yield return null;
        }
    }
}
