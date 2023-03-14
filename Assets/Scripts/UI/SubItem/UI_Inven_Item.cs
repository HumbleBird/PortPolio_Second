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

        itemCount    = GetText((int)Texts.ItemCountText);
        m_iItemPrice = GetText((int)Texts.ItemPriceText);
        itemIcon     = GetImage((int)Images.InventoryItemIcon);
        itemUseIcon  = GetImage((int)Images.UsingItemCheckIcon);

        itemCount.enabled    = false;
        m_iItemPrice.enabled = false;
        itemIcon.enabled     = false;
        itemUseIcon.enabled  = false;

        return true;
    }

    public void Shop(bool OnOff)
    {
        if(OnOff == true)
        {
            m_iItemPrice.enabled = true;

            itemCount.text = m_iCount.ToString();
            itemCount.enabled = true;
        }
        else
        {
            m_iItemPrice.enabled= false;
            itemCount.enabled = true;

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

            itemIcon.enabled = false;
            itemCount.enabled = false;
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
            itemIcon.enabled = true;

            //if (item.eItemType == ItemType.Consumable)
            //{
            //    itemCount.text = m_iCount.ToString();
            //    itemCount.gameObject.SetActive(true);
            //}

            itemUseIcon.enabled = m_bEquipped;
        }

        SetPurposeofUse();
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
            // 상점 창에서 아이템을 클릭하면 사기
            else if (eOpenWhat == OpenWhat.Shop)
            {
                UI_UseQuestions popup = Managers.UI.ShowPopupUI<UI_UseQuestions>();
                popup.SetQeustion($"Do you Buy Item? \n {newitem.Name} \n Price {m_iItemPrice.text}");
                // 버튼 누름
                //Coroutine co = StartCoroutine(InputButton());

                // 클릭
                popup.m_sYesText.gameObject.BindEvent(() => 
                {
                    // 나중에 수량 추가 가능하게
                    Managers.Battle.Buytem(Managers.Object.myPlayer, newitem, 1);
                    //StopCoroutine(co);
                });

                popup.m_sNoText.gameObject.BindEvent(() => 
                { 
                    Managers.UI.ClosePopupUI(); 
                    //StopCoroutine(co);
                });
            }
        });
    }

    //IEnumerator InputButton()
    //{
    //    while (true)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Y))
    //        {
    //            Managers.Battle.Buytem(Managers.Object.myPlayer, newitem, 1);
    //            yield break;
    //        }
    //        else if (Input.GetKeyDown(KeyCode.N))
    //        {
    //            popup.m_sNoText.gameObject.BindEvent(() => { Managers.UI.ClosePopupUI(); });
    //            yield break;
    //        }

    //        yield return null;
    //    }
    //}
}
