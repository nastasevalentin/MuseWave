using MuseWave.Domain.Common;

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
    
    public static Result<Genre> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Genre>.Failure("name is required");
        }
        
        return Result<Genre>.Success(new Genre(name));
    }
    
}