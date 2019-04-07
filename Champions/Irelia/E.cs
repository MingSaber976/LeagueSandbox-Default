using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class IreliaEquilibriumStrike : IGameScript
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
            AddParticleTarget(owner, "irelia_equilibriumStrike_cas.troy", owner);
            var myself = owner.Stats.CurrentHealth / owner.Stats.HealthPoints.Total;
            var targets = target.Stats.CurrentHealth / target.Stats.HealthPoints.Total;
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 30 + spell.Level * 50f + ap * 0.5f;
            var time = 0.75f + spell.Level * 0.25f;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(owner, "irelia_equilibriumStrike_tar_01.troy", target);
            AddParticleTarget(owner, "irelia_equilibriumStrike_tar_02.troy", target);
            if (targets > myself)
            {
                ((ObjAiBase)target).AddBuffGameScript("Stun", "Stun", spell, time, true);
                AddBuffHudVisual("Stun", time, 1, BuffType.STUN, (ObjAiBase)target, time);
                var stun = AddParticleTarget(owner, "Global_Stun.troy", target,1,"head");
                CreateTimer(time, () =>
                {
                    RemoveParticle(stun);
                });
            }
            else
            {
                ((ObjAiBase)target).AddBuffGameScript("IreliaESlow", "IreliaESlow", spell, time, true);
                var slow = AddParticleTarget(owner, "Global_Slow.troy", target);
                CreateTimer(time, () =>
                {
                    RemoveParticle(slow);
                });
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
