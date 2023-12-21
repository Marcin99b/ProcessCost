using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using ProcessCost.Domain.Models;

namespace ProcessCost.Domain;

public interface IStageEvent;
public record StageUpdatedMoneyEvent(Money OldMoney, Stage UpdatedStage) : IStageEvent;
public class StagesEventBus
{
    private readonly Channel<IStageEvent> _channel = Channel.CreateUnbounded<IStageEvent>();

    public ValueTask Publish(IStageEvent stageEvent) => this._channel.Writer.WriteAsync(stageEvent);
}
