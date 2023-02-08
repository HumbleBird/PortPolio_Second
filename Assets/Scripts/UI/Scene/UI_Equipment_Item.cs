using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Equipment_Item : UI_Base
{
    enum Images
    {
        EquipmentIcon
    }

    enum Texts
    {
        ProjectileCountText
    }

	public int m_iItemId { get; private set; }
	public int m_iItemCount { get; private set; }
    public Image  m_itemIcon { get; private set; }
    public EquimentItemCategory eEquimentItemCategory;

    // ���� �� ������ â�� � ī�װ����� �˰� �־�� ��.

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetText((int)Texts.ProjectileCountText).enabled = false;

        return true;
    }

    public void EquipItem(Item item)
    {
        // �����ܰ� ����, ���̵� ������
        RefreshUI();
    }

	public void RefreshUI()
	{
		if (_init == false)
			return;

        // 

	}
}
