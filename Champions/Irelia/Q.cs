using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class IreliaGatotsu : IGameScript
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
            var pC1 = AddParticleTarget(owner, "irelia_gotasu_cas.troy", owner);
            var pC2 = AddParticleTarget(owner, "irelia_gotasu_cast_01.troy", owner);
            var pC3 = AddParticleTarget(owner, "irelia_gotasu_cast_02.troy", owner);
            var time = owner.GetDistanceTo(target) * 0.000385f;
            DashToUnit(owner, target, 1600, false, "Spell1", 0, 0, 0, 0);
            owner.Stats.MoveSpeed.FlatBonus += 1600;
            var pD1 = AddParticleTarget(owner, "irelia_gotasu_dash_01.troy", owner);
            var pD2 = AddParticleTarget(owner, "irelia_gotasu_dash_02.troy", owner);
            CreateTimer(time, () => 
            {
                owner.Stats.MoveSpeed.FlatBonus -= 1600;
                CancelDash(owner);
                var ad = owner.Stats.AttackDamage.Total;
                var damage = -10 + spell.Level * 30 + ad;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, "irelia_gotasu_tar.troy", target);
                if (target.IsDead)
                {
                    spell.LowerCooldown(spell.GetCooldown());
                    owner.Stats.CurrentMana += 35;
                    AddParticleTarget(owner, "irelia_gotasu_mana_refresh.troy", owner);
                }
            });
            
            RemoveParticle(pD1);
            RemoveParticle(pD2);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
