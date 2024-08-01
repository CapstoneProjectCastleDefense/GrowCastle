namespace Runtime.StaticValues
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.PassiveSkills;
    using Runtime.Elements.PassiveSkills.Hassan;
    using Runtime.Elements.PassiveSkills.Shadow;

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
            { LightFootPassiveSkill, typeof(LightFootPassiveSkill) },
            { StealthPassiveSkill, typeof(StealthPassiveSkill) },
        };

        public static string GodApperancePassiveSkill   => "GodApperancePassiveSkill";
        public static string GodLightPassiveSkill       => "GodLightPassiveSkill";
        public static string ControlGravityPassiveSkill => "ControlGravityPassiveSkill";
        public static string IntimidationPassiveSkill   => "IntimidationPassiveSkill";
        public static string TheFearPassiveSkill        => "TheFearPassiveSkill";
        public static string DeadlyArrowPassiveSkill    => "DeadlyArrowPassiveSkill";
        public static string LightFootPassiveSkill      => "LightFootPassiveSkill";
        public static string StealthPassiveSkill        => "StealthPassiveSkill";
    }
}