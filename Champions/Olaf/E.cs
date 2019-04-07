using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class OlafRecklessStrike : IGameScript
    {
        private ISpell _spell;
        static IChampion owner;
        public void OnActivate(IChampion owner)
        {
            ApiEventManager.OnUpdate.AddListener(this, OnUpdate);
            // wait for API (OnAutoAttack)
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        private void OnProc(IAttackableUnit target, bool isCrit)
        {
            //_spell.LowerCooldown(1);
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var adcost = owner.Stats.AttackDamage.Total;
            var healthcost = 7.5f + spell.Level * 13.5f + adcost * 0.3f;
            if (owner.Stats.CurrentHealth > 0) { 
            
            spell.SpellAnimation("Spell3", owner);
            var p1 = AddParticleTarget(owner, "olaf_recklessStrike_cas_L.troy", owner, 1, "L_HAND");
            var p2 = AddParticleTarget(owner, "olaf_recklessStrike_cas.troy", owner, 1, "R_HAND");
            AddParticleTarget(owner, "olaf_recklessStrike_self.troy", owner, 1);
            AddParticleTarget(owner, "olaf_recklessStrike_axe_charge.troy", target, 1);
            CreateTimer(0.35f, () =>
            {
                RemoveParticle(p1);
                RemoveParticle(p2);
            });
            }
            if (owner.Stats.CurrentHealth > healthcost)
            {
                owner.Stats.CurrentHealth -= healthcost;
            }
            if (owner.Stats.CurrentHealth <= healthcost)
            {
                owner.Stats.CurrentHealth = 1;
            }
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var adcost = owner.Stats.AttackDamage.Total;
            var healthcost = 7.5f + spell.Level * 13.5f + adcost * 0.3f;
            AddParticleTarget(owner, "olaf_recklessSwing_tar.troy", target, 1);
            AddParticleTarget(owner, "olaf_recklessSwing_tar_02.troy", target, 1);
            AddParticleTarget(owner, "olaf_recklessSwing_tar_03.troy", target, 1);
            AddParticleTarget(owner, "olaf_recklessSwing_tar_04.troy", target, 1);
            AddParticleTarget(owner, "olaf_recklessSwing_tar_05.troy", target, 1);
            var ad = owner.Stats.AttackDamage.Total * 0.4f;
            var damage = 25f + spell.Level * 45 + ad;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
                       
            
            if (target.IsDead)
            {
                owner.Stats.CurrentHealth += healthcost;
            }

        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
            
        }

        public void OnUpdate(float diff)
        {
            
        }
    }
}

