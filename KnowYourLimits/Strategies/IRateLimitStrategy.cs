﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KnowYourLimits.Identity;
using KnowYourLimits.Strategies.LeakyBucket;
using Microsoft.AspNetCore.Http;

// This is a library, so not all of the methods are used in the project.
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

namespace KnowYourLimits.Strategies
{
    /// <summary>
    /// The strategy by which request allowances are calculated.
    /// </summary>
    public interface IRateLimitStrategy<TIdentityType, TConfig> 
        where TIdentityType : IClientIdentity
        where TConfig : BaseRateLimitConfiguration
    {
        /// <summary>
        /// Checks if the <param name="identity"></param> has available allowance.
        /// </summary>
        /// <param name="identity">The identity of the client</param>
        /// <returns>Returns true if the <param name="identity"></param> has remaining requests, false otherwise</returns>
        bool HasRemainingAllowance(TIdentityType identity, TConfig config);
        /// <summary>
        /// Gets the remaining allowance for the <param name="identity"></param>.
        /// </summary>
        /// <param name="identity">The identity of the client</param>
        /// <returns>The remaining allowance</returns>
        long GetRemainingAllowance(TIdentityType identity, TConfig config);

        /// <summary>
        /// Reduces the <param name="identity"></param> available allowance by <param name="requests"></param>.
        /// </summary>
        /// <param name="identity">The identity of the client</param>
        /// <param name="requests">The number of requests to reduce the allowance by</param>
        /// <returns>The new remaining allowance</returns>
        long ReduceAllowanceBy(TIdentityType identity, long requests);

        /// <summary>
        /// Reduces the <param name="identity"></param> available allowance by the <param name="config"></param> amount.
        /// </summary>
        /// <param name="identity">The identity of the client</param>
        /// <param name="config">The configuration</param>
        /// <returns>The new remaining allowance</returns>
        long ReduceAllowanceBy(TIdentityType identity, TConfig config);

        /// <summary>
        /// Increases the <param name="identity"></param> available allowance by <param name="requests"></param>.
        /// </summary>
        /// <param name="identity">The identity of the client</param>
        /// <param name="requests">The number of requests to increase the allowance by</param>
        /// <returns>The new remaining allowance</returns>
        long IncreaseAllowanceBy(TIdentityType identity, long requests);

        /// <summary>
        /// Handles the internals of the rate limiting strategy, calling a callback based on the result.
        /// </summary>
        /// <param name="onHasRequestsRemaining">A callback to be called when the rate limit has not been hit</param>
        /// <param name="onNoRequestsRemaining">A callback to be called when the rate limit has been hit</param>
        /// <param name="context">The HTTPContext for the current request</param>
        /// <returns></returns>
        Task OnRequest(Func<Task> onHasRequestsRemaining, Func<Task> onNoRequestsRemaining, TIdentityType identity, TConfig config);
        ///
        /// <summary>
        /// Gets a set of headers that represents the rate limit status of the <param name="identity"></param>.
        /// </summary>
        /// <param name="identity">The identity of the client</param>
        /// <returns>Headers</returns>
        Dictionary<string, string> GetResponseHeaders(TIdentityType identity, TConfig config);
        /// <summary>
        /// Check if headers should be added to the response
        /// </summary>
        /// <returns>True if headers describing the rate limiting strategy should be added to the response else false.</returns>
        bool ShouldAddHeaders(TConfig config);
    }
}
