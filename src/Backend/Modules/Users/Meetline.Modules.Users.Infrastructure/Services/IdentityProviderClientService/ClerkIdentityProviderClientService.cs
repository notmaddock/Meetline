using System.Net;
using Clerk.BackendAPI;
using Clerk.BackendAPI.Models.Errors;
using FluentResults;
using Meetline.Modules.Users.Application.Services;

namespace Meetline.Modules.Users.Infrastructure.Services.IdentityProviderClientService;

public class ClerkIdentityProviderClientService(ClerkBackendApi clerk) : IIdentityProviderClientService
{
    public async Task<Result<UserSyncData>> GetUser(string externalId, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        try
        {
            var response = await clerk.Users.GetAsync(externalId);

            var user = response.User;

            if (user is null)
                return Result.Fail<UserSyncData>(
                    new Error($"Clerk returned no user payload for '{externalId}'."));

            var email =
                user.EmailAddresses.FirstOrDefault(e => e.Id == user.PrimaryEmailAddressId)?.EmailAddressValue
                ?? user.EmailAddresses.FirstOrDefault()?.EmailAddressValue;

            if (string.IsNullOrWhiteSpace(email))
                return Result.Fail<UserSyncData>(
                    new Error($"Clerk user '{externalId}' does not have an email address."));

            return Result.Ok(new UserSyncData(
                user.Id,
                user.Username ?? string.Empty,
                email,
                user.FirstName,
                user.LastName));
        }
        catch (SDKBaseError ex) when (ex.Response.StatusCode == HttpStatusCode.NotFound)
        {
            return Result.Fail<UserSyncData>(
                new Error($"Clerk user '{externalId}' was not found."));
        }
        catch (SDKBaseError ex)
        {
            return Result.Fail<UserSyncData>(
                new Error($"Clerk API error while fetching user '{externalId}': {ex.Message}"));
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return Result.Fail<UserSyncData>(
                new Error($"Unexpected error while fetching Clerk user '{externalId}': {ex.Message}"));
        }
    }
}