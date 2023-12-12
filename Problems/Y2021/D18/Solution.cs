using Utilities.Extensions;

namespace Problems.Y2021.D18;

using SfNumber = List<Element>;

[PuzzleInfo("Snailfish", Topics.StringParsing, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private const int ExplodeThreshold = 5;
    private const int SplitThreshold = 10;

    public override object Run(int part)
    {
        var numbers = ParseInputLines(parseFunc: SnailfishParser.Parse).ToList();
        return part switch
        {
            1 => ComputeMagnitude(SumSfNumbers(numbers)),
            2 => ComputeMaxPairSumMagnitude(numbers),
            _ => ProblemNotSolvedString
        };
    }

    private static int ComputeMaxPairSumMagnitude(IList<SfNumber> numbers)
    {
        var max = 0;
        
        for (var i = 0; i < numbers.Count; i++)
        for (var j = 0; j < numbers.Count; j++)
        {
            if (i == j)
            {
                continue;
            }
            
            max = Math.Max(max, ComputeMagnitude(Add(numbers[i], numbers[j])));
            max = Math.Max(max, ComputeMagnitude(Add(numbers[j], numbers[i])));
        }
        
        return max;
    }
    
    private static int ComputeMagnitude(IEnumerable<Element> number)
    {
        var simplified = new SfNumber(number);
        while (true)
        {
            var lastOpenIndex = 0;
            for (var i = 0; i < simplified.Count; i++)
            {
                if (simplified[i].ElementType == Element.Type.Open)
                {
                    lastOpenIndex = i;
                    continue;
                }

                if (simplified[i].ElementType != Element.Type.Close)
                {
                    continue;
                }

                var lhsValue = simplified[lastOpenIndex + 1].Value;
                var rhsValue = simplified[lastOpenIndex + 3].Value;
                var magnitude = 3 * lhsValue + 2 * rhsValue;

                simplified.RemoveRange(lastOpenIndex, 5);
                simplified.Insert(lastOpenIndex, new Element(magnitude));
                
                break;
            }

            if (simplified.Count == 1)
            {
                return simplified[0].Value;
            }
        }
    }

    private static IEnumerable<Element> SumSfNumbers(IList<SfNumber> numbers)
    {
        var sum = numbers.First();
        for (var i = 1; i < numbers.Count; i++)
        {
            sum = Add(sum, numbers[i]);
        }
        return sum;
    }

    private static SfNumber Add(IEnumerable<Element> a, IEnumerable<Element> b)
    {
        var n = new SfNumber { Element.Open };
        n.AddRange(a);
        n.Add(Element.Delim);
        n.AddRange(b);
        n.Add(Element.Close);
        
        return Reduce(n);
    }
    
    private static SfNumber Reduce(SfNumber n)
    {
        while (true)
        {
            if (TryExplode(n) || TrySplit(n))
            {
                continue;
            }
            
            return n;
        }
    }

    private static bool TryExplode(SfNumber elements)
    {
        var pairCounter = 0;
        var length = elements.Count;
        
        for (var i = 0; i < length; i++)
        {
            switch (elements[i].ElementType)
            {
                case Element.Type.Open:
                    pairCounter++;
                    break;
                case Element.Type.Close:
                    pairCounter--;
                    break;
            }

            if (pairCounter < ExplodeThreshold)
            {
                continue;
            }

            var lhsValue = elements[i + 1].Value;
            var rhsValue = elements[i + 3].Value;

            elements.RemoveRange(i, 5);
            elements.Insert(i, new Element(0));
            
            for (var l = i - 1; l >= 0; l--)
            {
                if (elements[l].ElementType == Element.Type.Value)
                {
                    elements[l] = new Element(elements[l].Value + lhsValue);
                    break;
                }
            }
            
            for (var r = i + 1; r < elements.Count; r++)
            {
                if (elements[r].ElementType == Element.Type.Value)
                {
                    elements[r] = new Element(elements[r].Value + rhsValue);
                    break;
                }
            }

            return true;
        }

        return false;
    }

    private static bool TrySplit(IList<Element> elements)
    {
        for (var i = 0; i < elements.Freeze().Count; i++)
        {
            if (elements[i].ElementType != Element.Type.Value || elements[i].Value < SplitThreshold)
            {
                continue;
            }

            var lhs = (int)Math.Floor(elements[i].Value / 2f);
            var rhs = (int)Math.Ceiling(elements[i].Value / 2f);
            
            elements.RemoveAt(i);
            elements.Insert(i, Element.Close);
            elements.Insert(i, new Element(rhs));
            elements.Insert(i, Element.Delim);
            elements.Insert(i, new Element(lhs));
            elements.Insert(i, Element.Open);
            
            return true;
        }

        return false;
    }
}