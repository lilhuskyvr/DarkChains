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
            _ragdollPart.rb.isKinematic = true;
        }

        public void Init(HandleRagdoll handleRagdoll)
        {
            _ragdollGripEffectData = Catalog.GetData<EffectData>("SpellGravityGrab");
            //remove audio
            _ragdollGripEffect = _ragdollGripEffectData.Spawn(handleRagdoll.transform);
            foreach (var effect in _ragdollGripEffect.effects)
            {
                if (effect is EffectAudio)
                {
                    //mute
                    (effect as EffectAudio).audioSource.volume = 0;
                }
            }
            _ragdollGripEffect.Play();
        }

        private void OnDestroy()
        {
            _ragdollPart.rb.isKinematic = false;
            _ragdollGripEffect.End();
        }
    }
}