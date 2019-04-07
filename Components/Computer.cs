using System;
using System.Collections.Generic;
using Dataiku.Models;

namespace Dataiku.Components
{
    public class Computer
    {
        public int Autonomy { get;}
        public string Departure { get;}
        public string Arrival { get;}

        private readonly int[][] _galaxyMatrix;
        private readonly Dictionary<string, int> _planetLookup;
        private readonly Dictionary<int, string> _indexLookup;

        public Computer(int autonomy, string departure, string arrival, List<Route> routes)
        {
            Autonomy = autonomy;
            Departure = departure;
            Arrival = arrival;
            (_planetLookup, _indexLookup) = BuildPlanetLookups(routes);
            _galaxyMatrix = BuildGalaxyMatrix(routes);

            if(!_planetLookup.ContainsKey(Departure)){
                throw new ArgumentException($"Routes does not contains the departure {Departure}");
            }

            if(!_planetLookup.ContainsKey(Arrival)){
                throw new ArgumentException($"Routes does not contains the arrival {Arrival}");
            }
        }

        private int[][] BuildGalaxyMatrix(List<Route> routes)
        {
            var planetCount = _planetLookup.Count;
            var galaxyMatrix = new int[planetCount][];

            for (int i = 0; i < planetCount; i++)
            {
                galaxyMatrix[i] = new int[planetCount];
            }

            foreach (var route in routes)
            {
                galaxyMatrix[_planetLookup[route.Origin]][_planetLookup[route.Destination]] = route.TravelTime;
                galaxyMatrix[_planetLookup[route.Destination]][_planetLookup[route.Origin]] = route.TravelTime;
            }

            return galaxyMatrix;
        }

        private (Dictionary<string, int>, Dictionary<int, string>) BuildPlanetLookups(List<Route> routes)
        {
            var planetLookup = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            var indexLookup = new Dictionary<int, string>();
            var count = 0;
            foreach (var route in routes)
            {
                if (!planetLookup.ContainsKey(route.Origin))
                {
                    planetLookup[route.Origin] = count;
                    indexLookup[count] = route.Origin;
                    count++;
                }
                if (!planetLookup.ContainsKey(route.Destination))
                {
                    planetLookup[route.Destination] = count;
                    indexLookup[count] = route.Destination;
                    count++;
                }
            }

            return (planetLookup, indexLookup);
        }

        public int GetTravelTime(string origin, string destination)
        {
            return _galaxyMatrix[_planetLookup[origin]][_planetLookup[destination]];
        }

        public IEnumerable<string> GetReachablePlanets(string origin)
        {
            if(!_planetLookup.TryGetValue(origin, out var originIndex))
            {
                yield break;
            }

            for(int i = 0; i < _planetLookup.Count; ++i)
            {
                if(_galaxyMatrix[originIndex][i] > 0)
                {
                    yield return _indexLookup[i];
                }
            }
        }
    }
}
