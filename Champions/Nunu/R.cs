using System.Linq;
using GameServerCore;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
//WIP
namespace Spells
{
    public class AbsoluteZero : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            foreach (var enemyTarget in GetChampionsInRange(owner, 650, true)
                .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
                AddParticleTarget(owner, "Item_TrueIce_Freeze_Slow.troy", enemyTarget);
            }
            
                
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 375 + spell.Level * 250 + ap * 2.5f;
            foreach (var enemyTarget in GetChampionsInRange(owner, 650, true)
                .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
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
