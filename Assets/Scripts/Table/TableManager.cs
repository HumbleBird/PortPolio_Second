using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TableManager 
{
    public Table_Camera               m_Camera = new Table_Camera();
    public Table_Stat                 m_Stat = new Table_Stat();
    public Table_Attack               m_Attack = new Table_Attack();
    public Table_Player               m_Player = new Table_Player();
    public Table_Boss                 m_Boss = new Table_Boss();
    public Table_AI                   m_AI = new Table_AI();
    public Table_Monster              m_Monster = new Table_Monster();
    public Table_Item                 m_Item = new Table_Item();
    public Table_Item_Weapon          m_Item_Weapon = new Table_Item_Weapon();
    public Table_Item_Armor           m_Item_Armor = new Table_Item_Armor();
    public Table_Item_Consumable      m_Item_Consumable = new Table_Item_Consumable();
    public Table_Reward               m_Reward = new Table_Reward();
    public Table_MonsterSpawnPos      m_MonsterSpawnPos = new Table_MonsterSpawnPos();
    public Table_Sound                m_Sound = new Table_Sound();
    public Table_Dialogue             m_Dialogue = new Table_Dialogue();
    public Table_Shop                 m_Shop = new Table_Shop();

    public void Init()
    {
#if UNITY_EDITOR
        m_Camera.Init_CSV("Camera", 2, 0);
        m_Stat.Init_CSV("Stat", 2, 0);
        m_Attack.Init_CSV("Attack", 2, 0);
        m_Player.Init_CSV("Player", 2, 0);
        m_Boss.Init_CSV("Boss", 2, 0);
        m_AI.Init_CSV("AI", 2, 0);
        m_Monster.Init_CSV("Monster", 2, 0);
        m_Item.Init_CSV("Item", 2, 0);
        m_Item_Weapon.Init_CSV("Weapon", 2, 0);
        m_Item_Armor.Init_CSV("Armor", 2, 0);
        m_Item_Consumable.Init_CSV("Consumable", 2, 0);
        m_Reward.Init_CSV("Reward", 2, 0);
        m_MonsterSpawnPos.Init_CSV("MonsterSpawnPos", 2, 0);
        m_Sound.Init_CSV("Sound", 2, 0);
        m_Dialogue.Init_CSV("Dialogue", 2, 0);
        m_Shop.Init_CSV("Shop", 2, 0);
#else
        m_Camera.Init_Binary("Camera");
#endif
    }

    public void Save()
    {
        m_Camera.Save_Binary("Camera");
        m_Stat.Save_Binary("Stat");
        m_Attack.Save_Binary("Attack");
        m_Player.Save_Binary("Player");
        m_Boss.Save_Binary("Boss");
        m_AI.Save_Binary("AI");
        m_Monster.Save_Binary("Monster");
        m_Item.Save_Binary("Item");
        m_Item_Weapon.Save_Binary("Weapon");
        m_Item_Armor.Save_Binary("Armor");
        m_Item_Consumable.Save_Binary("Consumable");
        m_Reward.Save_Binary("Reward");
        m_MonsterSpawnPos.Save_Binary("MonsterSpawnPos");
        m_Sound.Save_Binary("Sound");
        m_Dialogue.Save_Binary("Dialogue");
        m_Shop.Save_Binary("Shop");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    public void Clear()
    {

    }
}
