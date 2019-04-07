using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace NasusR
{
    internal class NasusR : IBuffGameScript
    {
        private StatsModifier _statMod;
        private IBuff _visualBuff;
        private IParticle _particle;
        
        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
            _statMod = new StatsModifier();
            _statMod.Range.FlatBonus += 50;
            _statMod.HealthPoints.FlatBonus += 150 + ownerSpell.Level * 150f;
            _statMod.Size.FlatBonus += 0.175f;
            unit.AddStatModifier(_statMod);
            _visualBuff = AddBuffHudVisual("NasusR", 15.0f, 1, BuffType.COMBAT_ENCHANCER, unit);
            //Immunity to slowness not added
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            RemoveBuffHudVisual(_visualBuff);
            unit.RemoveStatModifier(_statMod);
        }

        private void OnAutoAttack(AttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
