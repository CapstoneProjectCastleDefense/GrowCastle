namespace Runtime.Elements.Entities.Archer.TestArcher
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Entities.Archer.Base;
    using UnityEngine;

    public class TestArcherPresenter : BaseArcherPresenter<TestArcherModel, TestArcherView, TestArcherPresenter>
    {
        public TestArcherPresenter(TestArcherModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager)
        {
        }
        public override    void                OnDestroyPresenter() { throw new System.NotImplementedException(); }
        protected override UniTask<GameObject> CreateView()         { throw new System.NotImplementedException(); }
        public override    void                Dispose()            { throw new System.NotImplementedException(); }
    }
}