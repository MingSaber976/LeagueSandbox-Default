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
    public class AhriSeduce : IGameScript
    {
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            AddParticleTarget(owner, "Ahri_Charm_cas.troy", owner, 1,"R_Hand");
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var current = new Vector2(owner.X, owner.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 975;
            var trueCoords = current + range;
            spell.AddProjectile("AhriSeduceMissile", owner.X, owner.Y, trueCoords.X, trueCoords.Y);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            var disarm = 6f;// TODO            
            var time = 0.75f + spell.Level * 0.25f;
            AddParticleTarget(owner, "Ahri_Charm_tar.troy", target);
            var p1 = AddParticleTarget(owner, "Ahri_Charm_buf.troy", target);
            var ap = owner.Stats.AbilityPower.Total * 0.35f;
            var damage = 30 + spell.Level * 30f + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            ((ObjAiBase)target).AddBuffGameScript("AhriE", "AhriE", spell, time, true);
            CreateTimer(time, () =>
            {
                RemoveParticle(p1);
            });
            projectile.SetToRemove();
            for (float i = 0.0f; i <= disarm; i += 0.1f)
            {
                ((ObjAiBase)target).SetTargetUnit(owner);
                if (((ObjAiBase)target).IsAttacking && owner.GetDistanceTo(target) <= target.Stats.Range.Total)
                {
                    ((ObjAiBase)target).StopMovement();
                }
            }
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
