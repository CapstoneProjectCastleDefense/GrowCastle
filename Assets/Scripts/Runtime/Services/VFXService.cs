namespace Runtime.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using UnityEngine;
    using Zenject;

    public class VFXService : MonoBehaviour
    {
        private Dictionary<string, HashSet<ParticleSystem>> FreeParticleSystems { get; } = new();

        #region Inject

        private IGameAssets GameAssets { get; set; }

        [Inject]
        private void Inject(IGameAssets gameAssets)
        {
            this.GameAssets = gameAssets;
        }

        #endregion

        public async Task SpawnVFX(string id, Vector3 position, Quaternion? rotation = null, Vector3? scale = null)
        {
            rotation ??= Quaternion.identity;
            scale    ??= Vector3.one;

            if (!this.FreeParticleSystems.TryGetValue(id, out var particleSystems))
            {
                particleSystems = new();
                this.FreeParticleSystems.Add(id, particleSystems);
            }

            var particle = particleSystems.FirstOrDefault();

            if (particle is null)
            {
                var prefab = this.GameAssets.LoadAssetAsync<GameObject>(id).WaitForCompletion();

                if (!prefab)
                {
                    return;
                }

                particle = Instantiate(prefab, this.transform).GetComponent<ParticleSystem>();
                particle.gameObject.SetActive(false);

                if (!particle)
                {
                    return;
                }

                particleSystems.Add(particle);
            }

            lock (particleSystems)
            {
                particleSystems.Remove(particle);
            }

            var transform1 = particle.transform;
            transform1.position   = position;
            transform1.rotation   = rotation.Value;
            transform1.localScale = scale.Value;

            particle.gameObject.SetActive(true);
            particle.Stop(true);
            particle.Play(true);

            while (particle.isPlaying)
            {
                await Task.Yield();
            }

            particle.gameObject.SetActive(false);

            lock (particleSystems)
            {
                particleSystems.Add(particle);
            }
        }
    }
}