namespace Runtime.StaticValues
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.PassiveSkills;

    public static class PassiveSkillName
    {
        public static Dictionary<string, Type> PassiveSkillPresenters = new()
        {
            { GodApperancePassiveSkill, typeof(GodApperancePassiveSkill) },
            { GodLightPassiveSkill, typeof(GodLightPassiveSkill) },
            { ControlGravityPassiveSkill, typeof(ControlGravityPassiveSkill) },
            { IntimidationPassiveSkill, typeof(IntimidationPassiveSkill) },
        };

        public static string GodApperancePassiveSkill   => "GodApperancePassiveSkill";
        public static string GodLightPassiveSkill       => "GodLightPassiveSkill";
        public static string ControlGravityPassiveSkill => "ControlGravityPassiveSkill";
        public static string IntimidationPassiveSkill   => "IntimidationPassiveSkill";
    }
}