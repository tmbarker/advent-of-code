namespace Problems.Y2019.D14;

public readonly struct Reaction
{
    public Reaction(IEnumerable<Term> reactants, Term product)
    {
        Reactants = new List<Term>(reactants);
        Product = product;
    }
    
    public IReadOnlyList<Term> Reactants { get; }
    public Term Product { get; }

    public override string ToString()
    {
        var reactantStrings = Reactants.Select(r => $"{r.Amount} {r.Substance}");
        return $"{string.Join(',', reactantStrings)} => {Product.Amount} {Product.Substance}";
    }
}