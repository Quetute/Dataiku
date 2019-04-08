# Launch the app

1. Download .Net Core runtime and SDK [here](https://dotnet.microsoft.com/download)
2. Add your files and specify the path of the millenium falcon in `appsettings.json`
3. Build and publish the solution: `dotnet publish -r [Runtime] -c Release`  and target the runtime you want ([list here](https://github.com/dotnet/docs/blob/master/docs/core/rid-catalog.md)).
4. Launch the solution: `./bin/Release/netcoreapp2.2/[Runtime]/Dataiku.exe`.
5. open your  browser on `https://localhost:5001`

# Design
 
We assume that the inputs are correct and very little verification is done. We also assume that the galaxy cannot change once the program has started. 

At startup time, build a matrix of the galaxy, with the `travelTime` as value. We also a maintain a lookup table to associate an index in the matrix to planet name. This give us a constant time to get a route between two planets, and a minimal time to get all routes from a planet. 

When the user upload the empire plan file, we first build a bounty hunter lookup table, in order to know in constant time if a planet is occuped on a given day. We then start a recursion in order to find the safest path. The terminating conditions are when we reach the countdown, or when  we reach the destination or when we find a perfectly safe path. 

The time complexity of this method is in worst case `O(n^d)`, where `n` is the number of planets and `d` the countdown. The worst case is when any planet is reachable from any other planet, the time travel between each of them is 1 and that we have bounty hunters on the destination planet every day. An average time complexity is `O(a^(d / t))`, where `n` is the average number of routes from a given planet, `d` the countdown and `t` the average route time travel.   

The space complexity is `O(max(n^2, b))`, `b` being the number of days the bounty hunter are deployed on any planet. 


# Improvement

The method is sure to terminate because of the coutdown terminating condition. As with every recursive method, we can still get a `stackOverflow` exception if the input is big enough. Also, for big input, the time complexity will exponentially increase. 

We could terminate the recursion when the number of bounty hunters met is higher than the current solution we have. For example, if a route from the current planet to the destination has already been found and we have met 3 bounty hunters , we can stop other recursion as soon as 3 or more bounty hunters are met from this planet.

We could also chose a heuristic in order to chose the next route to try. For example, we could pre compute every possible paths to get to any planet, and given a destination, try these paths first. We could also try routes where we are less likely to meet bounty hunters. 

We could also have some memoization. in place. Given the current day, the current autonomy and the planet, we could store the result of the recursion for these arguments.

The current solution has been manually tested with the examples given, but as we want production code, unit tests are missing.    