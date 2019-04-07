using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace NasusQHud
{
    internal class NasusQHud : IBuffGameScript
    {
        private StatsModifier _statMod;
        private IBuff _visualBuff;
        private IParticle _particle;
        
        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
            _visualBuff = AddBuffHudVisual("NasusQStacks", 0.1f, Spells.NasusQ.stacks, BuffType.COMBAT_ENCHANCER, unit);
            //Immunity to slowness not added
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            RemoveBuffHudVisual(_visualBuff);
        }

        private void OnAutoAttack(AttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(double diff)
        {
        }
    }
}
