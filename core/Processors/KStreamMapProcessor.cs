﻿using kafka_stream_core.Stream;
using kafka_stream_core.Stream.Internal.Graph;
using System;
using System.Collections.Generic;

namespace kafka_stream_core.Processors
{
    internal class KStreamMapProcessor<K, V, K1, V1> : AbstractProcessor<K, V>
    {
        private IKeyValueMapper<K, V, KeyValuePair<K1, V1>> mapper;

        public KStreamMapProcessor(IKeyValueMapper<K, V, KeyValuePair<K1, V1>> mapper)
        {
            this.mapper = mapper;
        }

        public override object Clone()
        {
            var p= new KStreamMapProcessor<K, V, K1, V1>(this.mapper);
            p.StateStores = new List<string>(this.StateStores);
            return p;
        }

        public override void Process(K key, V value)
        {
            KeyValuePair<K1, V1> newPair = this.mapper.Apply(key, value);
            this.Forward(newPair.Key, newPair.Value);
        }
    }
}
