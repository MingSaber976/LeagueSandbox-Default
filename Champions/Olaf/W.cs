using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class OlafFrenziedStrikes : IGameScript
    {
        private IChampion _owner;
        public void OnActivate(IChampion owner)
        {
            
            // if unit get healed, it will be "healing = healing + (healing * missHealth);"
        }

        private void ReduceCooldown(IAttackableUnit unit, bool isCrit)
        {
        //No Cooldown reduction on the other skills yet
        }

        public void OnDeactivate(IChampion owner)
        {
        }

        private void OnHit(IAttackableUnit target, bool isCrit)
        {
            var missingHealth = (_owner.Stats.HealthPoints.Total - _owner.Stats.CurrentHealth) * 0.02f;
            
        }
        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var p1 = AddParticleTarget(owner, "olaf_viciousStrikes_self.troy", owner, 1);
            var p201 = AddParticleTarget(owner, "olaf_viciousStrikes_weapon_glow.troy", owner, 1, "R_HAND");
            var p202 = AddParticleTarget(owner, "olaf_viciousStrikes_weapon_glow.troy", owner, 1, "L_HAND");
            //ApiEventManager.OnHitUnit.AddListener(this, _owner, OnHit);

            owner.AddBuffGameScript("OlafW", "OlafW", spell, 6.0f, true);
            
            CreateTimer(6.0f, () =>
            {
                RemoveParticle(p1);
                RemoveParticle(p201);
                RemoveParticle(p202);
            });
            //No increased durations on kills and assists yet
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
        }

        

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
