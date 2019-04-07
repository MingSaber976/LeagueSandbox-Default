using System;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Missiles;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class Consume : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {            
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {            
            var damage = 250 + spell.Level * 150;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(owner, "yeti_Consume_tar.troy", target, 1);
            PerformHeal(owner, spell, target);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {            
                        
        }

        private void PerformHeal(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AbilityPower.Total * 0.75f;            
            float healthGain = 25 + spell.Level * 45 + ap;
            var newHealth = owner.Stats.CurrentHealth + healthGain;
            owner.Stats.CurrentHealth = Math.Min(newHealth, owner.Stats.HealthPoints.Total);
            AddParticleTarget(owner, "global_ss_heal_02.troy", target);
            AddParticleTarget(owner, "global_ss_heal_speedboost.troy", target);
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
