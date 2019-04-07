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
    public class NasusR : IGameScript
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
            var model = owner.Model;
            owner.ChangeModel("NasusUlt");
            owner.AddBuffGameScript("NasusR", "NasusR", spell, 15f, true);
            owner.Stats.CurrentHealth += 150f + spell.Level * 150f;
            var p1 = AddParticleTarget(owner, "Nasus_Base_R_Aura.troy", owner);
            var p2 = AddParticleTarget(owner, "Nasus_Base_R_Avatar.troy", owner);
            var p3 = AddParticleTarget(owner, "Nasus_Base_R_HandGlow.troy", owner, 1, "R_Hand");

            for (byte e = 1; e <= 15; e++)
            {
                CreateTimer(e, () =>
                {
                    var ap = owner.Stats.AbilityPower.Total;
                    foreach (var units in GetUnitsInRange(owner, 225, true))
                    {
                        var damage = units.Stats.HealthPoints.Total * ((2f + spell.Level + ap * 0.01f) / 100f);
                        if (damage > 240)
                        {
                            damage = 240;
                        }
                        if (units.Team != owner.Team && !(units is ILaneTurret))
                        {
                            units.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        }
                    }
                    
                });
            }
            CreateTimer(14.5f, () => 
            {
                owner.ChangeModel(model);
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
