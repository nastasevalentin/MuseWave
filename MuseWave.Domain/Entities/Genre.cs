namespace MuseWave.Domain.Entities;

public class Genre
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    private Genre(string name)
    {
        Id = new Guid();
        Name = name;
    }
    
}