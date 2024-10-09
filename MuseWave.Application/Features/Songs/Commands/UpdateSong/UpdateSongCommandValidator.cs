using FluentValidation;

namespace MuseWave.Application.Features.Songs.Commands.UpdateSong;

public class UpdateSongCommandValidator: AbstractValidator<UpdateSongCommand>
{
    public UpdateSongCommandValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        
        RuleFor(p => p.ArtistId)    
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        
        RuleFor(p => p.Genre)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
    }
}