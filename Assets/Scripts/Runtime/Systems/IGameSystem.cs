namespace Runtime.Systems
{
    using System;
    using Zenject;

    public interface IGameSystem : IInitializable, ITickable, IDisposable
    {
        
    }
}