using System.Text.Json;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using MediatR;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Commands.PostPluginBigData;

public record PostPluginBigDataCommand(PostPluginBigDataRequestDto PostPluginConfigurationRequestDto) : IRequest;

internal class PostPluginBigDataCommandHandler : IRequestHandler<PostPluginBigDataCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompressionService _compression;
    public PostPluginBigDataCommandHandler(IUnitOfWork unitOfWork, ICompressionService compression)
    {
        _unitOfWork = unitOfWork;
        _compression = compression;
    }

    public async Task Handle(PostPluginBigDataCommand command, CancellationToken ct)
    {
        var dto = command.PostPluginConfigurationRequestDto;

        var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(dto.JsonData);

        var compressed = _compression.Compress(jsonBytes);

        var entity = new PluginBigData(
            dto.UserCreater,
            dto.PluginName,
            dto.ConfigurationName,
            compressed);

        await _unitOfWork.Repository<PluginBigData>()
            .AddAsync(entity);

        await _unitOfWork.SaveAsync(ct);
    }
}