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

    public int Id { get; set; }
    public string Name { get; set; }
    public ItemType eItemType { get; set; }
    public CharacterClass eCharacterClass { get; set; }
    public string iconPath { get; set; }

    public int Count { get; set; }
    public int m_iPrice { get; set; }
    public int InventorySlot { get; set; }
    public int EquipmentSlot { get; set; } = -1;

    public bool m_bStackable { get; protected set; }
    public bool m_bEquipped { get; set; } = false;

    // 아이템
    public static Item MakeItem(Table_Item.Info info)
    {
        Item item = null;

        if (info == null)
            return null;

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

        return item;
    }
}

public class Weapon : Item
{
    public WeaponType eWeaponType { get; private set; }
    public int Damage { get; private set; }
    public float AttackSpeed { get; private set; }

    public Weapon(int id) : base(ItemType.Weapon)
    {
        Table_Item.Info item = null;
        item = Managers.Table.m_Item.Get(id);

        if (item.m_iItemType != (int)ItemType.Weapon)
            return;

        Table_Item_Weapon.Info data = null;
        data = Managers.Table.m_Item_Weapon.Get(item.m_nID);

        if (data == null)
            return;

        {
            Id = id;
            Name = data.m_sName;
            eItemType = (ItemType)item.m_iItemType;
            eCharacterClass = (CharacterClass)item.m_iCharacterClass;
            iconPath = item.m_sIconPath;

            Count = 1;
            m_bStackable = false;

            eWeaponType = (WeaponType)data.m_iWeaponType;
            Damage = data.m_fDamage;
            AttackSpeed = data.m_fAttackSpeed;
        }
    }
}

public class Armor : Item
{
    public ArmorType eArmorType { get; private set; }
    public int Defence { get; set; }
    public float MoveSpeed { get; set; }

    public Armor(int id) : base(ItemType.Armor)
    {
        Table_Item.Info item = null;
        item = Managers.Table.m_Item.Get(id);

        if (item.m_iItemType != (int)ItemType.Armor)
            return;

        Table_Item_Armor.Info data = null;
        data = Managers.Table.m_Item_Armor.Get(item.m_nID);

        if (data == null)
            return;

        {
            Id = id;
            Name = data.m_sName;
            eItemType = (ItemType)item.m_iItemType;
            eCharacterClass = (CharacterClass)item.m_iCharacterClass;
            iconPath = item.m_sIconPath;

            Count = 1;
            m_bStackable = false;

            eArmorType = (ArmorType)data.m_iArmorType;
            Defence = data.m_fDefense;
            MoveSpeed = data.m_fMoveSpeed;
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
        Table_Item.Info item = null;
        item = Managers.Table.m_Item.Get(id);

        if (item.m_iItemType != (int)ItemType.Consumable)
            return;

        Table_Item_Consumable.Info data = null;
        data = Managers.Table.m_Item_Consumable.Get(item.m_nID);

        if (data == null)
            return;

        {
            Id = id;
            Name = data.m_sName;
            eItemType = (ItemType)item.m_iItemType;
            eCharacterClass = (CharacterClass)item.m_iCharacterClass;
            iconPath = item.m_sIconPath;

            Count = 1;
            m_bStackable = true;

            eConsumableType = (ConsumableType)data.m_iConsumableType;
            Value = data.m_iValue;
            m_bStackable = (data.m_iMaxCount > 1 );
        }
    }


}

