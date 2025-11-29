using RestApi.Dtos;

namespace RestApi;

public class ParticipantClient :IParticipantClient
{
    private readonly HttpClient _http;
 
    public ParticipantClient(HttpClient http)
    {
        _http = http;
    }
 
    public async Task<ParticipantBasicDto?> GetBasicAsync(int participantId)
    {
        // calls your ParticipantService
        return await _http.GetFromJsonAsync<ParticipantBasicDto>(
            $"participant/{participantId}/basic");
    }
}