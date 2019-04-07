using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class RenektonReignOfTheTyrant : IGameScript
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
            owner.AddBuffGameScript("NasusR", "NasusR", spell, 15f, true);
            owner.Stats.CurrentHealth += new[] { 200, 400, 800 }[spell.Level - 1];
            var p1 = AddParticleTarget(owner, "RenektonDominus_aura.troy", owner);
            var p2 = AddParticleTarget(owner, "RenektonDominus_transform.troy", owner);
            var p3 = AddParticleTarget(owner, "RenektonDominus_sword.troy", owner, 1, "R_Hand");

            for (byte e = 1; e <= 15; e++)
            {
                CreateTimer(e, () =>
                {
                    owner.Stats.CurrentMana += 5;
                    var ap = owner.Stats.AbilityPower.Total;
                    foreach (var units in GetUnitsInRange(owner, 225, true))
                    {
                        var damage = new[] { 30,60,120 }[spell.Level -1] + ap * 0.1f;
                        if (units.Team != owner.Team && !(units is ILaneTurret))
                        {
                            units.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        }
                    }
                    
                });
            }
            CreateTimer(14.5f, () => 
            {
                RemoveParticle(p1);
                RemoveParticle(p2);
                RemoveParticle(p3);
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
