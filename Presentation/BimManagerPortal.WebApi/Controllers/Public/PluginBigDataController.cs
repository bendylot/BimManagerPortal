using BimManagerPortal.Application.Features.PluginBigDatas.Commands.PostPluginBigData;
using BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigData;
using BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigDatas;
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
            return TypedResults.BadRequest(ex.Message);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest($"Validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
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
            return TypedResults.BadRequest(ex.Message);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest($"Validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
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
            return TypedResults.BadRequest(ex.Message);
        }
        catch (ValidationException ex)
        {
            return TypedResults.BadRequest($"Validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(
                title: "Internal Server Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    #endregion
    
}