﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBot.MessageInterpreter.Contract.Models;

namespace ChatBot.MessageInterpreter.Contract
{
    public interface IMessageInterpreter
    {
        /// <summary>
        ///     Processes a message and returns questions until all required parameters could be collected.
        ///     It then returns those parameters
        /// </summary>
        /// <param name="message"></param>
        /// <param name="contextId"></param>
        /// <returns></returns>
        Task<InterpretationResult> InterpretMessageAsync(string message, string contextId);

        /// <summary>
        ///     Returns all terms known by the interpreter
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<KnowledgeTerm>> GetAllKnownTermsAsync();

        /// <summary>
        ///     Returns all known definition types
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetAllDefinitionTypeNamesAsync();

        /// <summary>
        ///     Deletes the term from the message interpreter
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        Task DeleteKnownTermAsync(string term);

        /// <summary>
        ///     Indicated whether the specified definition type exists
        /// </summary>
        /// <param name="definitionType"></param>
        /// <returns></returns>
        Task<bool> DefinitionTypeExistsAsync(string definitionType);

        /// <summary>
        ///     Creates the specified definition
        /// </summary>
        /// <param name="term"></param>
        /// <param name="synonyms"></param>
        /// <returns></returns>
        Task<KnowledgeTerm> CreateKnowledgeTermAsync(string term, IEnumerable<string> synonyms);
    }
}