﻿namespace LinkyAppBackend.Api.Exceptions.Service;

public class BadRequestException(string message) : Exception(message);