public enum Alliance
{
    Neutral,
    Player,
    Enemy
}

public class BaseStats
{
    public readonly int
        STR, AGI, INT,
        AtkDmg, DEF, MagRes,
        AtkSpd, MovSpd,
        MaxHP, MaxMP;

    public readonly float CritChc, HPRegen, MPRegen;

    public BaseStats(int STR, int AGI, int INT)
    {
        this.STR = STR;
        this.AGI = AGI;
        this.INT = INT;
    }

    public BaseStats(
        int STR, int AGI, int INT,
        int AtkDmg, float CritChc,
        int DEF, int MagRes,
        int AtkSpd, int MovSpd,
        int MaxHP, float HPRegen,
        int MaxMP, float MPRegen) : this(STR, AGI, INT)
    {
        this.AtkDmg = AtkDmg;
        this.CritChc = CritChc;

        this.DEF = DEF;
        this.MagRes = MagRes;

        this.AtkSpd = AtkSpd;
        this.MovSpd = MovSpd;

        this.MaxHP = MaxHP;
        this.MPRegen = MPRegen;

        this.MaxMP = MaxMP;
        this.MPRegen = MPRegen;
    }

    //public static BaseStats operator +(BaseStats stats1, BaseStats stats2)
    //{
    //    return new BaseStats(
    //        stats1.STR + stats2.STR, stats1.AGI + stats2.AGI, stats1.INT + stats2.INT,
    //        stats1.AtkDmg + stats2.AtkDmg, stats1.CritChc + stats2.CritChc,
    //        stats1.DEF + stats2.DEF, stats1.MagRes + stats2.MagRes,
    //        stats1.AtkSpd + stats2.AtkSpd, stats1.MovSpd + stats2.MovSpd,
    //        stats1.MaxHP + stats2.MaxHP, stats1.HPRegen + stats2.HPRegen,
    //        stats1.MaxMP + stats2.MaxMP, stats1.MPRegen + stats2.MPRegen);
    //}

    public static BaseStats operator -(BaseStats stats)
    {
        return new BaseStats(
           -stats.STR, -stats.AGI, -stats.INT,
           -stats.AtkDmg, -stats.CritChc,
           -stats.DEF, -stats.MagRes,
           -stats.AtkSpd, -stats.MovSpd,
           -stats.MaxHP, -stats.HPRegen,
           -stats.MaxMP, -stats.MPRegen);
    }
}
