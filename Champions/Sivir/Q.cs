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
    public class SivirQ : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            AddParticleTarget(owner, "Sivir_Q.troy", owner, 1, "L_HAND");
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 1250;
            var trueCoords = current + range;

            IMinion m = AddMinion(owner, "TestCube", "TestCube", trueCoords.X, trueCoords.Y);
            var timing = owner.GetDistanceTo(m) * 0.00078f;
            m.Die(m);

            spell.AddProjectile("SivirQMissile", owner.X, owner.Y, trueCoords.X, trueCoords.Y);
            CreateTimer(timing,()=>
            {                
                spell.AddProjectile("SivirQMissileReturn", trueCoords.X, trueCoords.Y, owner.X, owner.Y);
                owner.TeleportTo(owner.X, owner.Y);
                
                
            });
            
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            var ad = owner.Stats.AttackDamage.Total * 1.1f;
            var ap = owner.Stats.AbilityPower.Total * 0.5f;
            var damage = 15 + spell.Level * 20 + ad + ap;
            var reduc = 1 - (Math.Min(projectile.ObjectsHit.Count, 6) / 10);
            var reducedDamage = damage * reduc;
            target.TakeDamage(owner, reducedDamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);            
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
