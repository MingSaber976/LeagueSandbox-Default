using System;
using System.Linq;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class SpellFlux : IGameScript
    {
        
        public void OnActivate(IChampion owner)
        {
            
        }

        public void OnDeactivate(IChampion owner)
        {

        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            spell.SpellAnimation("Spell3", owner);
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            spell.AddProjectileTarget("spellflux", target, false);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            projectile.SetToRemove();
            var hasbuff = owner.HasBuffGameScriptActive("RyzeR", "RyzeR");
            var ap = owner.Stats.AbilityPower.Total;
            var mana = owner.Stats.ManaPoints.Total;
            var damage = 30 + spell.Level * 20f + ap * 0.35f + mana * 0.01f;
            var reduce = 9 + spell.Level * 3f;
            if (hasbuff)
            {
                damage = damage / 2;
                foreach (var I in GetUnitsInRange(target, 200, true))
                {
                    if (owner.Team != I.Team && I is IAttackableUnit)
                    {
                        AddParticleTarget(owner, "DesperatePower_aoe.troy", target);
                        I.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
            }
            if (hasbuff == false)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
            
            foreach (var B in GetUnitsInRange(target, 400, true).Take(4))
            {
                if (owner.Team != B.Team && B is IChampion || B is IMinion && target != B)
                {
                    for (int q = 0; q <= 5; q++)
                    {
                        CreateTimer(q, () =>
                         {
                             var temp = 0;
                            //CreateTimer(1.5f, () => {
                            if (temp <= 5)
                             {
                                 var a = owner.X;
                                 var b = owner.Y;
                                 TeleportTo(owner, target.X, target.Y);
                                 spell.AddProjectileTarget("SpellFluxMissile", B, false);
                                 TeleportTo(owner, a, b);
                                 temp++;
                             }
                         });
                        //});
                    }
                }
            }
            
        }

        public void OnUpdate(double diff)
        {
        }
    }
}

