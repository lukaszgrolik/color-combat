using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Projectile : MonoBehaviour
    {
        private AgentParty agentParty;
        private IRegistry registry;
        private IGameLayerMasksProvider layerMasksProvider;

        private Rigidbody rb;

        private Dictionary<AgentType, float> damage;

        private ProjectileDamage projectileDamage;

        private float destroyTime;

        public void Setup(
            AgentParty agentParty,
            Dictionary<AgentType, float> damage,
            IRegistry registry,
            IGameLayerMasksProvider layerMasksProvider,
            Color color,
            float speed
        ) {
            this.agentParty = agentParty;
            this.damage = damage;
            this.registry = registry;
            this.layerMasksProvider = layerMasksProvider;

            this.projectileDamage = new ProjectileDamage_Splash(
                agentParty,
                registry,
                layerMasksProvider,
                damage
            );

            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * speed;

            // var spriteRend = GetComponentInChildren<SpriteRenderer>();
            // spriteRend.color = GetGreatestDamageAgentType().color;
            var meshRend = GetComponentInChildren<MeshRenderer>();
            var matProps = new MaterialPropertyBlock();
            matProps.SetColor("_BaseColor", GetGreatestDamageAgentType().color);
            meshRend.SetPropertyBlock(matProps);

            destroyTime = Time.time + 5f;
        }

        void Delete() {
            // registry.UnregisterProjectile(this);

            Destroy(gameObject);
        }

        public void OnUpdate() {
            if (Time.time >= destroyTime) {
                Delete();
            }
        }

        public void OnTriggerEnter(Collider other) {
            var givenDamage = projectileDamage.OnContact(other);

            // Debug.Log(other);

            if (givenDamage)
            {
                Delete();
            }
        }

        AgentType GetGreatestDamageAgentType() {
            var max = -Mathf.Infinity;
            AgentType maxAgentType = null;

            foreach (var dmg in damage)
            {
                if (dmg.Value > max)
                {
                    max = dmg.Value;
                    maxAgentType = dmg.Key;
                }
            }

            return maxAgentType;
        }
    }
}
