namespace Recommendations.Domain;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public List<Discussion> Discussions { get; set; } = new();
}