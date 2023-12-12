using Utilities.Geometry.Euclidean;
using Utilities.Numerics;

namespace Problems.Y2019.D12;

[PuzzleInfo("The N-Body Problem", Topics.Math, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var bodies = ParseInitialStates(GetInputLines());
        return part switch
        {
            1 => ComputeEnergyAfterSteps(bodies, steps: 1000),
            2 => ComputeFirstSystemCycle(bodies),
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
            FindCycle(Axis.Z, bodies)
        };

        return Numerics.Lcm(cycles);
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

            if (states.TryGetValue(key, out var state))
            {
                return step - state;
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
                Pos: state.Pos + nextVel, 
                Vel: nextVel));
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
        var potential = state.Pos.Magnitude(Metric.Taxicab);
        var kinematic = state.Vel.Magnitude(Metric.Taxicab);

        return potential * kinematic;
    }

    private static Dictionary<Moon, State> ParseInitialStates(IList<string> input)
    {
        return new Dictionary<Moon, State>
        {
            { Moon.Io,       new State(Pos: Vector3D.Parse(input[0]), Vel: Vector3D.Zero)},
            { Moon.Europa,   new State(Pos: Vector3D.Parse(input[1]), Vel: Vector3D.Zero)},
            { Moon.Ganymede, new State(Pos: Vector3D.Parse(input[2]), Vel: Vector3D.Zero)},
            { Moon.Callisto, new State(Pos: Vector3D.Parse(input[3]), Vel: Vector3D.Zero)}
        };
    }
}