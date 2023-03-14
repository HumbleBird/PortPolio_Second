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

    public int m_iItemId { get; private set; } = -1;
	public TextMeshProUGUI m_iItemCount { get; private set; }
    public Image  m_itemIcon { get; private set; }
    public string m_sName { get; private set; }
    public EquimentItemCategory eEquimentItemCategory;
    public bool m_bEquipped = false;

    public int m_iSlot;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        m_itemIcon = GetImage((int)Images.EquipmentIcon);
        m_iItemCount = GetText((int)Texts.ProjectileCountText);

        GetText((int)Texts.ProjectileCountText).enabled = false;

        UI_Equipment pop = transform.GetComponentInParent<UI_Equipment>();
        if (pop == null)
            Debug.Break();

        m_itemIcon.gameObject.BindEvent(() =>
        {
            pop.equipmentNameText.text = m_sName;
        });

        return true;
    }

    public void EquipItem(Item item)
    {
        m_iItemId = item.Id;
        m_iItemCount.text = item.Count.ToString();
        m_itemIcon.enabled = true;
        m_sName = item.Name;
        m_itemIcon.sprite = Managers.Resource.Load<Sprite>(item.iconPath);
        item.EquipmentSlot = m_iSlot;

        m_bEquipped = true;
        m_iItemCount.enabled = false;

        if (item.eItemType == ItemType.Consumable)
        {
            m_iItemCount.enabled = true;
        }

        RefreshUI();
    }

	public override void RefreshUI()
	{
        base.RefreshUI();

	}

    public void UnEquipItem()
    {
        m_iItemId = -1;
        m_iItemCount.enabled = false;
        m_itemIcon.enabled = false;
        m_bEquipped = false;

    }
}
