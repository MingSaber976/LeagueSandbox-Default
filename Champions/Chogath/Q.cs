using System;
using System.Numerics;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class Rupture : IGameScript
    {
        private StatsModifier _stat;
        private static IBuff p1;
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
            //var units = IObjAiBase;
            //var p1 = Particle; 
            _stat = new StatsModifier();
            _stat.MoveSpeed.PercentBonus -= 60f / 100f;
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 25 + spell.Level * 55f + ap;
            AddParticle(owner, "rupture_cas_01.troy", spell.X, spell.Y, 1);
            AddParticle(owner, "rupture_cas_01_green_team.troy", spell.X, spell.Y, 1);
            CreateTimer(0.625f, () =>
             {
                 IMinion m = AddMinion(owner, "TestCube", "TestCube", spell.X, spell.Y);
                 AddParticle(owner, "rupture_cas_02.troy", spell.X, spell.Y);
                 foreach (var units in GetUnitsInRange(m, 200, true))
                 {
                     IObjAiBase enemy = (ObjAiBase)units;
                     if (units.Team != owner.Team && units != m)
                     {
                         enemy.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                         DashToLocation(enemy, enemy.X+10, enemy.Y+10, 10, false, null, 10, 0, 0, 1f);
                         CancelDash(enemy);
                         enemy.AutoAttackTarget = owner;
                         //enemy.AddBuffGameScript("Knockup", "Knockup", spell, 1f, false);
                     }
                 }
                 m.Die(m);
             });
            CreateTimer(0.625f + 1f, () =>
              {
                  IMinion m = AddMinion(owner, "TestCube", "TestCube", spell.X, spell.Y);
                  foreach (var units in GetUnitsInRange(m, 200, true))
                  {
                      IObjAiBase enemy = (ObjAiBase)units;
                      if (units.Team != owner.Team && units != m)
                      {
                          //p1 = AddBuffHudVisual("Slow", 1.5f, 1, BuffType.COMBAT_DEHANCER, enemy, 1.5f);                          
                          enemy.AddStatModifier(_stat);
                          //enemy.Stats.MoveSpeed.PercentBonus -= 60f / 100f;
                      }
                  };
                  m.Die(m);
              });
            CreateTimer(0.625f + 2.5f, () =>
            {
                IMinion m = AddMinion(owner, "TestCube", "TestCube", spell.X, spell.Y);
                foreach (var units in GetUnitsInRange(m, 300, true))
                {
                    IObjAiBase enemy = (ObjAiBase)units;
                    if (units.Team != owner.Team)
                    {
                        //enemy.ClearMovementUpdated();
                        enemy.RemoveStatModifier(_stat);
                        //enemy.Stats.MoveSpeed.PercentBonus += 60f / 100f;
                    }
                };
                m.Die(m);
                p1 = null;
            });
        }

        public void ApplyEffects(IChampion owner, IAttackableUnit target, ISpell spell, IProjectile projectile)
        {

        }

        public void OnUpdate(double diff)
        {
        }
    }
}
