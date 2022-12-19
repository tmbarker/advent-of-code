using Problems.Y2022.Common;
using Utilities.DataStructures.Cartesian;

namespace Problems.Y2022.D18;

/// <summary>
/// Boiling Boulders: https://adventofcode.com/2022/day/18
/// </summary>
public class Solution : SolutionBase2022
{
    private const char Delimiter = ',';
    private const int CubeFaces = 6;
    
    public override int Day => 18;
    
    public override string Run(int part)
    {
        var surfaceVectors = ParseSurfaceVectors(GetInput());
        return part switch
        {
            0 => ComputeSurfaceArea(surfaceVectors).ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private static int ComputeSurfaceArea(IEnumerable<Vector3D> surface)
    {
        var surfaceArea = 0;
        var surfaceSet = new HashSet<Vector3D>(surface);
        
        foreach (var element in surfaceSet)
        {
            var faceAdjacentPositions = element.GetAdjacentSet(DistanceMetric.Taxicab);
            surfaceArea += CubeFaces - faceAdjacentPositions.Count(p => surfaceSet.Contains(p));
        }

        return surfaceArea;
    }

    private static IEnumerable<Vector3D> ParseSurfaceVectors(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var elements = line.Split(Delimiter);
            var x = int.Parse(elements[0]);
            var y = int.Parse(elements[1]);
            var z = int.Parse(elements[2]);
            
            yield return new Vector3D(x, y, z);
        }
    }
}