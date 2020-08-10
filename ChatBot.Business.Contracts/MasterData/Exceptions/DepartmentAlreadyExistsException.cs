using System;

namespace ChatBot.Business.Contracts.MasterData.Exceptions
{
    /// <summary>
    ///     The exception that is thrown if one tries to create a department which already exists
    /// </summary>
    public class DepartmentAlreadyExistsException : Exception
    {
    }
}