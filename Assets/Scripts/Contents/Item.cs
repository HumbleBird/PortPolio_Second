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
    public int Count { get; set; }
    public int Slot { get; set; }
    
    public ItemType eItemType { get; set; }

    public bool m_bStackable { get; protected set; }

    //public int m_iItemEquip { get; set; }
    //public int m_iEquipHerotype { get; set; }
    //public int m_iItemEquip { get; set; }
    //public int m_iItemEquip { get; set; }

    //// DB
    //public int? OwnerId { get; set; }
    //public Player Owner { get; set; }
}

public class Weapon : Item
{
    public WeaponType eWeaponType { get; private set; }
    public int Damage;

    public Weapon(int id) : base(ItemType.Weapon)
    {
        Table_Item.Info item = null;
        item = Managers.Table.m_Item.Get(id);

        if (item.m_iItemType != (int)ItemType.Weapon)
            return;

        Table_Item_Weapon.Info weapon = null;
        weapon = Managers.Table.m_Item_Weapon.Get(item.m_nID);

        if (weapon == null)
            return;

        {
            Id = id;
            Name = weapon.m_sName;
            eItemType = ItemType.Weapon;
            Count = 1;
            eWeaponType = (WeaponType)weapon.m_iWeaponType;
            Damage = weapon.m_fDamage;
            m_bStackable = false;
        }
    }


}

public class Armor : Item
{
    public ArmorType eArmorType { get; private set; }
    public int Defence;

    public Armor(int id) : base(ItemType.Armor)
    {
        Table_Item.Info item = null;
        item = Managers.Table.m_Item.Get(id);

        if (item.m_iItemType != (int)ItemType.Armor)
            return;

        Table_Item_Armor.Info armor = null;
        armor = Managers.Table.m_Item_Armor.Get(item.m_nID);

        if (armor == null)
            return;

        {
            Id = id;
            Name = armor.m_sName;
            eItemType = ItemType.Weapon;
            Count = 1;
            eArmorType = (ArmorType)armor.m_iArmorType;
            Defence = armor.m_fDefense;
            m_bStackable = false;
        }
    }


}

public class Consumable : Item
{
    public ConsumableType eConsumableType { get; private set; }
    public int MaxCount;

    public Consumable(int id) : base(ItemType.Consumable)
    {
        Table_Item.Info item = null;
        item = Managers.Table.m_Item.Get(id);

        if (item.m_iItemType != (int)ItemType.Consumable)
            return;

        Table_Item_Consumable.Info consumable = null;
        consumable = Managers.Table.m_Item_Consumable.Get(item.m_nID);

        if (consumable == null)
            return;

        {
            Id = id;
            Name = consumable.m_sName;
            eItemType = ItemType.Weapon;
            Count = 1;
            MaxCount = consumable.m_iMaxCount;
            eConsumableType = (ConsumableType)consumable.m_iConsumableType;
            m_bStackable = true;
        }
    }


}

