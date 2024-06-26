namespace Runtime.Elements.Entities.Projectile
{
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class ProjectileModel : IElementModel
    {
        public string      Id              { get; set; }
        public string      AddressableName { get; set; }
        public GameObject  Prefab          { get; set; }
        public Vector3     StartPoint      { get; set; }
        public Vector3     EndPoint        { get; set; }
        public float       Damage          { get; set; }
        public ITargetable Target          { get; set; }
    }
}