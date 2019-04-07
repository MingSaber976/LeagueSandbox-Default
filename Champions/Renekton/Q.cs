using System.Linq;
using GameServerCore;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class RenektonCleave : IGameScript
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
            spell.SpellAnimation("Spell1", owner);
            var ad = owner.Stats.AttackDamage.Total;
            var damage = 30f + spell.Level * 30f + ad * 0.8f;
            var range = GetUnitsInRange(owner, 325, true);
            var healing = 0f;
            if (owner.Stats.CurrentMana < 50)
            {
                AddParticleTarget(owner, "RenektonCleave_trail.troy", owner);
            }
            if (owner.Stats.CurrentMana >= 50)
            {
                AddParticleTarget(owner, "renektoncleave_trail_rage,troy", owner);
                damage = 45f + spell.Level * 45f + ad * 1.2f;
            }
            foreach (var units in range)
            {
                if (units.Team != owner.Team && units is IChampion || units is IMinion)
                {
                    owner.Stats.CurrentMana += 5;
                    units.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    if (units is IChampion)
                    {
                        if (owner.Stats.CurrentMana < 50)
                        {
                            healing += damage * 0.2f;
                        }
                        if (owner.Stats.CurrentMana >= 50)
                        {
                            healing += damage * 0.4f;
                        }
                    }
                    if (units is IMinion)
                    {
                        healing += damage * 0.05f;
                    }
                }
            }
            if (owner.Stats.CurrentMana < 50)
            {
                if (healing > 25 + spell.Level * 25)
                {
                    healing = 25f + spell.Level * 25f;
                }
            }
            if (owner.Stats.CurrentMana >= 50)
            {
                owner.Stats.CurrentMana -= 50;
                if (healing > 75 + spell.Level * 75)
                {
                    healing = 75f + spell.Level * 75f;
                }
            }
            owner.Stats.CurrentHealth += healing;
            healing = 0;
                
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {

        }

        public void OnUpdate(double diff)
        {
        }
    }
}
