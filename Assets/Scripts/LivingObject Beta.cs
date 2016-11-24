using System.Collections.Generic;
using UnityEngine;

public abstract class LivingObject : MonoBehaviour
{
    protected sealed class Stats
    {
        public int
            STR, AGI, INT,
            AtkDmg, DEF, MagRes,
            AtkSpd, MovSpd,
            MaxHP, MaxMP;

        public float CritChc, HPRegen, MPRegen;

        public Stats(BaseStats baseStats)
        {
            Add(baseStats);
        }

        public void Add(BaseStats baseStats)
        {
            STR += baseStats.STR;
            AtkDmg += baseStats.STR;
            DEF += baseStats.STR;
            MaxHP += baseStats.STR * 2;
            HPRegen += baseStats.HPRegen * 0.01f;

            AGI += baseStats.AGI;
            AtkDmg += baseStats.AGI;
            CritChc += baseStats.AGI * 0.001f;
            AtkSpd += baseStats.AGI;
            MovSpd += baseStats.AGI;

            INT += baseStats.INT;
            AtkDmg += baseStats.INT;
            MagRes += baseStats.INT;
            MaxMP += baseStats.INT;
            MPRegen += baseStats.INT * 0.005f;


            AtkDmg += baseStats.AtkDmg;
            CritChc += baseStats.CritChc;

            DEF += baseStats.DEF;
            MagRes += baseStats.MagRes;

            AtkSpd += baseStats.AtkSpd;
            MovSpd += baseStats.MovSpd;

            MaxHP += baseStats.MaxHP;
            HPRegen += baseStats.HPRegen;

            MaxMP += baseStats.MaxMP;
            MPRegen += baseStats.MPRegen;
        }
    }

    private int hP;
    public int HP
    {
        get { return hP; }
        set
        {
            if (value > currentStats.MaxHP) value = currentStats.MaxHP;
            else if (value <= 0)
            {
                value = 0;
                Death();
            }
            hP = value;
        }
    }

    private int mP;
    public int MP
    {
        get { return mP; }
        set
        {
            value = Mathf.Clamp(value, 0, currentStats.MaxMP);
            mP = value;
        }
    }

    static BaseStats levelGainStats = new BaseStats(1, 1, 1);
    private int level;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            if (value <= 0) return;
            if (value > level)
            {
                for (int i = value; i > level; i--)
                    AddBonusStats(levelGainStats);
            }
            else if (value < level)
            {
                for (int i = value; i < level; i++)
                    RemoveBonusStats(levelGainStats);
            }
            level = value;
        }
    }

    private BaseStats baseStats;
    private List<BaseStats> bonusStatsList;
    private Stats currentStats;
    public void AddBonusStats(BaseStats bonusStats)
    {
        bonusStatsList.Add(bonusStats);
        currentStats.Add(bonusStats);
        HP = HP;
        MP = MP;
    }
    public void RemoveBonusStats(BaseStats bonusStats)
    {
        if (bonusStatsList.Remove(bonusStats))
        {
            currentStats.Add(-bonusStats);
            HP = HP;
            MP = MP;
        }
    }

    protected virtual void Awake()
    {
        SetBaseAttribute(out baseStats);
        currentStats = new Stats(baseStats);
    }

    protected abstract void SetBaseAttribute(out BaseStats baseStats);
    protected abstract void Death();
}
