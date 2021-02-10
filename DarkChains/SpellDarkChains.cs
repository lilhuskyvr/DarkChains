using System;
using ThunderRoad;
using UnityEngine;

namespace DarkChains
{
    public class SpellDarkChains : SpellCastCharge
    {
        public float ragdollSpeedBoost = 1000.0f;

        // ReSharper disable once ParameterHidesMember
        public override void Load(SpellCaster spellCaster)
        {
            base.Load(spellCaster);

            spellCaster.telekinesis.grabRagdoll = true;

            //minimum is 1.0
            ModifyRagdollSpeed(Mathf.Max(1.0f, ragdollSpeedBoost));
        }

        private void ModifyRagdollSpeed(float value)
        {
            var ragdollSpeedCurve = spellCaster.telekinesis.followRagdollSpeedCurve.keys[1];
            ragdollSpeedCurve.value *= value;
            spellCaster.telekinesis.followRagdollSpeedCurve.RemoveKey(1);
            spellCaster.telekinesis.followRagdollSpeedCurve.AddKey(ragdollSpeedCurve);
        }

        public override void Unload()
        {
            spellCaster.telekinesis.grabRagdoll = false;

            ModifyRagdollSpeed(1 / 1000f);

            base.Unload();
        }
    }
}