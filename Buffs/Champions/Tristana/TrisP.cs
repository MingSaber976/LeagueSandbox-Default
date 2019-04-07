using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace TrisP
{
    internal class TrisP : IBuffGameScript
    {
        private StatsModifier _statMod;
        private IBuff _visualBuff;

        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
            _statMod = new StatsModifier();            
            _statMod.Range.FlatBonus = _statMod.Range.FlatBonus + unit.Stats.Level * 10;
            unit.AddStatModifier(_statMod);
            _visualBuff = AddBuffHudVisual("Draw a Bead", -1, 1, BuffType.COMBAT_ENCHANCER, unit);
            //Immunity to slowness not added
        }

        public void OnDeactivate(IObjAiBase unit)
        {            
        }

        private void OnAutoAttack(AttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
