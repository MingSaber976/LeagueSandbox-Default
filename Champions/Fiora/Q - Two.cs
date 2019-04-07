using System;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Missiles;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class FioraQLunge : IGameScript
    {
        public static byte temp01 = 0;
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
            var partic = FioraQ.p1;
            var distan = owner.GetDistanceTo(target) * 0.0005f;
            var ad = owner.Stats.AttackDamage.Total;
            var damage = 15 + spell.Level * 25f + ad * 0.6f;
            DashToUnit(owner, target, 1800, false, "Spell1", 0, 0, 0, 0);
            owner.Stats.CurrentMana += owner.Spells[0].SpellData.ManaCost[owner.Spells[0].Level];
            owner.Stats.MoveSpeed.FlatBonus += 1800;
            CancelDash(owner);
            CreateTimer(distan, () =>
            {
                spell.SpellAnimation("Attack1", owner);
                owner.Stats.MoveSpeed.FlatBonus -= 1800;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, "FioraQLunge_tar.troy", target);
            });
            if (partic != null)
            {
                RemoveParticle(partic);
                partic = null;
            }
            RemoveBuffHudVisual(FioraQ.buff01);
            FioraQ.buff01 = null;
            owner.RemoveSpell(0);
            owner.SetSpell("FioraQ", 0, true);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            
        }

        public void OnUpdate(double diff)
        {            
        }
    }
}
