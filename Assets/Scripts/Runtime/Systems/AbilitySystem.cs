namespace Runtime.Systems
{
    using System.Collections.Generic;
    using Runtime.Interfaces.Abilities;

    public class AbilitySystem : IGameSystem
    {
        #region Inject

        private IReadOnlyList<IAbility> abilities;
        
        public AbilitySystem(IReadOnlyList<IAbility> abilities)
        {
            this.abilities = abilities;
        }

        #endregion
        
        #region IGameSystem

        public void Initialize()
        {
            
        }
        public void Tick()
        {
            
        }
        public void Dispose()
        {
            
        }

        #endregion
    }
}