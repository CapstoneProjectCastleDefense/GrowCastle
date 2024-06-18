namespace Runtime.Elements.Entities.Projectile
{
    using Runtime.Elements.Base;
    using UnityEngine;

    public class ProjectileModel : IElementModel
    {
        public string     Id              { get; set; }
        public string     AddressableName { get; set; }
        public GameObject Prefab          { get; set; }
        public Vector3    StartPoint      { get; set; }
        public Vector3    EndPoint        { get; set; }
    }
}