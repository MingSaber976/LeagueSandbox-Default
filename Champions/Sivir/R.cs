using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using GameServerCore.Domain;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Spells
{
    public class SivirR : IGameScript
    {
        
        public void OnActivate(IChampion owner)
        {
        }
                
        public void OnDeactivate(IChampion owner)
        {
        }

        public void OnStartCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var timing = spell.Level * 2f;
                        
            foreach (var allys in GetUnitsInRange(owner, 1000, true)
                .Where(x => x.Team != CustomConvert.GetEnemyTeam(owner.Team)))
            {
                if (owner != allys && allys is IChampion || allys is IMinion || allys is ILaneMinion)
                {                    
                    var b1 = AddBuffHudVisual("SivirR", 8f, 1, BuffType.COMBAT_DEHANCER, (ObjAiBase)allys);
                    var p1 = AddParticleTarget(owner, "Sivir_Base_R_alliesbuf.troy", allys); 
                    var p2 = AddParticleTarget(owner, "Sivir_Base_R_Allyboost.troy", allys);
                    var p3 = AddParticleTarget(owner, "Sivir_Base_R_Boost.troy", allys);
                    //var p4 = AddParticle(owner, "Sivir_Base_R_mis.troy", allys.X,allys.Y);

                    allys.Stats.MoveSpeed.PercentBonus += (60f/100f);
                    CreateTimer(timing, () =>
                    {                        
                        allys.Stats.MoveSpeed.PercentBonus -= (60f / 100f);
                        allys.Stats.MoveSpeed.PercentBonus += (20f/100f);
                    });
                    CreateTimer(8f, () =>
                    {
                        allys.Stats.MoveSpeed.PercentBonus -= (20f/100f);
                        RemoveParticle(p1);
                        RemoveParticle(p2);
                        RemoveParticle(p3);
                        RemoveBuffHudVisual(b1);
                    });
                }
            }
            
            
        }

        public void OnFinishCasting(IChampion owner, ISpell spell, IAttackableUnit target)
        {
            var timing = spell.Level * 2f;

            var p1 = AddParticleTarget(owner, "Sivir_base_R_buf.troy", owner);
            var b1 = AddBuffHudVisual("SivirR", 8f, 1, BuffType.COMBAT_DEHANCER, owner);
            var p101 = AddParticleTarget(owner, "Sivir_Base_R_alliesbuf.troy", owner);
            var p201 = AddParticleTarget(owner, "Sivir_Base_R_Allyboost.troy", owner);
            var p301 = AddParticleTarget(owner, "Sivir_Base_R_Boost.troy", owner);
            owner.Stats.MoveSpeed.PercentBonus += (60f/100f);
            CreateTimer(timing, () =>
            {                
                owner.Stats.MoveSpeed.PercentBonus -= (60f / 100f);
                owner.Stats.MoveSpeed.PercentBonus += (20f/100f);
            });
            CreateTimer(8f, () =>
            {
                owner.Stats.MoveSpeed.PercentBonus -= (20f/100f);
                RemoveParticle(p1);
                RemoveParticle(p101);
                RemoveParticle(p201);
                RemoveParticle(p301);
                RemoveBuffHudVisual(b1);
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
