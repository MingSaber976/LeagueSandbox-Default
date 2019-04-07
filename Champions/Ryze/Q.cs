using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class Overload : IGameScript
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
            spell.AddProjectileTarget("Overload", target, false);
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            var hasbuff = owner.HasBuffGameScriptActive("RyzeR", "RyzeR");
            var ap = owner.Stats.AbilityPower.Total;
            var mana = owner.Stats.ManaPoints.Total;
            var damage = 20 + spell.Level * 20f + ap * 0.4f + mana * 0.0065f;
            if (hasbuff)
            {
                damage = damage / 2;
                foreach (var i in GetUnitsInRange(target, 200, true))
                {
                    if (owner.Team != i.Team && i is IAttackableUnit)
                    {
                        AddParticleTarget(owner, "DesperatePower_aoe.troy", target);
                        i.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
                projectile.SetToRemove();
            }
            if (hasbuff == false)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(owner, "Overload_tar.troy", target);
                projectile.SetToRemove();
            }
        }

        public void OnUpdate(double diff)
        {
        }
    }
}

