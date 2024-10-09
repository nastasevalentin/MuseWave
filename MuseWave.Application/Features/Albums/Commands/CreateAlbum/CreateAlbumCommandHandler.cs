using MediatR;
using MuseWave.Application.Persistence;
using MuseWave.Domain.Entities;

namespace MuseWave.Application.Features.Albums.Commands.CreateAlbum
{
    public class CreateAlbumCommandHandler : IRequestHandler<CreateAlbumCommand, CreateAlbumCommandResponse>
    {
        private readonly IAlbumRepository repository;
        
        public CreateAlbumCommandHandler(IAlbumRepository repository)
        {
            this.repository = repository;
        }
        
        public async Task<CreateAlbumCommandResponse> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateAlbumCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                return new CreateAlbumCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(error => error.ErrorMessage).ToList()
                };
            }
            
            var album = Album.Create(request.Title, request.ArtistId, request.Genre, request.ReleaseDate, request.CoverImage);            
            if(!album.IsSuccess)
            {
                return new CreateAlbumCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { album.Error }
                };
            }

            await repository.AddAsync(album.Value);

            return new CreateAlbumCommandResponse
            {
                Success = true,
                Album = new AlbumDto
                {
                    Id = album.Value.Id,
                    Title = album.Value.Title,
                    ArtistId = album.Value.ArtistId,
                    Genre = album.Value.Genre,
                    ReleaseDate = album.Value.ReleaseDate,
                    CoverImage = album.Value.CoverImage
                }
            };
        }
    }
}

