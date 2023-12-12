namespace Solutions.Y2019.D14;

public readonly struct Reaction(IEnumerable<Term> reactants, Term product)
{
    public IReadOnlyList<Term> Reactants { get; } = new List<Term>(reactants);
    public Term Product { get; } = product;

    public override string ToString()
    {
        var reactantStrings = Reactants.Select(r => $"{r.Amount} {r.Substance}");
        return $"{string.Join(',', reactantStrings)} => {Product.Amount} {Product.Substance}";
    }
}