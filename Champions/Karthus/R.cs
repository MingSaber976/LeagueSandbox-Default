using System.Linq;
using GameServerCore;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class KarthusFallenOne : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            spell.SpellAnimation("Spell4_Loop", owner);
            AddParticleTarget(owner, "Karthus_Base_R_Cas.troy", owner);
            AddParticleTarget(owner, "Karthus_Base_R_Cas_Hand_Glow.troy", owner, 1, "R_Hand");
            foreach (var enemyTarget in GetChampionsInRange(owner, 20000, true)
                .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
                AddParticleTarget(owner, "Karthus_Base_R_Target.troy", enemyTarget);
            }
            
        }   

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            spell.SpellAnimation("Spell4_Winddown", owner);
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 100 + spell.Level * 150 + ap * 0.6f;
            foreach (var enemyTarget in GetChampionsInRange(owner, 20000, true)
                .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
                AddParticleTarget(owner, "Karthus_Base_R_Explosion.troy", enemyTarget, 3);
                AddParticleTarget(owner, "ZiggsR_Nova_pool.troy", enemyTarget, 1);
                AddParticleTarget(owner, "ZiggsR_Nova.troy", enemyTarget, 1);
                enemyTarget.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL,
                    false);
            }
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
