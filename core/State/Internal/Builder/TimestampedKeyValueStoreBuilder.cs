﻿using System;
using System.Collections.Generic;
using System.Text;
using kafka_stream_core.SerDes;
using kafka_stream_core.State.Supplier;

namespace kafka_stream_core.State.Internal.Builder
{
    internal class TimestampedKeyValueStoreBuilder<K, V>
        : AbstractStoreBuilder<K, ValueAndTimestamp<V>, TimestampedKeyValueStore<K, V>>
    {
        private readonly KeyValueBytesStoreSupplier storeSupplier;

        public TimestampedKeyValueStoreBuilder(KeyValueBytesStoreSupplier supplier, ISerDes<K> keySerde, ISerDes<V> valueSerde, DateTime time) :
            base(supplier.Name, keySerde, new ValueAndTimestampSerDes<V>(valueSerde), time)
        {
            this.storeSupplier = supplier;
        }

        public override TimestampedKeyValueStore<K, V> build()
        {
            // TODO : 
            return null;
        }
    }
}