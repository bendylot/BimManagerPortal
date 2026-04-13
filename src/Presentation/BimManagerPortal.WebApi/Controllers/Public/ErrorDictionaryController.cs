using BimManagerPortal.Application.Features.ErrorDictionary.Commands.CreateErrorDictionaryEntry;
using BimManagerPortal.Application.Features.ErrorDictionary.Commands.DeleteErrorDictionaryEntry;
using BimManagerPortal.Application.Features.ErrorDictionary.Commands.UpdateErrorDictionaryEntry;
using BimManagerPortal.Application.Features.ErrorDictionary.Queries.GetErrorDictionaryEntries;
using BimManagerPortal.Shared.Dtos.ErrorDictionary;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BimManagerPortal.WebApi.Controllers.Public;

[ApiController]
[Route("api/v1/public/error-dictionary")]
public class ErrorDictionaryController
{
    public ErrorDictionaryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    private readonly IMediator _mediator;

    #region Get

    [HttpGet]
    public async Task<IResult> GetAll()
    {
        try
        {
            var dto = await _mediator.Send(new GetErrorDictionaryEntriesQuery());
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

    #endregion

    #region Post

    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateErrorDictionaryEntryRequestDto dto)
    {
        try
        {
            await _mediator.Send(new CreateErrorDictionaryEntryCommand(dto));
            return TypedResults.Ok();
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

    #region Put

    [HttpPut]
    public async Task<IResult> Update([FromBody] UpdateErrorDictionaryEntryRequestDto dto)
    {
        try
        {
            await _mediator.Send(new UpdateErrorDictionaryEntryCommand(dto));
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

    #region Delete

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(string id)
    {
        try
        {
            await _mediator.Send(new DeleteErrorDictionaryEntryCommand(id));
            return TypedResults.Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return TypedResults.NotFound(ex.Message);
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
