﻿using kafka_stream_core.Crosscutting;
using kafka_stream_core.Processors.Internal;
using kafka_stream_core.SerDes;
using kafka_stream_core.State;
using kafka_stream_core.Stream;
using kafka_stream_core.Stream.Internal;
using kafka_stream_core.Table;

namespace kafka_stream_core
{
    public class StreamBuilder
    {
        private readonly Topology topology = new Topology();
        private readonly InternalTopologyBuilder internalTopologyBuilder;
        private readonly InternalStreamBuilder internalStreamBuilder;

        public StreamBuilder()
        {
            internalTopologyBuilder = topology.Builder;
            internalStreamBuilder = new InternalStreamBuilder(internalTopologyBuilder);
        }

        #region KStream

        public KStream<string, string> stream(string topic) => this.stream(topic, Consumed<string, string>.with(new StringSerDes(), new StringSerDes()));

        public KStream<K, V> stream<K,V>(string topic, Consumed<K, V> consumed)
        {
            return internalStreamBuilder.stream(topic, consumed);
        }

        #endregion

        #region KTable

        // TODO
        //public KTable<object, object> table(string topic) 
        //    => this.table<object, object>(topic, 
        //        Consumed<object, object>.with(null, null), 
        //        Materialized<object, object, KeyValueStore<Bytes, byte[]>>.with(default, default));

        public KTable<K,V> table<K,V>(string topic, Consumed<K,V> consumed, Materialized<K, V, KeyValueStore<Bytes, byte[]>> materialized = null)
        {
            materialized?.useProvider(internalStreamBuilder, $"{topic}-").initConsumed(consumed);

            return internalStreamBuilder.table(topic, consumed, materialized);
        }

        #endregion

        public Topology build()
        {
            this.internalStreamBuilder.build();
            return topology;
        }
    }
}
