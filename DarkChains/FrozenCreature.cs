using System.Linq;
using ThunderRoad;
using UnityEngine;

namespace DarkChains
{
    public class FrozenCreature : MonoBehaviour
    {
        private Creature _creature;
        private float _defaultLocomotionSpeed;
        private float _defaultAnimatorSpeed;
        private string _defaultCreatureBrainId;

        private void Awake()
        {
            _creature = GetComponent<Creature>();

            _defaultCreatureBrainId = _creature.brain.instance.id;
            _defaultLocomotionSpeed = _creature.locomotion.speed;
            // _defaultAnimatorSpeed = _creature.animator.speed;
            _creature.brain.Stop();
            _creature.brain.Load("FrozenCreature");
            // _creature.brain.StopNavigation();
            // _creature.brain.StopTurn();
            _creature.locomotion.MoveStop();
            _creature.locomotion.speed = 0;
            // _creature.animator.speed = 0;
        }

        private void Update()
        {
            var count = _creature.ragdoll.parts.Count(part => part.GetComponent<FrozenRagdollPart>() != null);

            if (count == 0)
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            _creature.brain.Load(_defaultCreatureBrainId);
            _creature.locomotion.speed = _defaultLocomotionSpeed;
            // _creature.animator.speed = _defaultAnimatorSpeed;
        }
    }
}