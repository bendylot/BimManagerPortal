using System.Text.Json;
using AutoMapper;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using MediatR;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Commands.PostPluginBigData;

public record PostPluginBigDataCommand(PostPluginBigDataRequestDto PostPluginConfigurationRequestDto) : IRequest;

internal class GetAllPlayersQueryHandler : IRequestHandler<PostPluginBigDataCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICompressionService _compression;
    public GetAllPlayersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICompressionService compression)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
            compressed);

        await _unitOfWork.Repository<PluginBigData>()
            .AddAsync(entity);

        await _unitOfWork.SaveAsync(ct);
    }
}