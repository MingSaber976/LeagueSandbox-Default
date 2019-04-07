using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class RunePrison : IGameScript
    {
        
        public void OnActivate(IChampion owner)
        {
            
        }

        public void OnDeactivate(IChampion owner)
        {

        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            spell.SpellAnimation("Spell2", owner);
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var hasbuff = owner.HasBuffGameScriptActive("RyzeR", "RyzeR");
            if (hasbuff)
            {
                var time = 0.5f + spell.Level * 0.25f;
                var ap = owner.Stats.AbilityPower.Total;
                var mana = owner.Stats.ManaPoints.Total;
                var damage = (25 + spell.Level * 35f + ap * 0.6f + mana * 0.045f)/2;
                ((ObjAiBase)target).AddBuffGameScript("Root", "Root", spell, time, true);
                AddBuffHudVisual("Root", time, 1, BuffType.COMBAT_DEHANCER, (ObjAiBase)target, time);
                var p1 = AddParticleTarget(owner, "RunePrison_tar.troy", target);
                var p2 = AddParticleTarget(owner, "RunePrison_cas.troy", owner);
                foreach (var i in GetUnitsInRange(target, 200, true))
                {
                    if (owner.Team != i.Team && i is IAttackableUnit)
                    {
                        AddParticleTarget(owner, "DesperatePower_aoe.troy", target);
                        i.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
                
                CreateTimer(time, () =>
                {
                    RemoveParticle(p1);
                    RemoveParticle(p2);
                });
            }
            if (hasbuff == false)
            {
                var time = 0.5f + spell.Level * 0.25f;
                var ap = owner.Stats.AbilityPower.Total;
                var mana = owner.Stats.ManaPoints.Total;
                var damage = 25 + spell.Level * 35f + ap * 0.6f + mana * 0.045f;
                ((ObjAiBase)target).AddBuffGameScript("Root", "Root", spell, time, true);
                AddBuffHudVisual("Root", time, 1, BuffType.COMBAT_DEHANCER, (ObjAiBase)target, time);
                var p1 = AddParticleTarget(owner, "RunePrison_tar.troy", target);
                var p2 = AddParticleTarget(owner, "RunePrison_cas.troy", owner);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                CreateTimer(time, () =>
                {
                    RemoveParticle(p1);
                    RemoveParticle(p2);
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

