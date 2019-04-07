using System;
using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class AhriOrbofDeception : IGameScript
    {
        public int maff = 0;
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            AddParticleTarget(owner, "Ahri_Orb_cas.troy", owner, 1,"R_Hand");
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 880;
            var trueCoords = current + range;

            IMinion m = AddMinion(owner, "TestCube", "TestCube", trueCoords.X, trueCoords.Y);
            var timing = owner.GetDistanceTo(m) * 0.0009f;
            m.Die(m);

            spell.AddProjectile("AhriOrbMissile", owner.X, owner.Y, trueCoords.X, trueCoords.Y);
            CreateTimer(timing,()=>
            {
                maff++;
                spell.AddProjectile("AhriOrbReturn", trueCoords.X, trueCoords.Y, owner.X, owner.Y);
                owner.TeleportTo(owner.X, owner.Y);
            });

            CreateTimer(timing + 3f, () =>
            {
                maff = 0;
            });
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            AddParticleTarget(owner, "Ahri_Orb_tar.troy", target);
            var ap = owner.Stats.AbilityPower.Total * 0.35f;
            var damage = 15 + spell.Level * 25 + ap;
            if (maff == 0)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
            if (maff == 1)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
