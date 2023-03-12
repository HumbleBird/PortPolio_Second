using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Inven_Item : UI_Base
{
    enum Texts
    {
        ItemCountText,
        ItemPriceText
    }

    enum Images
    {
        InventoryItemIcon,
        UsingItemCheckIcon
    }

    public TextMeshProUGUI m_iItemPrice { get; private set; }
    public OpenWhat eOpenWhat = OpenWhat.None;

    TextMeshProUGUI itemCount;
    Image itemIcon;
    Image itemUseIcon;

    public int m_iItemID { get; private set; }
    public int m_iSlot { get; private set; }
    public int m_iCount { get; private set; }
    public bool m_bEquipped { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false; 

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        itemCount = GetText((int)Texts.ItemCountText);
        itemCount.gameObject.SetActive(false);

        m_iItemPrice = GetText((int)Texts.ItemPriceText);
        m_iItemPrice.enabled = false;

        itemIcon = GetImage((int)Images.InventoryItemIcon);

        itemIcon.gameObject.SetActive(false);

        itemUseIcon = GetImage((int)Images.UsingItemCheckIcon);
        itemUseIcon.gameObject.SetActive(false);

        gameObject.BindEvent(() => { Debug.Log(eOpenWhat); });

        return true;
    }

    public void Shop(bool OnOff)
    {
        if(OnOff == true)
        {
            m_iItemPrice.enabled = true;

            itemCount.text = m_iCount.ToString();
            itemCount.gameObject.SetActive(true);
        }
        else
        {
            m_iItemPrice.enabled= false;
            itemCount.gameObject.SetActive(true);

        }
    }

    public void SetItem(Item item)
    {
        if(item == null)
        {
            m_iItemID = 0;
            m_iSlot = 0;
            m_iCount = 0;
            m_bEquipped = false;

            itemIcon.gameObject.SetActive(false);
            itemCount.gameObject.SetActive(false);
            m_iItemPrice.text = "";
        }
        else
        {
            m_iItemID = item.Id;
            m_iSlot = item.InventorySlot;
            m_iCount = item.Count;
            m_bEquipped = item.m_bEquipped;
            m_iItemPrice.text = item.m_iPrice.ToString();

            Table_Item.Info itemData = null;
            itemData = Managers.Table.m_Item.Get(m_iItemID);

            itemIcon.sprite = Managers.Resource.Load<Sprite>(itemData.m_sIconPath);
            itemIcon.gameObject.SetActive(true);

            //if (item.eItemType == ItemType.Consumable)
            //{
            //    itemCount.text = m_iCount.ToString();
            //    itemCount.gameObject.SetActive(true);
            //}

            itemUseIcon.gameObject.SetActive(m_bEquipped);
        }
    }

    public void SetPurposeofUse()
    {
        Item newitem = new Item(Define.ItemType.None);
        newitem.Id = m_iItemID;
        newitem.InventorySlot = m_iSlot;

        newitem.eItemType = (ItemType)Managers.Table.m_Item.Get(newitem.Id).m_iItemType;

        itemIcon.gameObject.BindEvent(() =>
        {
            // 인벤토리에서 창을 클릭하면 아이템을 착용
            if (eOpenWhat == OpenWhat.Inventory)
            {
                newitem.m_bEquipped = !m_bEquipped;
                Managers.Battle.EquipItem(Managers.Object.myPlayer, newitem);
            }
            else if (eOpenWhat == OpenWhat.Shop)
            {
                UI_UseQuestions popup = Managers.UI.ShowPopupUI<UI_UseQuestions>();
                popup.SetQeustion($"아이템을 구매하시겠습니까? \n {newitem.Name} \n 가격 {m_iItemPrice.text}");
                Coroutine co = StartCoroutine( Managers.Battle.IStandAction(() => 
                {
                    if(Input.GetKeyDown(KeyCode.Y))
                        
                }));
                // 살지 말지 Yes Or No 창을 띄우고
                // Yes면 배틀매니저에서 ButItem을 띄움.

                //newitem.eItemType = (ItemType)Managers.Table.m_Item.Get(newitem.Id).m_iItemType;

                //Managers.Battle.EquipItem(Managers.Object.myPlayer, newitem);

                Debug.Log("상점에서 열엇음.");
            }
        });
    }
}
