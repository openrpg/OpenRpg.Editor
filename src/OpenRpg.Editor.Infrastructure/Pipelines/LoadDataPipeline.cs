using System.Collections.Generic;
using Persistity.Core.Serialization;
using Persistity.Endpoints;
using Persistity.Flow.Builders;
using Persistity.Flow.Pipelines;
using Persistity.Flow.Steps.Types;

namespace OpenRpg.Editor.Infrastructure.Pipelines
{
    public class LoadDataPipeline<T> : FlowPipeline, ILoadDataPipeline<T>
    {
        public IDeserializer Deserializer { get; }
        public IReceiveDataEndpoint Endpoint { get; }

        public LoadDataPipeline(PipelineBuilder pipelineBuilder, IDeserializer deserializer, IReceiveDataEndpoint endpoint)
        {
            Deserializer = deserializer;
            Endpoint = endpoint;
            Steps = BuildSteps(pipelineBuilder);
        }
        
        protected IEnumerable<IPipelineStep> BuildSteps(PipelineBuilder builder)
        {
            return builder
                .StartFrom(Endpoint)
                .DeserializeWith<T>(Deserializer)
                .BuildSteps();
        }
    }
}