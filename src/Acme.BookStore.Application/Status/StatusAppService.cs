using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Acme.BookStore.Status.Dtos;


using EdgeSync.Common.Net.Attributes;
using EdgeSync.Common.Net.Exceptions;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Acme.BookStore.Status;

public class StatusAppService : ApplicationService, IStatusAppService
{
    // In-memory storage for demo purposes
    private static readonly Dictionary<int, ResourceDto> _resources = new();
    private static int _nextId = 1;
    
    /// <summary>
    /// For the AbpAuthorizationException:
    /// Returns 401 (unauthorized) if user has not logged in.
    /// Returns 403 (forbidden) if user has logged in.
    /// Returns 400 (bad request) for the AbpValidationException.
    /// Returns 404 (not found) for the EntityNotFoundException.
    /// Returns 403 (forbidden) for the IBusinessException (and IUserFriendlyException since it extends the IBusinessException).
    /// Returns 501 (not implemented) for the NotImplementedException.
    /// Returns 500 (internal server error) for other exceptions (those are assumed as infrastructure exceptions).
    /// </summary>
    /// <param name="exceptionType"></param>
    /// <returns>The status code of the exception</returns>
    /// <exception cref="UserFriendlyException"></exception>
    /// <exception cref="EntityNotFoundException"></exception>
    /// <exception cref="AbpAuthorizationException"></exception>
    /// <exception cref="AbpValidationException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="BusinessException"></exception>
    /// <exception cref="Exception"></exception>
    public Task<string> GetStatusAsync(ExceptionDto input)
    {
        throw input.ExceptionType switch
        {
            ExceptionType.UserFriendlyException => new UserFriendlyException("UserFriendlyException"),
            ExceptionType.EntityNotFoundException => new EntityNotFoundException("EntityNotFoundException"),
            ExceptionType.AbpAuthorizationException => new AbpAuthorizationException("AbpAuthorizationException"),
            ExceptionType.AbpValidationException => new AbpValidationException("AbpValidationException"),
            ExceptionType.NotImplementedException => new NotImplementedException("NotImplementedException"),
            ExceptionType.BusinessException => new BusinessException("BusinessException"),
            _ => new Exception("Unknown exception type"),
        };
    }
    
    /// <summary>
    /// Demonstrates returning 201 Created with location header
    /// </summary>
    [CreatedResponse]
    public Task<object> CreateAsync(CreateResourceDto input)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new EdgeSyncBadRequestException("Resource name is required");
        }
        
        if (input.Name.Length < 3)
        {
            throw new EdgeSyncUnprocessableEntityException("Resource name must be at least 3 characters long");
        }
        
        // Check for duplicates
        if (_resources.Values.Any(r => r.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new EdgeSyncConflictException($"A resource with the name '{input.Name}' already exists");
        }
        
        // Create the resource
        var resource = new ResourceDto
        {
            Id = _nextId++,
            Name = input.Name,
            Description = input.Description
        };
        
        _resources[resource.Id] = resource;
        
        // Return the created resource (201 status will be set by [CreatedResponse] attribute)
        return Task.FromResult<object>(resource);
    }
    
    /// <summary>
    /// Demonstrates returning 404 Not Found or 200 OK with resource
    /// </summary>
    public Task<object> GetAsync(int id)
    {
        if (!_resources.TryGetValue(id, out var resource))
        {
            throw new EdgeSyncNotFoundException($"Resource with ID {id} not found");
        }
        
        return Task.FromResult<object>(resource);
    }
    
    /// <summary>
    /// Demonstrates updating a resource with various error scenarios
    /// </summary>
    public Task<object> UpdateAsync(int id, UpdateResourceDto input)
    {
        // Check if resource exists
        if (!_resources.TryGetValue(id, out var resource))
        {
            throw new EdgeSyncNotFoundException($"Resource with ID {id} not found");
        }
        
        // Validation
        if (string.IsNullOrWhiteSpace(input.Name))
        {
            throw new EdgeSyncBadRequestException("Resource name cannot be empty");
        }
        
        // Check for permission (simulated)
        if (id == 999) // Special ID to simulate forbidden access
        {
            throw new EdgeSyncForbiddenException("You don't have permission to update this resource");
        }
        
        // Check for conflicts with other resources
        var duplicateExists = _resources.Values
            .Any(r => r.Id != id && r.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase));
        
        if (duplicateExists)
        {
            throw new EdgeSyncConflictException($"Another resource with the name '{input.Name}' already exists");
        }
        
        // Update the resource
        resource.Name = input.Name;
        resource.Description = input.Description;
        
        return Task.FromResult<object>(resource);
    }
    
    /// <summary>
    /// Demonstrates deleting a resource with authorization check
    /// </summary>
    public Task<object> DeleteAsync(int id)
    {
        // Simulate authentication check
        if (id == 401) // Special ID to simulate unauthorized
        {
            throw new EdgeSyncAuthorizationException("Please log in to delete resources");
        }
        
        // Simulate permission check
        if (id == 403) // Special ID to simulate forbidden
        {
            throw new EdgeSyncForbiddenException("You don't have permission to delete this resource");
        }
        
        // Check if resource exists
        if (!_resources.Remove(id))
        {
            throw new EdgeSyncNotFoundException($"Resource with ID {id} not found");
        }
        
        // Return success message
        return Task.FromResult<object>(new { message = $"Resource {id} deleted successfully" });
    }
}


