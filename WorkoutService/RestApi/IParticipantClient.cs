using RestApi.Dtos;

namespace RestApi;

public interface IParticipantClient
{
    Task<ParticipantBasicDto> GetBasicAsync(int participantId);
}