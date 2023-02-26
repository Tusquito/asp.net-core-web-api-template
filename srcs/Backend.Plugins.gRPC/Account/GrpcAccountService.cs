﻿using Backend.Libs.Database.Abstractions;
using Backend.Libs.Database.Account;
using Backend.Libs.gRPC.Account;
using Backend.Libs.gRPC.Account.Request;
using Backend.Libs.gRPC.Account.Responses;
using Backend.Libs.gRPC.Enums;
using Backend.Plugins.Database.Entities;
using Microsoft.Extensions.Logging;

namespace Backend.Plugins.gRPC.Account;

public class GrpcAccountService : IGrpcAccountService
{
    private readonly IGenericUuidRepositoryAsync<AccountEntity, AccountDto> _accountRepository;
    private readonly ILogger<GrpcAccountService> _logger;

    public GrpcAccountService(IGenericUuidRepositoryAsync<AccountEntity, AccountDto> accountRepository, ILogger<GrpcAccountService> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<GrpcAccountResponse> GetAccountByIdAsync(GrpcGetAccountByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty)
            {
                return new GrpcAccountResponse { Type = GrpcResponseType.RequestError };
            }

            AccountDto? accountDto = await _accountRepository.GetByIdAsync(request.Id, cancellationToken);

            return new GrpcAccountResponse
            {
                AccountDto = accountDto,
                Type = accountDto == null ? GrpcResponseType.NotFound : GrpcResponseType.Success
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetAccountByIdAsync));
            return new GrpcAccountResponse { Type = GrpcResponseType.UnknownError };
        }
    }

    public async Task<GrpcAccountResponse> GetAccountByEmailAsync(GrpcGetAccountByStringRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Search))
            {
                return new GrpcAccountResponse { Type = GrpcResponseType.RequestError };
            }

            AccountDto? accountDto = await _accountRepository.FirstOrDefaultAsync(
                x => string.Equals(x.Email, request.Search, StringComparison.InvariantCulture), cancellationToken);

            return new GrpcAccountResponse
            {
                AccountDto = accountDto,
                Type = accountDto == null ? GrpcResponseType.NotFound : GrpcResponseType.Success
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetAccountByEmailAsync));
            return new GrpcAccountResponse { Type = GrpcResponseType.UnknownError };
        }
    }

    public async Task<GrpcAccountResponse> GetAccountByUsernameAsync(GrpcGetAccountByStringRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Search))
            {
                return new GrpcAccountResponse { Type = GrpcResponseType.RequestError };
            }

            AccountDto? accountDto = await _accountRepository.FirstOrDefaultAsync(
                x => string.Equals(x.Username, request.Search, StringComparison.InvariantCulture), cancellationToken);

            return new GrpcAccountResponse
            {
                AccountDto = accountDto,
                Type = accountDto == null ? GrpcResponseType.NotFound : GrpcResponseType.Success
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(GetAccountByUsernameAsync));
            return new GrpcAccountResponse { Type = GrpcResponseType.UnknownError };
        }
    }

    public async Task<GrpcAccountResponse> UpdateAccountAsync(GrpcSaveAccountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AccountDto == null)
            {
                return new GrpcAccountResponse { Type = GrpcResponseType.RequestError };
            }

            AccountDto? accountDto = await _accountRepository.UpdateAsync(request.AccountDto, cancellationToken);

            return new GrpcAccountResponse
            {
                AccountDto = accountDto,
                Type = accountDto == null ? GrpcResponseType.Failure : GrpcResponseType.Success
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(UpdateAccountAsync));
            return new GrpcAccountResponse { Type = GrpcResponseType.UnknownError };
        }
    }

    public async Task<GrpcAccountResponse> AddAccountAsync(GrpcSaveAccountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AccountDto == null)
            {
                return new GrpcAccountResponse { Type = GrpcResponseType.RequestError };
            }

            AccountDto? accountDto = await _accountRepository.AddAsync(request.AccountDto, cancellationToken);

            return new GrpcAccountResponse
            {
                AccountDto = accountDto,
                Type = accountDto == null ? GrpcResponseType.Failure : GrpcResponseType.Success
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[{Scope}]", nameof(AddAccountAsync));
            return new GrpcAccountResponse { Type = GrpcResponseType.UnknownError };
        }
    }
}