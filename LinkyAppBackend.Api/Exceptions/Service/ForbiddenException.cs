namespace LinkyAppBackend.Api.Exceptions.Service;

public class ForbiddenException(string message) : Exception(message);