using ThunderRoad;
using UnityEngine;
// ReSharper disable ParameterHidesMember

namespace DarkChains
{
    public class SpellDarkChains : SpellCastCharge
    {
        public float ragdollSpeedBoost = 1000.0f;
        private float _defaultRagdollSpeed;
        
        public override void Load(SpellCaster spellCaster, Level level)
        {
            base.Load(spellCaster, level);

            spellCaster.telekinesis.grabRagdoll = true;
            _defaultRagdollSpeed = spellCaster.telekinesis.followRagdollSpeedCurve.keys[1].value;

            //minimum is 1.0
            ModifyRagdollSpeed(Mathf.Max(1.0f, ragdollSpeedBoost));
        }

        private void ModifyRagdollSpeed(float value)
        {
            var ragdollSpeedCurve = spellCaster.telekinesis.followRagdollSpeedCurve.keys[1];
            ragdollSpeedCurve.value = value;
            spellCaster.telekinesis.followRagdollSpeedCurve.RemoveKey(1);
            spellCaster.telekinesis.followRagdollSpeedCurve.AddKey(ragdollSpeedCurve);
        }
        
        private void ResetRagdollSpeed()
        {
            var ragdollSpeedCurve = spellCaster.telekinesis.followRagdollSpeedCurve.keys[1];
            ragdollSpeedCurve.value = _defaultRagdollSpeed;
            spellCaster.telekinesis.followRagdollSpeedCurve.RemoveKey(1);
            spellCaster.telekinesis.followRagdollSpeedCurve.AddKey(ragdollSpeedCurve);
        }
        
        
        public override void Unload()
        {
            spellCaster.telekinesis.grabRagdoll = false;

            ResetRagdollSpeed();

            base.Unload();
        }
    }
}