namespace Runtime.Interfaces.Entities
{
    public interface IDeployable
    {
        void Deploy(IDeploymentTarget target);
        void Retract();
    }
}