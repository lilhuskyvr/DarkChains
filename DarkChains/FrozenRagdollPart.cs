using ThunderRoad;
using UnityEngine;

namespace DarkChains
{
    public class FrozenRagdollPart : MonoBehaviour
    {
        private RagdollPart _ragdollPart;
        private EffectData _ragdollGripEffectData;
        private EffectInstance _ragdollGripEffect;

        private void Start()
        {
            _ragdollPart = GetComponent<RagdollPart>();
            _ragdollPart.rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        public void Init(HandleRagdoll handleRagdoll)
        {
            _ragdollGripEffectData = Catalog.GetData<EffectData>("SpellDarkChainsGrab");
            _ragdollGripEffect = _ragdollGripEffectData.Spawn(handleRagdoll.transform);
            _ragdollGripEffect.Play();
        }

        private void OnDestroy()
        {
            _ragdollPart.rb.constraints = RigidbodyConstraints.None;
            _ragdollGripEffect.End();
        }
    }
}