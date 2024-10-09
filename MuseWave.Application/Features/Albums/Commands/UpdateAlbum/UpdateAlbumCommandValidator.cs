using FluentValidation;

namespace MuseWave.Application.Features.Albums.Commands.UpdateAlbum;

public class UpdateAlbumCommandValidator: AbstractValidator<UpdateAlbumCommand>
{
    public UpdateAlbumCommandValidator()
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