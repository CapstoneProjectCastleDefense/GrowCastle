namespace Runtime.StaticValues
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.PassiveSkills;
    using Runtime.Elements.PassiveSkills.ARise;
    using Runtime.Elements.PassiveSkills.GreenLeaf;
    using Runtime.Elements.PassiveSkills.Hassan;
    using Runtime.Elements.PassiveSkills.Judge;
    using Runtime.Elements.PassiveSkills.Knight;
    using Runtime.Elements.PassiveSkills.MoonShadow;
    using Runtime.Elements.PassiveSkills.Shadow;
    using Runtime.Elements.PassiveSkills.Storm;

    public static class PassiveSkillName
    {
        public static Dictionary<string, Type> PassiveSkillPresenters = new()
        {
            { GodApperancePassiveSkill, typeof(GodApperancePassiveSkill) },
            { GodLightPassiveSkill, typeof(GodLightPassiveSkill) },
            { ControlGravityPassiveSkill, typeof(ControlGravityPassiveSkill) },
            { IntimidationPassiveSkill, typeof(IntimidationPassiveSkill) },
            { TheFearPassiveSkill, typeof(TheFearPassiveSkill) },
            { DeadlyArrowPassiveSkill, typeof(DeadlyArrowPassiveSkill) },
            { DarknessArise, typeof(DarknessArise) },
            { ShadowLord, typeof(ShadowLord) },
            { LightFootPassiveSkill, typeof(LightFootPassiveSkill) },
            { StealthPassiveSkill, typeof(StealthPassiveSkill) },
            { BloodScythe, typeof(BloodScythe) },
            { PiercingScythe, typeof(PiercingScythe) },
            { ForesightPassiveSkill, typeof(ForesightPassiveSkill) },
            { ObscurePassiveSkill, typeof(ObscurePassiveSkill) },
            { FightForeverPassiveSkill, typeof(FightForeverPassiveSkill) },
            { GreatAdmiralPassiveSkill, typeof(GreatAdmiralPassiveSkill) },
            { ElvenMarkPassiveSkill, typeof(ElvenMarkPassiveSkill) },
            { HunterZonePassiveSkill, typeof(HunterZonePassiveSkill) },
        };

        public static string GodApperancePassiveSkill   => "GodApperancePassiveSkill";
        public static string GodLightPassiveSkill       => "GodLightPassiveSkill";
        public static string ControlGravityPassiveSkill => "ControlGravityPassiveSkill";
        public static string IntimidationPassiveSkill   => "IntimidationPassiveSkill";
        public static string TheFearPassiveSkill        => "TheFearPassiveSkill";
        public static string DeadlyArrowPassiveSkill    => "DeadlyArrowPassiveSkill";
        public static string LightFootPassiveSkill      => "LightFootPassiveSkill";
        public static string StealthPassiveSkill        => "StealthPassiveSkill";
        public static string DarknessArise              => "DarknessArise";
        public static string ShadowLord                 => "ShadowLord";
        public static string BloodScythe                => "BloodScythe";
        public static string PiercingScythe             => "PiercingScythe";
        public static string ForesightPassiveSkill      => "ForesightPassiveSkill";
        public static string ObscurePassiveSkill        => "ObscurePassiveSkill";
        public static string FightForeverPassiveSkill   => "FightForeverPassiveSkill";
        public static string GreatAdmiralPassiveSkill   => "GreatAdmiralPassiveSkill";
        public static string ElvenMarkPassiveSkill      => "ElvenMarkPassiveSkill";
        public static string HunterZonePassiveSkill     => "HunterZonePassiveSkill";

    }
}