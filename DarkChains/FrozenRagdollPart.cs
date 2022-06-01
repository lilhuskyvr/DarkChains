using System.Collections;
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

        public void Quarter()
        {
            StartCoroutine(QuarterCoroutine());
        }

        private IEnumerator QuarterCoroutine()
        {
            var speakModule = _ragdollPart.ragdoll.creature.brain.instance.GetModule<BrainModuleSpeak>();
            _ragdollPart.sliceAllowed = true;
            if (Random.Range(0, 2) == 0)
                _ragdollPart.characterJoint.breakForce = 30000;

            if (_ragdollPart.parentPart != null)
            {
                _ragdollPart.sliceAllowed = true;
                if (Random.Range(0, 2) == 0)
                    _ragdollPart.characterJoint.breakForce = 30000;
            }

            var startTime = Time.time;
            while (Time.time - startTime <= 10)
            {
                var direction = (_ragdollPart.rb.transform.position -
                                 _ragdollPart.ragdoll.GetPart(RagdollPart.Type.Torso).transform.position).normalized;
                _ragdollPart.rb.transform.position += 0.05f * Time.deltaTime * direction;

                if (speakModule != null)
                {
                    if (!speakModule.isSpeaking && _ragdollPart.ragdoll.creature.state != Creature.State.Dead)
                        speakModule.Play(BrainModuleSpeak.hashHit, false);
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private void OnDestroy()
        {
            _ragdollPart.rb.isKinematic = false;
            _ragdollGripEffect.End();
            StopAllCoroutines();
        }
    }
}