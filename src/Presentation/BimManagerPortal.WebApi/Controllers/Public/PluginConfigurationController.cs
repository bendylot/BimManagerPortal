using BimManagerPortal.Application.Features.PluginConfigurations.Commands.DeletePluginConfiguration;
using BimManagerPortal.Application.Features.PluginConfigurations.Commands.PostPluginConfiguration;
using BimManagerPortal.Application.Features.PluginConfigurations.Commands.PutPluginConfiguration;
using BimManagerPortal.Application.Features.PluginConfigurations.Queries.GetPluginConfiguration;
using BimManagerPortal.Application.Features.PluginConfigurations.Queries.GetPluginConfigurations;
using BimManagerPortal.Shared.Other.Dtos.Requests.PluginConfigs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BimManagerPortal.WebApi.Controllers.Public;

[ApiController]
[Route("api/v1/public/plugin-configurations")]
public class PluginConfigurationController
{
    private readonly IMediator _mediator;

    public PluginConfigurationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IResult> GetAll()
    {
        try
        {
            var result = await _mediator.Send(new GetPluginConfigurationsQuery());
            return TypedResults.Ok(result);
        }
        catch (Exception)
        {
            return TypedResults.Problem(detail: "An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetById(string id)
    {
        try
        {
            var result = await _mediator.Send(new GetPluginConfigurationQuery(id));
            return TypedResults.Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception)
        {
            return TypedResults.Problem(detail: "An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public async Task<IResult> Create([FromBody] PluginConfigRequestDto dto)
    {
        try
        {
            await _mediator.Send(new PostPluginConfigurationCommand(dto));
            return TypedResults.Ok();
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return TypedResults.Problem(detail: "An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id}")]
    public async Task<IResult> Update(string id, [FromBody] PluginConfigRequestDto dto)
    {
        try
        {
            await _mediator.Send(new PutPluginConfigurationCommand(id, dto));
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return TypedResults.Problem(detail: "An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(string id)
    {
        try
        {
            await _mediator.Send(new DeletePluginConfigurationCommand(id));
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (Exception)
        {
            return TypedResults.Problem(detail: "An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
