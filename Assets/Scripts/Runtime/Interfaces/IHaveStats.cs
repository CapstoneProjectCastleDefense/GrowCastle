namespace Runtime.Interfaces
{
    using System.Collections.Generic;
    using Runtime.Enums;

    public interface IHaveStats
    {
        Dictionary<StatEnum, object> Stats { get; set; }
    }
}