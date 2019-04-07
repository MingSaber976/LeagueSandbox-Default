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
    public class FioraQ : IGameScript
    {
        public static Particle p1;
        public static IBuff buff01;
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
            owner.RemoveSpell(0);
            owner.SetSpell("FioraQLunge", 0, true);
            var temp01 = FioraQLunge.temp01;
            var distan = owner.GetDistanceTo(target) * 0.0005f;
            var ad = owner.Stats.AttackDamage.Total;
            var damage = 15 + spell.Level * 25f + ad * 0.6f;
            DashToUnit(owner, target, 1800, false, "Spell1", 0, 0, 0, 0);
            owner.Stats.MoveSpeed.FlatBonus += 1800;
            CancelDash(owner);
            p1 = AddParticleTarget(owner, "FioraQLunge_buf.troy", owner, 1, "R_Hand");
            var p2 = AddParticleTarget(owner, "FioraQLunge_dashtrail.troy", owner);
            CreateTimer(distan, () =>
            {
                spell.SpellAnimation("Attack1", owner);
                owner.Stats.MoveSpeed.FlatBonus -= 1800;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, "FioraQLunge_tar.troy", target);
                RemoveParticle(p2);
            });

            CreateTimer(0.05f, () =>
            {
                owner.Spells[0].LowerCooldown(owner.Spells[0].GetCooldown());
                buff01 = AddBuffHudVisual("FioraQCD", 4f, 1, BuffType.COMBAT_ENCHANCER, owner);
            });
            CreateTimer(4.05f, () =>
            {
                if (buff01 != null)
                {
                    RemoveBuffHudVisual(buff01);
                    buff01 = null;
                }
                if (owner.Spells[0].SpellName == "FioraQLunge")
                {
                    owner.SetSpell("FioraQ", 0, true);
                    owner.Spells[0].LowerCooldown(-(owner.Spells[0].GetCooldown()) + 4f);
                }
            });
            CreateTimer(distan + 4.05f, () =>
            {
                if (p1 != null)
                {
                    RemoveParticle(p1);
                    p1 = null;
                }
            });
            
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            
        }

        public void OnUpdate(double diff)
        {            
        }
    }
}
