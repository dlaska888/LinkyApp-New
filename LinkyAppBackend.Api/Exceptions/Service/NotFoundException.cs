﻿namespace LinkyAppBackend.Api.Exceptions.Service;

public class NotFoundException(string message) : Exception(message);