using Utilities.Extensions;

namespace Solutions.Y2021.D21;

[PuzzleInfo("Dirac Dice", Topics.Math|Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private const int BoardPlaces = 10;
    private const int QuantumDieSides = 3;
    private const int WinningScoreDeterministic = 1000;
    private const int WinningScoreQuantum = 21;

    private static readonly IReadOnlyList<int> QuantumRollSums = new List<int>(GetQuantumRollSums());

    public override object Run(int part)
    {
        var initialPositions = ParseInitialPositions(GetInputLines());
        var initialState = new GameState(
            P1: new Player(Position: initialPositions.P1, Score: 0),
            P2: new Player(Position: initialPositions.P2, Score: 0));
        
        return part switch
        {
            1 => PlayDeterministicGame(initialState),
            2 => PlayQuantumGame(initialState),
            _ => ProblemNotSolvedString
        };
    }
    
    private static long PlayDeterministicGame(GameState state)
    {
        var die = new DeterministicDie();
        
        while (true)
        {
            state.P1 = TakeTurn(state.P1, die.Roll() + die.Roll() + die.Roll());
            
            if (state.P1.Score >= WinningScoreDeterministic)
            {
                return die.NumRolls * state.P2.Score;
            }

            (state.P1, state.P2) = (state.P2, state.P1);
        }
    }

    private static IEnumerable<int> GetQuantumRollSums()
    {
        for (var r1 = 1; r1 <= QuantumDieSides; r1++)
        for (var r2 = 1; r2 <= QuantumDieSides; r2++)
        for (var r3 = 1; r3 <= QuantumDieSides; r3++)
        {
            yield return r1 + r2 + r3;
        }
    }
    
    private static long PlayQuantumGame(GameState state)
    {
        var memo = new Dictionary<GameState, WinCounts>();
        var winCounts = GetWinCounts(state, memo);

        return Math.Max(winCounts.P1, winCounts.P2);
    }

    private static WinCounts GetWinCounts(GameState state, IDictionary<GameState, WinCounts> memo)
    {
        if (state.P1.Score >= WinningScoreQuantum)
        {
            return new WinCounts(1, 0);
        }
    
        if (state.P2.Score >= WinningScoreQuantum)
        {
            return new WinCounts(0, 1);
        }

        if (memo.TryGetValue(state, out var counts))
        {
            return counts;
        }
        
        var winCounts = new WinCounts(0, 0);
        foreach (var rollSum in QuantumRollSums)
        {
            var p1 = TakeTurn(state.P1, rollSum);
            var branch = GetWinCounts(new GameState(state.P2, p1), memo);

            winCounts = new WinCounts(winCounts.P1 + branch.P2, winCounts.P2 + branch.P1);
        }

        memo[state] = winCounts;
        return winCounts;
    }
    
    private static Player TakeTurn(Player player, int rollSum)
    {
        player.Position = (player.Position + rollSum - 1) % BoardPlaces + 1;
        player.Score += player.Position;
        
        return player;
    }

    private static (int P1, int P2) ParseInitialPositions(IList<string> input)
    {
        var p1 = input[0].ParseInts()[1];
        var p2 = input[1].ParseInts()[1];
        
        return (p1, p2);
    }
}