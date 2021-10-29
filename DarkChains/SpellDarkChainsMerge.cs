using System;
using ThunderRoad;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DarkChains
{
    public class SpellDarkChainsMerge : SpellMergeData
    {
        public override void Merge(bool active)
        {
            base.Merge(active);

            if (active || currentCharge < 1.0f)
                return;

            var distance =
                Vector3.Distance(mana.casterLeft.magicSource.position, mana.casterRight.magicSource.position);

            if (distance >= handExitDistance)
            {
                foreach (var creature in Creature.allActive)
                {
                    foreach (var part in creature.ragdoll.parts)
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
            }

            currentCharge = 0;
        }
    }
}