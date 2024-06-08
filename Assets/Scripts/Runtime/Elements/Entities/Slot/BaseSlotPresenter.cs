using GameFoundation.Scripts.Utilities.ObjectPool;
using Runtime.Elements.Base;
using Runtime.Interfaces.Entities;
using System;

namespace Assets.Scripts.Runtime.Elements.Entities.Slot
{
    public abstract class BaseSlotPresenter : BaseElementPresenter<BaseSlotModel, BaseSlotView, BaseSlotPresenter>, ISlotPresenter{
        public BaseSlotPresenter(BaseSlotModel model, ObjectPoolManager objectPoolManager)
            : base(model, objectPoolManager) {
        }

        public void OnDeploy(IDeployable deployable) {
            throw new NotImplementedException();
        }

        public void OnRetract() {
            throw new NotImplementedException();
        }
    }
}
