using System.Text.Json;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using MediatR;

namespace BimManagerPortal.Application.Features.PluginBigDatas.Commands.SeedFakePluginBigData;

public record SeedFakePluginBigDataCommand(int Count = 20) : IRequest<int>;

internal class SeedFakePluginBigDataCommandHandler : IRequestHandler<SeedFakePluginBigDataCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompressionService _compression;

    private static readonly string[] PluginNames =
        ["RestrictedAreaPlugin", "ClashDetectionPlugin", "QuantityTakeoffPlugin", "ModelAuditPlugin", "CoordinationPlugin"];

    private static readonly string[] Configurations =
        ["Floor-1", "Floor-2", "Floor-3", "Zone-A", "Zone-B", "Zone-C", "Roof", "Basement", "Section-North", "Section-South"];

    private static readonly string[] Users =
        ["ivanov.a", "petrov.s", "sidorova.m", "kozlov.d", "novikova.e"];

    private static readonly string[] Statuses = ["OK", "Warning", "Error"];

    private static readonly string[] ElementTypes = ["Wall", "Floor", "Column", "Beam", "Door", "Window", "Stair", "Railing"];

    public SeedFakePluginBigDataCommandHandler(IUnitOfWork unitOfWork, ICompressionService compression)
    {
        _unitOfWork = unitOfWork;
        _compression = compression;
    }

    public async Task<int> Handle(SeedFakePluginBigDataCommand command, CancellationToken ct)
    {
        var count = Math.Clamp(command.Count, 1, 500);
        var rng = new Random();
        var repo = _unitOfWork.Repository<PluginBigData>();

        for (int i = 0; i < count; i++)
        {
            var pluginName = PluginNames[rng.Next(PluginNames.Length)];
            var configName = Configurations[rng.Next(Configurations.Length)];
            var user = Users[rng.Next(Users.Length)];

            var jsonData = BuildFakeJson(pluginName, configName, rng);
            var jsonBytes = JsonSerializer.SerializeToUtf8Bytes(jsonData);
            var compressed = _compression.Compress(jsonBytes);

            var entity = new PluginBigData(user, pluginName, configName, compressed);
            await repo.AddAsync(entity);
        }

        await _unitOfWork.SaveAsync(ct);
        return count;
    }

    private static object BuildFakeJson(string pluginName, string configName, Random rng)
    {
        var elementCount = rng.Next(10, 300);
        var issueCount = rng.Next(0, elementCount / 3 + 1);

        return pluginName switch
        {
            "RestrictedAreaPlugin" => new
            {
                configuration = configName,
                scannedElements = elementCount,
                violations = Enumerable.Range(0, issueCount).Select(_ => new
                {
                    elementId = $"E-{rng.Next(100000, 999999)}",
                    type = ElementTypes[rng.Next(ElementTypes.Length)],
                    area = Math.Round(rng.NextDouble() * 50 + 1, 2),
                    status = "Violation",
                    level = $"Level {rng.Next(1, 20)}"
                }).ToArray(),
                summary = new { total = elementCount, issues = issueCount, status = issueCount == 0 ? "OK" : "Warning" }
            },
            "ClashDetectionPlugin" => new
            {
                configuration = configName,
                clashGroups = rng.Next(1, 10),
                clashes = Enumerable.Range(0, issueCount).Select(_ => new
                {
                    id = $"CLH-{rng.Next(1000, 9999)}",
                    discipline1 = new[] { "Architecture", "Structure", "MEP" }[rng.Next(3)],
                    discipline2 = new[] { "Architecture", "Structure", "MEP" }[rng.Next(3)],
                    severity = new[] { "Critical", "Major", "Minor" }[rng.Next(3)],
                    point = new { x = Math.Round(rng.NextDouble() * 100, 3), y = Math.Round(rng.NextDouble() * 100, 3), z = Math.Round(rng.NextDouble() * 30, 3) }
                }).ToArray(),
                summary = new { total = elementCount, clashes = issueCount, status = Statuses[rng.Next(Statuses.Length)] }
            },
            "QuantityTakeoffPlugin" => new
            {
                configuration = configName,
                elements = Enumerable.Range(0, Math.Min(elementCount, 20)).Select(_ => new
                {
                    type = ElementTypes[rng.Next(ElementTypes.Length)],
                    count = rng.Next(1, 100),
                    totalArea = Math.Round(rng.NextDouble() * 500, 2),
                    totalVolume = Math.Round(rng.NextDouble() * 200, 2),
                    material = new[] { "Concrete", "Steel", "Wood", "Glass", "Brick" }[rng.Next(5)]
                }).ToArray(),
                totalCost = Math.Round(rng.NextDouble() * 10_000_000, 2),
                currency = "RUB"
            },
            "ModelAuditPlugin" => new
            {
                configuration = configName,
                checkedElements = elementCount,
                warnings = issueCount,
                errors = rng.Next(0, issueCount / 2 + 1),
                rules = Enumerable.Range(0, rng.Next(3, 8)).Select(i => new
                {
                    ruleId = $"RULE-{i + 1:D3}",
                    name = new[] { "No shared coordinates", "Missing category", "Unnamed element", "Duplicate Id", "Missing level" }[i % 5],
                    passed = rng.Next(0, 2) == 1
                }).ToArray(),
                status = Statuses[rng.Next(Statuses.Length)]
            },
            _ => new
            {
                configuration = configName,
                processedElements = elementCount,
                issues = issueCount,
                status = Statuses[rng.Next(Statuses.Length)],
                details = $"Coordination check completed for {configName}"
            }
        };
    }
}
