using System;
using System.Collections;
using System.Reflection;
using HarmonyLib;
using ThunderRoad;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DarkChains
{
    public class DarkChainsLevelModule : LevelModule
    {
        private Harmony _harmony;

        public override IEnumerator OnLoadCoroutine(Level level)
        {
            try
            {
                _harmony = new Harmony("DarkChains");
                _harmony.PatchAll(Assembly.GetExecutingAssembly());

                Debug.Log("Dark Chains Loaded");
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            EventManager.onCreatureSpawn += EventManagerOnonCreatureSpawn;

            return base.OnLoadCoroutine(level);
        }

        private void EventManagerOnonCreatureSpawn(Creature creature)
        {
            var ragdoll = creature.ragdoll;
            foreach (var part in ragdoll.parts)
            {
                try
                {
                    Object.Destroy(part.gameObject.GetComponent<FrozenRagdollPart>());
                }
                catch (Exception exception)
                {
                    Debug.Log(exception.Message);
                }
            }
        }

        [HarmonyPatch(typeof(HandleRagdoll))]
        [HarmonyPatch("OnGrab")]
        private static class RagdollHandleOnGrabPatch
        {
            [HarmonyPostfix]
            private static void Postfix(HandleRagdoll __instance)
            {
                var ragdollPart = __instance.ragdollPart;
                try
                {
                    Object.Destroy(ragdollPart.gameObject.GetComponent<FrozenRagdollPart>());
                }
                catch (Exception exception)
                {
                    Debug.Log(exception.Message);
                }

                RagdollPart otherPart = null;

                if (ragdollPart.type == RagdollPart.Type.LeftArm)
                    otherPart = ragdollPart.ragdoll.GetPart(RagdollPart.Type.LeftHand);
                if (ragdollPart.type == RagdollPart.Type.RightArm)
                    otherPart = ragdollPart.ragdoll.GetPart(RagdollPart.Type.RightHand);
                if (ragdollPart.type == RagdollPart.Type.LeftLeg)
                    otherPart = ragdollPart.ragdoll.GetPart(RagdollPart.Type.LeftFoot);
                if (ragdollPart.type == RagdollPart.Type.RightLeg)
                    otherPart = ragdollPart.ragdoll.GetPart(RagdollPart.Type.RightFoot);

                if (otherPart != null)
                {
                    try
                    {
                        Object.Destroy(otherPart.gameObject.GetComponent<FrozenRagdollPart>());
                    }
                    catch (Exception exception)
                    {
                        Debug.Log(exception.Message);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(HandleRagdoll))]
        [HarmonyPatch("OnTelekinesisGrab")]
        private static class RagdollHandleOnTelekinesisGrabPatch
        {
            [HarmonyPostfix]
            private static void Postfix(SpellTelekinesis spellTelekinesis, HandleRagdoll __instance)
            {
                try
                {
                    var ragdollPart = __instance.ragdollPart;
                    if (spellTelekinesis.spellCaster.spellInstance.id == "DarkChains")
                    {
                        try
                        {
                            if (ragdollPart.ragdoll.creature.gameObject.GetComponent<FrozenCreature>() != null)
                                ragdollPart.characterJoint.breakForce = 4000;
                        }
                        catch (Exception exception)
                        {
                            Debug.Log(exception.Message);
                        }
                    }

                    try
                    {
                        Object.Destroy(ragdollPart.gameObject.GetComponent<FrozenRagdollPart>());
                    }
                    catch (Exception exception)
                    {
                        Debug.Log(exception.Message);
                    }
                }
                catch (Exception exception)
                {
                    __instance.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    Debug.Log(exception.Message);
                }
            }
        }

        [HarmonyPatch(typeof(HandleRagdoll))]
        [HarmonyPatch("OnTelekinesisRelease")]
        private static class RagdollHandleOnTelekinesisReleasePatch
        {
            [HarmonyPostfix]
            private static void Postfix(SpellTelekinesis spellTelekinesis, bool tryThrow, ref bool throwing,
                HandleRagdoll __instance)
            {
                try
                {
                    if (!throwing && spellTelekinesis.spellCaster.spellInstance.id == "DarkChains")
                    {
                        var ragdollPart = __instance.ragdollPart;
                        ragdollPart.ResetCharJointBreakForce();
                        if (ragdollPart.gameObject.GetComponent<FrozenRagdollPart>() == null)
                        {
                            var frozenRagdollPart = ragdollPart.gameObject.AddComponent<FrozenRagdollPart>();

                            frozenRagdollPart.Init(__instance);
                        }

                        try
                        {
                            var creature = ragdollPart.ragdoll.creature;
                            if (creature.gameObject.GetComponent<FrozenCreature>() == null)
                            {
                                creature.gameObject.AddComponent<FrozenCreature>();
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.Log(exception.Message);
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (!throwing && spellTelekinesis.spellCaster.spellInstance.id == "DarkChains")
                    {
                        __instance.GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    }

                    Debug.Log(exception.Message);
                }
            }
        }
    }
}