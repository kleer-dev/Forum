namespace Recommendations.Domain;

public class Image
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string Url { get; set; }
    public string FolderName { get; set; }
    
    public Discussion Discussion { get; set; }
}