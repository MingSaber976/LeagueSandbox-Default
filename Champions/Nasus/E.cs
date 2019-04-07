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
    public class NasusE : IGameScript
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
            var ap = owner.Stats.AbilityPower.Total;
            var Firstdamage = 15 + spell.Level * 40f + ap * 0.6f;
            var Secdamage = 3 + spell.Level * 8f + 0.12f;
            //TODO: reduce armor
            IMinion m = AddMinion(owner, "TestCube", "TestCube", spell.X, spell.Y);
            AddParticle(owner, "Nasus_Base_E_Warning.troy", spell.X, spell.Y);
            AddParticle(owner, "Nasus_Base_E_SpiritFire.troy", spell.X, spell.Y);
            AddParticle(owner, "Nasus_Base_E_Staff_Swirl.troy", spell.X, spell.Y);
            AddParticle(owner, "Nasus_E_Green_Ring.troy", spell.X, spell.Y);
            CreateTimer(0.215f, () =>
            {
                foreach (var units in GetUnitsInRange(m, 400, true))
                {
                    if (units.Team != owner.Team && units != m)
                    {
                        units.TakeDamage(owner, Firstdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
            });
            for (float e = 0.715f; e <= 5.715; e++)
                {
                    CreateTimer(e, () =>
                    {
                        foreach (var enemy in GetUnitsInRange(m, 400, true))
                        {
                            if (enemy.Team != owner.Team && enemy != m)
                            {
                                enemy.TakeDamage(owner, Secdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                            }
                        }
                    });
                }
            
            CreateTimer(5.715f, () =>
                 {
                     m.Die(m);
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
