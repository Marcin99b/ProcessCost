using System.Threading.Channels;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain;

public interface IStageEvent;

public record StageUpdatedMoneyEvent(Money OldMoney, Stage UpdatedStage) : IStageEvent;

public class StagesEventBus
{
    private readonly Channel<IStageEvent> _channel = Channel.CreateUnbounded<IStageEvent>();

    public ValueTask Publish(IStageEvent stageEvent)
    {
        return this._channel.Writer.WriteAsync(stageEvent);
    }
}