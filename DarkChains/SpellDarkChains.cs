using ThunderRoad;

 namespace DarkChains
{
    public class SpellDarkChains : SpellCastCharge
    {

        // ReSharper disable once ParameterHidesMember
        public override void Load(SpellCaster spellCaster)
        {
            base.Load(spellCaster);

            spellCaster.telekinesis.grabRagdoll = true;

            ModifyRagdollSpeed(1000);
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