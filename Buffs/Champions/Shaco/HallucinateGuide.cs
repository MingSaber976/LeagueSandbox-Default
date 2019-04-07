using GameServerCore.Enums;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;

namespace HallucinateGuide
{
    internal class HallucinateGuide : IBuffGameScript
    {
        public void OnActivate(IObjAiBase unit, ISpell ownerSpell)
        {
            var s = new Spells.HallucinateFull();
            //ApiEventManager.OnUpdate.AddListener(this, OnUpdate);
        }

        public void OnDeactivate(IObjAiBase unit)
        {
            
        }

        public void OnUpdate(float diff)
        {

        }

        public void OnUpdate(double diff)
        {

        }
    }
}