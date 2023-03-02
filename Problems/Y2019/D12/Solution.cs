using System.Text.RegularExpressions;
using Problems.Attributes;
using Problems.Common;
using Problems.Y2019.Common;
using Utilities.Cartesian;
using Utilities.Numerics;

namespace Problems.Y2019.D12;

/// <summary>
/// The N-Body Problem: https://adventofcode.com/2019/day/12
/// </summary>
[Favourite("The N-Body Problem", Topics.Math, Difficulty.Hard)]
public class Solution : SolutionBase2019
{
    public override int Day => 12;
    
    public override object Run(int part)
    {
        var bodies = ParseInitialStates(GetInputLines());
        return part switch
        {
            0 => ComputeEnergyAfterSteps(bodies, steps: 1000),
            1 => ComputeFirstSystemCycle(bodies),
            _ => ProblemNotSolvedString
        };
    }

    private static int ComputeEnergyAfterSteps(Dictionary<Moon, State> bodies, int steps)
    {
        for (var i = 0; i < steps; i++)
        {
            bodies = StepBodies(bodies);
        }
        
        return bodies.Values.Sum(ComputeEnergy);
    }

    private static long ComputeFirstSystemCycle(Dictionary<Moon, State> bodies)
    {
        var cycles = new List<long>
        {
            FindCycle(Axis.X, bodies),
            FindCycle(Axis.Y, bodies),
            FindCycle(Axis.Z, bodies),
        };

        return NumericsHelper.Lcm(cycles);
    }

    private static int FindCycle(Axis component, Dictionary<Moon, State> bodies)
    {
        var step = 0;
        var states = new Dictionary<Tuple<StateComp, StateComp, StateComp, StateComp>, int>();

        while (step < int.MaxValue)
        {
            bodies = StepBodies(bodies);
            var key = Tuple.Create(
                new StateComp(component, bodies[Moon.Io]),
                new StateComp(component, bodies[Moon.Europa]),
                new StateComp(component, bodies[Moon.Ganymede]),
                new StateComp(component, bodies[Moon.Callisto]));

            if (states.ContainsKey(key))
            {
                return step - states[key];
            }

            states[key] = step;
            step++;
        }

        throw new NoSolutionException();
    }
    
    private static Dictionary<Moon, State> StepBodies(Dictionary<Moon, State> bodies)
    {
        var nextPoses = new Dictionary<Moon, State>();
        foreach (var (moon, state) in bodies)
        {
            var nextVel = new Vector3D(
                x: StepVelocityComponent(Axis.X, moon, bodies),
                y: StepVelocityComponent(Axis.Y, moon, bodies),
                z: StepVelocityComponent(Axis.Z, moon, bodies));
                
            nextPoses.Add(moon, new State(
                pos: state.Pos + nextVel, 
                vel: nextVel));
        }
        return nextPoses;
    }

    private static int StepVelocityComponent(Axis component, Moon target, Dictionary<Moon, State> bodies)
    {
        var pos = bodies[target].Pos.GetComponent(component);
        var vel = bodies[target].Vel.GetComponent(component);
        
        foreach (var (otherMoon, otherState) in bodies)
        {
            if (otherMoon == target)
            {
                continue;
            }

            var otherPos = otherState.Pos.GetComponent(component); 
            if (otherPos > pos)
            {
                vel++;
            }
            else if (otherPos < pos)
            {
                vel--;
            }
        }

        return vel;
    }

    private static int ComputeEnergy(State state)
    {
        var potential = Vector3D.Distance(Vector3D.Zero, state.Pos, DistanceMetric.Taxicab);
        var kinematic = Vector3D.Distance(Vector3D.Zero, state.Vel, DistanceMetric.Taxicab);

        return potential * kinematic;
    }

    private static Dictionary<Moon, State> ParseInitialStates(IList<string> input)
    {
        return new Dictionary<Moon, State>
        {
            { Moon.Io,       new State(ParsePosition(input[0]), Vector3D.Zero)},
            { Moon.Europa,   new State(ParsePosition(input[1]), Vector3D.Zero)},
            { Moon.Ganymede, new State(ParsePosition(input[2]), Vector3D.Zero)},
            { Moon.Callisto, new State(ParsePosition(input[3]), Vector3D.Zero)},
        };
    }

    private static Vector3D ParsePosition(string line)
    {
        var match = Regex.Match(line, @"<x=(-?\d+), y=(-?\d+), z=(-?\d+)>");
        return new Vector3D(
            x: int.Parse(match.Groups[1].Value),
            y: int.Parse(match.Groups[2].Value),
            z: int.Parse(match.Groups[3].Value));
    }
}