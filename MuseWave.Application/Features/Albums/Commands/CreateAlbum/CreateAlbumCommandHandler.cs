using MediatR;
using MuseWave.Application.Persistence;
using MuseWave.Application.Services;
using MuseWave.Domain.Entities;

namespace MuseWave.Application.Features.Albums.Commands.CreateAlbum
{
    public class CreateAlbumCommandHandler : IRequestHandler<CreateAlbumCommand, CreateAlbumCommandResponse>
    {
        private readonly IAlbumRepository repository;
        private readonly AlbumService albumService;

        public CreateAlbumCommandHandler(IAlbumRepository repository, AlbumService albumService)
        {
            this.repository = repository;
            this.albumService = albumService;
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
            
            var albumResult = albumService.CreateAlbum(request.Title, request.ArtistId, request.Genre, request.ReleaseDate, request.CoverImage);
            if (!albumResult.IsSuccess)
            {
                return new CreateAlbumCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { albumResult.Error }
                };
            }

            await repository.AddAsync(albumResult.Value);

            return new CreateAlbumCommandResponse
            {
                Success = true,
                Album = new AlbumDto
                {
                    Id = albumResult.Value.Id,
                    Title = albumResult.Value.Title,
                    ArtistId = albumResult.Value.ArtistId,
                    Genre = albumResult.Value.Genre,
                    ReleaseDate = albumResult.Value.ReleaseDate,
                    CoverImage = albumResult.Value.CoverImage
                }
            };
        }
    }
}

