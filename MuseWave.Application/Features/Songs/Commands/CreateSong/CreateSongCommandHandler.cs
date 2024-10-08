using MediatR;
using MuseWave.Application.Persistence;
using MuseWave.Domain.Entities;

namespace MuseWave.Application.Features.Songs.Commands.CreateSong
{
    public class CreateSongCommandHandler : IRequestHandler<CreateSongCommand, CreateSongCommandResponse>
    {
        private readonly ISongRepository repository;
        
        public CreateSongCommandHandler(ISongRepository repository)
        {
            this.repository = repository;
        }
        
        public async Task<CreateSongCommandResponse> Handle(CreateSongCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateSongCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                return new CreateSongCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validationResult.Errors.Select(error => error.ErrorMessage).ToList()
                };
            }
            
            var song = Song.Create(request.Title, request.ArtistId, request.AlbumId, request.Genre, request.AudioFile, request.ReleaseDate);            if(!song.IsSuccess)
            {
                return new CreateSongCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { song.Error }
                };
            }

            await repository.AddAsync(song.Value);

            return new CreateSongCommandResponse
            {
                Success = true,
                Song = new CreateSongDto
                {
                    Id = song.Value.Id,
                    Title = song.Value.Title,
                    ArtistId = song.Value.ArtistId,
                    AlbumId = song.Value.AlbumId,
                    Genre = song.Value.Genre,
                    AudioFile = song.Value.AudioFile,
                    ReleaseDate = song.Value.ReleaseDate
                }
            };
        }
    }
}

