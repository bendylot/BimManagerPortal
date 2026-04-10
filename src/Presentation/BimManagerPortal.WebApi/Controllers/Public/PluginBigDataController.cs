using BimManagerPortal.Application.Features.PluginBigDatas.Commands.DeletePluginBigData;
using BimManagerPortal.Application.Features.PluginBigDatas.Commands.PostPluginBigData;
using BimManagerPortal.Application.Features.PluginBigDatas.Commands.SeedFakePluginBigData;
using BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigData;
using BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigDatas;
using BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigDatasPaged;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BimManagerPortal.WebApi.Controllers.Public;

[ApiController]
[Route("api/v1/public/plugin-big-data")]
public class PluginBigDataController
{
    public PluginBigDataController(IMediator mediator)
    {
        _mediator = mediator;
    }
    private readonly IMediator _mediator;

    #region Get

    [HttpGet]
    public async Task<IResult> GetAllPluginBigDatas()
    {
        try
        {
            var dto = await _mediator.Send(new GetPluginBigDatasQuery());
            return TypedResults.Ok(dto);
        }
        catch (KeyNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest($"Validation failed: {ex.Message}");
        }
        catch (Exception)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    [HttpGet("paged")]
    public async Task<IResult> GetAllPluginBigDatasPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? sortColumn = "CreatedAt",
        [FromQuery] bool sortAscending = false)
    {
        try
        {
            var dto = await _mediator.Send(
                new GetPluginBigDatasPagedQuery(page, pageSize, search, sortColumn, sortAscending));
            return TypedResults.Ok(dto);
        }
        catch (Exception)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetAllPluginBigData(string id)
    {
        try
        {
            var dto = await _mediator.Send(new GetPluginBigDataQuery(new GetPluginBigDataRequestDto(id)));
            return TypedResults.Ok(dto);
        }
        catch (KeyNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest($"Validation failed: {ex.Message}");
        }
        catch (Exception)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    #endregion
    
    #region Post

    [HttpPost]
    public async Task<IResult> PostPluginBigData([FromBody] PostPluginBigDataRequestDto postPluginBigDataRequestDto)
    {
        try
        {
            await _mediator.Send(new PostPluginBigDataCommand(postPluginBigDataRequestDto));
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest($"Validation failed: {ex.Message}");
        }
        catch (Exception)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    #endregion

    [HttpPost("seed-fake")]
    public async Task<IResult> SeedFakePluginBigData([FromQuery] int count = 20)
    {
        try
        {
            var created = await _mediator.Send(new SeedFakePluginBigDataCommand(count));
            return TypedResults.Ok(new { created });
        }
        catch (Exception)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    #region Delete

    [HttpDelete("{id}")]
    public async Task<IResult> DeletePluginBigData(string id)
    {
        try
        {
            await _mediator.Send(new DeletePluginBigDataCommand(id));
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest($"Validation failed: {ex.Message}");
        }
        catch (Exception)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    #endregion

}