using System;
using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class IreliaTranscendentBlades : IGameScript
    {
        public byte temp = 0;
        public void OnActivate(IChampion owner)
        {
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            AddParticleTarget(owner, "irelia_ult_energy_active.troy", owner);
            AddParticleTarget(owner, "irelia_ult_magic_resist.troy", owner);
            AddParticleTarget(owner, "irelia_ult_energy_ready.troy", owner);
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            //AddParticleTarget(owner, "irelia_ult_cas.troy", owner);
            spell.SpellAnimation("Spell4", owner);
            var current = new Vector2(owner.X, owner.Y);
            var to = Vector2.Normalize(new Vector2(spell.X, spell.Y) - current);
            var range = to * 1000;
            var trueCoords = current + range;

            spell.AddProjectile("IreliaTranscendentBladesSpell", owner.X, owner.Y, trueCoords.X, trueCoords.Y, false);
            temp++;
            if (temp < 4)
            {
                CreateTimer(0.2f, () =>
                {
                    owner.Spells[3].LowerCooldown(owner.Spells[3].GetCooldown());
                });
            }
            
            if (temp == 4)
            {
                temp = 0;
            }
            else { return; }
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            AddParticleTarget(owner, "irelia_ult_tar.troy", target);
            var bonusAd = owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue;
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 40 + spell.Level * 40f + bonusAd * 0.6f + ap * 0.5f;
            var defaultDamage = 40 + spell.Level * 40f;
            if (owner != target)
            {
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
            if (target is IChampion)
            {
                owner.Stats.CurrentHealth += defaultDamage * (25f / 100f);
            }
            else
            {
                owner.Stats.CurrentHealth += defaultDamage * (10f / 100f);
            }
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
