using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace RyzeR
{
    internal class RyzeR : IBuffGameScript
    {
        private IBuff _visualBuff;
        private StatsModifier _statMod;
        public void OnActivate(IObjAiBase unit,ISpell spell)
        {
            var time = 4 + spell.Level * 1f;
            _statMod = new StatsModifier();
            _statMod.SpellVamp.PercentBonus += (10 + spell.Level *5f) / 100f;
            _statMod.MoveSpeed.FlatBonus += 80f;
            unit.AddStatModifier(_statMod);
            _visualBuff = AddBuffHudVisual("desperatepower", time, 1, BuffType.COMBAT_ENCHANCER, unit);            
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
