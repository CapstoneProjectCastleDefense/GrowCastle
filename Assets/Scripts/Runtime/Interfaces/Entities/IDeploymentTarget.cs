namespace Runtime.Interfaces.Entities
{
    public interface IDeploymentTarget
    {
        public void OnDeploy(IDeployable deployable);
        public void OnRetract();
    }
}