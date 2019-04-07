using System;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Missiles;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class PowerFist : IGameScript
    {
        public static Particle mark;
        public static float marktime = 5.0f;
        public static float marktimeactive;
        public static float updateinterval = 0.1f;
        public static byte stacks = 0;
        static IBuff buff01;
        static IChampion owner;
        static IAttackableUnit target;
        //WIP, wait for onhit
        public void OnActivate(IChampion owner)
        {
            PowerFist.owner = owner;
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnProc);
            ApiEventManager.OnUpdate.AddListener(this, OnUpdate);
        }

        public static void OnProc(IAttackableUnit target, bool isCrit)
        {
            if (mark == null)
            {
                return;
            }

            var ad = owner.Stats.AttackDamage.Total;
            //TODO 1 : add MissingHealth to increase damage
            //TODO 2 : add Maximum Damage
            var damage = ad *2;

            RemoveParticle(mark);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            if (!(target is IObjBuilding) || !(target is ILaneTurret) || !(target is IBaseTurret))
            {
                DashToLocation((ObjAiBase)target, target.X+10, target.Y+10, 20, true, null, 10, 0, 0, 1f);
                CreateTimer(1f, () =>
                {
                    CancelDash((ObjAiBase)target);
                });
            }

                AddParticleTarget(owner, "Powerfist_tar.troy", target, 1);
            owner.AutoAttackTarget = null;
            mark = null;
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var mark = AddParticleTarget(owner, "Powerfist_buf.troy", owner, 1, "R_HAND");
            var p102 = AddParticleTarget(owner, "Powerfist_buf.troy", owner, 1, "L_HAND");
            var b1 = AddBuffHudVisual("PowerFist", 5f, 1, BuffType.COMBAT_ENCHANCER, owner);
            for (marktimeactive = 0.0f; marktimeactive < marktime; marktimeactive += updateinterval)
            {
                if (mark == null)
                {
                    RemoveParticle(mark);
                    return;
                }
                CreateTimer(marktimeactive, () => {
                    if (owner.IsAttacking && owner.AutoAttackTarget != null && mark != null)
                    {
                        RemoveBuffHudVisual(b1);
                        b1 = null;
                        RemoveParticle(mark);
                        RemoveParticle(p102);
                        OnProc(owner.AutoAttackTarget, false);
                        //spell.SpellAnimation("Spell1", owner);
                        return;
                    }
                });
            }
            CreateTimer(5.0125f, () =>
            {
                if (mark != null && b1 == null)
                {
                    //RemoveBuffHudVisual(b1);
                    RemoveParticle(mark);
                    RemoveParticle(p102);
                    mark = null;
                }
            });
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
        }

        private void AttackUnit(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var damage = (owner.Stats.AttackDamage.Total) * 2f;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            if (!(target is IObjBuilding) || !(target is ILaneTurret) || !(target is IBaseTurret))
            {
                DashToLocation((ObjAiBase)target, target.X, target.Y, 100, true, null, 50, 0, 0, 0.35f);
                CreateTimer(0.35f, () =>
                 {
                     CancelDash((ObjAiBase)target);
                 });
            }
        }
        
        public void OnUpdate(float diff)
        { 
        }
    }
}
