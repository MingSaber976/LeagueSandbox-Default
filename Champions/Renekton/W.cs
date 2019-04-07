using System.Numerics;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class RenektonPreExecute : IGameScript
    {
        public static Particle mark;
        public static float marktime = 7.0f;
        public static float marktimeactive;
        public static float updateinterval = 0.1f;
        static IChampion owner;
        static ISpell spell;

        public void OnActivate(IChampion owner)
        {
            RenektonPreExecute.owner = owner;
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
            var damage = (-10 + owner.Spells[1].Level *20f + ad * 1.5f) / 3;
            var i = (byte)2;
            var time = 0.75f;
            if (owner.Stats.CurrentMana >= 50)
            {
                i = 3;
                time = 1.5f;
                damage = (-15f + owner.Spells[1].Level * 30f + ad * 2.25f) / 3;
                owner.Stats.CurrentMana -= 50;
            }
            //((ObjAiBase)target).AddBuffGameScript("Stun", "Stun", spell, time, true);
            //AddBuffHudVisual("Stun", time, 1, BuffType.COMBAT_DEHANCER, (ObjAiBase)target, time);
            AddParticleTarget(owner, "Global_Stun.troy", target, 1f, "head");
            RemoveParticle(mark);
            for (var p = 1; p <= i; p++)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, "Renekhton_Execute_tar.troy", target, 1);
            }
                        
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
            mark = AddParticleTarget(owner, "Renekton_Weapon_Hot.troy", owner, 1, "R_HAND");
            var b1 = AddBuffHudVisual("RenektonPreExecute", 7f, 1, BuffType.COMBAT_ENCHANCER, owner);
            owner.Stats.Range.FlatBonus += 50;
            //var b1 = AddBuffHudVisual("NasusQ", 8f, 1, BuffType.COMBAT_ENCHANCER, owner);
            for (marktimeactive = 0.0f; marktimeactive < marktime; marktimeactive += updateinterval)
            {
                if (mark == null)
                {
                    RemoveParticle(mark);
                    break;
                }                
                CreateTimer(marktimeactive, () => {
                    if (owner.IsAttacking && owner.AutoAttackTarget != null && mark != null)
                    {
                        RemoveBuffHudVisual(b1);
                        b1 = null;
                        RemoveParticle(mark);
                        OnProc(owner.AutoAttackTarget, false);
                        owner.Stats.Range.FlatBonus -= 50;
                        spell.SpellAnimation("Spell2", owner);
                        return;
                    }
                });
            }
            CreateTimer(7.1f,()=> 
            {
                if (mark != null)
                {
                    //RemoveBuffHudVisual(b1);
                    RemoveParticle(mark);
                    mark = null;
                }
                if (b1 != null)
                {
                    RemoveBuffHudVisual(b1);
                    b1 = null;
                }
                else
                {
                    return;
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
