using System;

namespace ChatBot.AuthProvider.Contract.Exceptions
{
    /// <summary>
    ///     The exception that is thrown if one tries to create a user which already exists
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
    }
}