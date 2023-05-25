using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentPartiesManager
    {
        private Dictionary<AgentParty, List<AgentParty>> enemies = new Dictionary<AgentParty, List<AgentParty>>();

        public void AddParty(AgentParty party, List<AgentParty> enemyParties)
        {
            enemies.Add(party, enemyParties);
        }

        public bool IsHostile(AgentParty partyA, AgentParty partyB)
        {
            return enemies[partyA].Contains(partyB);
        }

        public bool IsFriendly(AgentParty partyA, AgentParty partyB)
        {
            return IsHostile(partyA, partyB) == false;
        }
    }
}