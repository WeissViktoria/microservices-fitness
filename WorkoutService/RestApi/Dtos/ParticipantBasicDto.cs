namespace RestApi.Dtos;


    public record ParticipantBasicDto(
        int ParticipantId,
        string Firstname,
        string Lastname,
        DateTime BirthDate,
        string Email,
        decimal Weight,
        decimal Height);
