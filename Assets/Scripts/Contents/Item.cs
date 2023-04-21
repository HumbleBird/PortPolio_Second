using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Define;

public class Item
{
    public Item(ItemType type)
    {
        eItemType = type;
    }

    public int Id                    ;
    public string Name               ;
    public ItemType eItemType        ;
    public string   iconPath           ;
    public string   m_sPrefabPath      ;
    public string   m_sItemDescription ;

    public int Count { get; set; }
    public int m_iPrice { get; set; }
    public int InventorySlot { get; set; }
    public int EquipmentSlot { get; set; } = -1;

    public bool m_bStackable { get; protected set; }
    public bool m_bEquipped { get; set; } = false;
    public bool m_bIsUnarmed = false;

        // 아이템
    public static Item MakeItem(int id)
    {
        Table_Item.Info info = Managers.Table.m_Item.Get(id);

        if (info == null)
            return null;
        
        Item item = new Item(ItemType.None);

        switch (info.m_iItemType)
        {
            case (int)ItemType.Weapon:
                item = new Weapon(info.m_nID);
                break;
            case (int)ItemType.Armor:
                item = new Armor(info.m_nID);
                break;
            case (int)ItemType.Consumable:
                item = new Consumable(info.m_nID);
                break;
            default:
                break;
        }

        item.Id = info.m_nID;
        item.Name = info.m_sName;
        item.eItemType = (ItemType)info.m_iItemType;
        item.iconPath = info.m_sIconPath;
        item.m_sPrefabPath = info.m_sPrefabPath;
        item.m_sItemDescription = info.m_sItemDescription;

        return item;
    }
}

public class Weapon : Item
{
    public WeaponType eWeaponType { get; private set; }
    public int Damage { get; private set; }
    public int DamageReduction { get; private set; }

    // Animation
    public string m_sLight_Attack_1 = "Straight Swords Light Attack 1";
    public string m_sLight_Attack_2 = "Straight Swords Light Attack 2";
    public string m_sHeavy_Attack_1 = "Straight Swords Heavy Attack 1";
    public string m_sHeavy_Attack_2 = "Straight Swords Heavy Attack 2";
    public string m_sRight_Hand_Idle = "Player Right Arm Idle";
    public string m_sLeft_Hand_Idle = "Player Left Arm Idle";
    public string m_sTwo_Hand_Idle = "Player Great Sword Idle";

    public Weapon(int id) : base(ItemType.Weapon)
    {
        Table_Item_Weapon.Info data = Managers.Table.m_Item_Weapon.Get(id);

        if (data == null)
            return;
        {
            Count = 1;
            m_bStackable = false;

            eWeaponType = (WeaponType)data.m_iWeaponType;
            Damage = data.m_iPhysical;
            DamageReduction = data.m_iDamageReduction;
        }
    }

    public Weapon() : base(ItemType.Weapon)
    {
    }
}

public class Armor : Item
{
    public ArmorType eArmorType { get; private set; }
    public float PhysicalResitance { get; set; }

    public Armor(int id) : base(ItemType.Armor)
    {
        Table_Item_Armor.Info data = Managers.Table.m_Item_Armor.Get(id);

        if (data == null)
            return;
        {

            Count = 1;
            m_bStackable = false;

            eArmorType = (ArmorType)data.m_iArmorType;
            PhysicalResitance = data.m_fPhysicalResitance;
        }
    }


}

public class Consumable : Item
{
    public ConsumableType eConsumableType { get; private set; }
    public int Value { get; set; }
    public int MaxCount { get; set; }

    public Consumable(int id) : base(ItemType.Consumable)
    {
        Table_Item_Consumable.Info data = Managers.Table.m_Item_Consumable.Get(id);

        if (data == null)
            return;
        {
            Count = 1;
            m_bStackable = true;

            eConsumableType = (ConsumableType)data.m_iConsumableType;
            Value = data.m_iValue;
            m_bStackable = (data.m_iMaxCount > 1 );
        }
    }


}

