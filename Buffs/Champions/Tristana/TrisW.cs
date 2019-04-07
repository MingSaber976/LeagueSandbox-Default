using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace TrisWSlow
{
    internal class TrisWSlow : IBuffGameScript
    {
        private StatsModifier _statMod;
        private IBuff _visualBuff;

        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
            _statMod = new StatsModifier();
            _statMod.MoveSpeed.PercentBonus -= (60f / 100f);
            unit.AddStatModifier(_statMod);
            var slowtime = 0.5f + ownerSpell.Level * 0.5f;
            _visualBuff = AddBuffHudVisual("Slow", slowtime, 1, BuffType.COMBAT_DEHANCER, unit);
            //Immunity to slowness not added
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            RemoveBuffHudVisual(_visualBuff);
            unit.RemoveStatModifier(_statMod);
        }
        
        public void OnUpdate(double diff)
        {
        }
    }
}
