using System;
using System.Collections.Generic;
using Dataiku.Models;

namespace Dataiku.Components
{
    public class OddCalculator
    {
        private readonly Computer _computer;
        private int _countdown;
        private Dictionary<string, HashSet<int>> _bountyHunterLookup;

        public OddCalculator(Computer computer, Empire empire)
        {
            _computer = computer;
            _countdown = empire.Countdown;
            _bountyHunterLookup = BuildBountyHunterLookup(empire.BountyHunters);
        }

        public double CalculateOdds()
        {

            return CalculateOdds(0, _computer.Departure, _computer.Autonomy, 0);
        }

        private double CalculateOdds(int currentDay, string currentPlanet, int currentAutonomy, int timesCaptured)
        {
            // Because we have four parameters, memoization seems too expensive for what it will provides.
            // We could maybe reduce the number of parameters involved in memoization, but it is not straightforward.

            // We assume that if bounty hunters are on the destination planet, we still have a risk to get caught. 
            if(_bountyHunterLookup.TryGetValue(currentPlanet, out var days) && days.Contains(currentDay))
            {
                ++timesCaptured;
            }

            if (currentDay <= _countdown && currentPlanet == _computer.Arrival)
            {
                return CalculateOdds(timesCaptured);
            }

            if(currentDay >= _countdown)
            {
                return 0;
            }

            double max = 0;

            foreach(var reachablePlanet in _computer.GetReachablePlanets(currentPlanet))
            {
                int travelTime = _computer.GetTravelTime(currentPlanet, reachablePlanet); 
                if (travelTime <= currentAutonomy)
                {
                    max = Math.Max(max, CalculateOdds(currentDay + travelTime, reachablePlanet, currentAutonomy - travelTime, timesCaptured));
                    
                    // if we find a perfectly safe route, we can stop here
                    if(max == 1)
                    {
                        return max;
                    }
                }
            }

            // Wait one day and refuel
            max = Math.Max(max, CalculateOdds(currentDay + 1, currentPlanet, _computer.Autonomy, timesCaptured));
            return max;
        }

        private double CalculateOdds(int timesCaptured)
        {
            double res = 1;
            for(int i = 0; i < timesCaptured; i++)
            {
                res -= Math.Pow(9, i) / Math.Pow(10, i + 1);
            }

            return res;
        }

        private Dictionary<string, HashSet<int>> BuildBountyHunterLookup(IEnumerable<BountyHunter> bountyHunters)
        {
            var bountyHuntersLookup = new Dictionary<string, HashSet<int>>();
            foreach(var bountyHunter in bountyHunters ?? new BountyHunter[]{})
            {
                if(!bountyHuntersLookup.ContainsKey(bountyHunter.Planet)){
                    bountyHuntersLookup[bountyHunter.Planet] = new HashSet<int>();
                }

                bountyHuntersLookup[bountyHunter.Planet].Add(bountyHunter.Day);
            }

            return bountyHuntersLookup;
        }
    }
}