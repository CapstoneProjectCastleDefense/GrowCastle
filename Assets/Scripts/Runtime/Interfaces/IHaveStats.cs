namespace Runtime.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Runtime.Enums;

    public interface IHaveStats
    {
        Dictionary<StatEnum, (Type, object)> Stats { get; set; }
    }
}