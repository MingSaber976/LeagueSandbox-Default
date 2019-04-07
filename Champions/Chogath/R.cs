using System;
using System.Numerics;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class Feast : IGameScript
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
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 1000f + ap * 0.7f;
            if (target is IChampion)
            {
                damage = 125f + spell.Level * 175f + ap * 0.7f;
            }
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            if (target.IsDead)
            {
                owner.Stats.Size.FlatBonus += 1f;
                owner.Stats.HealthPoints.FlatBonus += 100f;
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
