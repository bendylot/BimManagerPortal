using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Shared.Dtos;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigDatasPaged;

public record GetPluginBigDatasPagedQuery(
    int Page,
    int PageSize,
    string? Search,
    string? SortColumn,
    bool SortAscending) : IRequest<PagedResultDto<GetAllPluginBigDatasDto>>;

public class GetPluginBigDatasPagedQueryHandler
    : IRequestHandler<GetPluginBigDatasPagedQuery, PagedResultDto<GetAllPluginBigDatasDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPluginBigDatasPagedQueryHandler> _logger;

    public GetPluginBigDatasPagedQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetPluginBigDatasPagedQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResultDto<GetAllPluginBigDatasDto>> Handle(
        GetPluginBigDatasPagedQuery query, CancellationToken cancellationToken)
    {
        var q = _unitOfWork.Repository<PluginBigData>().Entities.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.ToLower();
            q = q.Where(x =>
                x.ConfigurationName.ToLower().Contains(s) ||
                x.PluginName.ToLower().Contains(s) ||
                x.UserCreater.ToLower().Contains(s));
        }

        q = (query.SortColumn, query.SortAscending) switch
        {
            ("ConfigurationName", true)  => q.OrderBy(x => x.ConfigurationName),
            ("ConfigurationName", false) => q.OrderByDescending(x => x.ConfigurationName),
            ("UserCreater", true)        => q.OrderBy(x => x.UserCreater),
            ("UserCreater", false)       => q.OrderByDescending(x => x.UserCreater),
            ("CreatedAt", true)          => q.OrderBy(x => x.CreatedAt),
            _                            => q.OrderByDescending(x => x.CreatedAt),
        };

        var totalCount = await q.CountAsync(cancellationToken);

        var entities = await q
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(e => new
            {
                e.Id,
                e.PluginName,
                e.ConfigurationName,
                e.UserCreater,
                e.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var items = entities
            .Select(e => new GetAllPluginBigDatasDto(
                e.Id.ToString(),
                e.PluginName,
                e.ConfigurationName,
                e.UserCreater,
                e.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")))
            .ToList();

        _logger.LogInformation(
            "Paged query: page {Page}, size {PageSize}, total {Total}",
            query.Page, query.PageSize, totalCount);

        return new PagedResultDto<GetAllPluginBigDatasDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }
}
