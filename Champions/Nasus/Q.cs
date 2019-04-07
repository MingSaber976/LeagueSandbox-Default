using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class NasusQ : IGameScript
    {
        public static Particle mark;
        public static float marktime = 8.0f;
        public static float marktimeactive;
        public static float updateinterval = 0.1f;
        public static byte stacks = 0;
        static IBuff buff01;
        static IChampion owner;
        static IAttackableUnit target;

        public void OnActivate(IChampion owner)
        {
            NasusQ.owner = owner;
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
            var damage = 10 + owner.Spells[0].Level *20f + ad + stacks;
            
            RemoveParticle(mark);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            if (target.IsDead)
            {
                stacks += 3;                
                //AddBuffHudVisual("NasusQStacks", -1, stack, BuffType.COUNTER, owner);
            }
            AddParticleTarget(owner, "Nasus_Base_Q_Tar.troy", target, 1);
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
            buff01 = AddBuffHudVisual("NasusQ", 8f, 1, BuffType.COMBAT_ENCHANCER, owner);
            owner.AddBuffGameScript("NasusQHud", "NasusQHud", spell, -1f, true);
            if (mark != null)
            {
                return;
            }
            mark = AddParticleTarget(owner, "Nasus_Base_Q_Buf.troy", owner, 1, "R_HAND");
            owner.Stats.Range.FlatBonus += 50;
            //var b1 = AddBuffHudVisual("NasusQ", 8f, 1, BuffType.COMBAT_ENCHANCER, owner);
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
                        //RemoveBuffHudVisual(b1);
                        RemoveParticle(mark);
                        OnProc(owner.AutoAttackTarget, false);
                        owner.Stats.Range.FlatBonus -= 50;
                        RemoveBuffHudVisual(buff01);
                        buff01 = null;
                        //spell.SpellAnimation("Spell1", owner);
                        return;
                    }
                });
            }
            CreateTimer(8.1f,()=> 
            {
                if (mark != null && buff01 == null)
                {
                //RemoveBuffHudVisual(b1);
                RemoveParticle(mark);
                mark = null;
                }
            });
            
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {

        }

        public void OnUpdate(float diff)
        {

        }
    }
}
