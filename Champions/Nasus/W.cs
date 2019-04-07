using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects;

namespace Spells
{
    public class NasusW : IGameScript
    {
        private StatsModifier _statMod;

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
            _statMod = new StatsModifier();
            var slowStart = 35f / 100f;
            var slowPre = (spell.Level * 12f / 100f) / 5;
            AddBuffHudVisual("NasusW", 5f, 1, BuffType.COMBAT_DEHANCER, (ObjAiBase)target, 5f);
            target.Stats.MoveSpeed.PercentBonus -= slowStart;
            var p101 = AddParticleTarget(owner, "Nasus_Base_W_Glowy_Hands.troy", target, 1, "Hand");
            //var p102 = AddParticleTarget(owner, "Nasus_Base_W_Glowy_Hands.troy", target, 1, "L_Hand");
            var p201 = AddParticleTarget(owner, "Nasus_Base_W_Hand_Glow.troy", target, 1, "Hand");
            //var p202 = AddParticleTarget(owner, "Nasus_Base_W_Hand_Glow.troy", target, 1, "L_Hand");
            var p3 = AddParticleTarget(owner, "Nasus_Base_W_Runes.troy", target);
            var p4 = AddParticleTarget(owner, "Nasus_Base_W_tar.troy", target);
            //_statMod.AttackSpeed.PercentBonus -= slowStart / 2;
            //owner.Stats.AddModifier(_statMod);
            //_statMod.AttackSpeed.PercentBonus += slowStart / 2;

            for (byte p = 1; p <= 5; p++)
            {
                CreateTimer(p, () =>
                {
                    target.Stats.MoveSpeed.PercentBonus -= slowPre;
                    //_statMod.AttackSpeed.PercentBonus -= slowPre / 2;
                    //owner.Stats.AddModifier(_statMod);
                    //_statMod.AttackSpeed.PercentBonus += slowPre / 2;
                });
            }
            CreateTimer(5f, () =>
            {
                target.Stats.MoveSpeed.PercentBonus += slowStart + slowPre * 5;
                RemoveParticle(p101);
                //RemoveParticle(p102);
                RemoveParticle(p201);
                //RemoveParticle(p202);
                RemoveParticle(p3);
                RemoveParticle(p4);
                //_statMod.AttackSpeed.PercentBonus -= (slowPre / 2) * 5;
                //_statMod.AttackSpeed.PercentBonus -= slowStart / 2;
                //owner.Stats.RemoveModifier(_statMod);
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
