﻿using kafka_stream_core.Nodes;
using kafka_stream_core.Nodes.Parameters;
using kafka_stream_core.SerDes;
using System;
using System.Collections.Generic;

namespace kafka_stream_core.Stream
{
    public class KStream<K,V>
    {
        private readonly string nameNode;
        private readonly ISerDes<K> keySerdes;
        private readonly ISerDes<V> valueSerdes;
        private readonly List<string> setSourceNodes;
        private readonly StreamGraphNode node;
        private readonly InternalStreamBuilder builder;

        internal KStream(string name, ISerDes<K> keySerdes, ISerDes<V> valueSerdes, List<string> setSourceNodes, StreamGraphNode node, InternalStreamBuilder builder)
        {
            nameNode = name;
            this.keySerdes = keySerdes;
            this.valueSerdes = valueSerdes;
            this.setSourceNodes = setSourceNodes;
            this.node = node;
            this.builder = builder;
        }

        public KStream<K, V> filter(Func<K, V, bool> predicate)
        {
            string name = this.builder.newProcessorName("KSTREAM-FILTER-");
            ProcessorParameters<KStreamFilter<K, V>, K, V> processorParameters = new ProcessorParameters<KStreamFilter<K, V>, K, V>(new KStreamFilter<K, V>(predicate), name);
            ProcessorGraphNode<KStreamFilter<K, V>, K, V> filterProcessorNode = new ProcessorGraphNode<KStreamFilter<K, V>, K, V>(name, processorParameters);
            this.builder.addGraphNode(node, filterProcessorNode);
            return new KStream<K, V>(name, this.keySerdes, this.valueSerdes, this.setSourceNodes, filterProcessorNode, this.builder);
        }

        public KStream<K, V> filterNot(Func<K, V, bool> predicate)
        {
            string name = this.builder.newProcessorName("KSTREAM-FILTER-");
            ProcessorParameters<KStreamFilter<K, V>, K, V> processorParameters = new ProcessorParameters<KStreamFilter<K, V>, K, V>(new KStreamFilter<K, V>(predicate, true), name);
            ProcessorGraphNode<KStreamFilter<K, V>, K, V> filterProcessorNode = new ProcessorGraphNode<KStreamFilter<K, V>, K, V>(name, processorParameters);
            this.builder.addGraphNode(node, filterProcessorNode);
            return new KStream<K, V>(name, this.keySerdes, this.valueSerdes, this.setSourceNodes, filterProcessorNode, this.builder);
        }

        public KStream<K1, V1> transform<K1,V1>(Func<K, V, KeyValuePair<K1, V1>> transformSupplier)
        {
            string name = this.builder.newProcessorName("KSTREAM-TRANSFORM-");
            ProcessorParameters<KStreamTransform<K, V, K1, V1>, K, V> processorParameters = new ProcessorParameters<KStreamTransform<K, V, K1, V1>, K, V>(new KStreamTransform<K, V, K1, V1>(transformSupplier), name);
            TransformKeyValueGraphNode<KStreamTransform<K, V, K1, V1>, K, V, K1, V1> transformProcessorNode = new TransformKeyValueGraphNode<KStreamTransform<K, V, K1, V1>, K, V, K1, V1>(name, processorParameters);
            this.builder.addGraphNode(node, transformProcessorNode);
            return new KStream<K1, V1>(name, null, null, this.setSourceNodes, transformProcessorNode, this.builder);
        }

        public void to(string topicName, Produced<K,V> produced)
        {
            string name = this.builder.newProcessorName("KSTREAM-SINK-");
            StreamSinkNode<K, V> sinkNode = new Nodes.StreamSinkNode<K,V>(topicName, name, produced);
            this.builder.addGraphNode(node, sinkNode);
        }

        public void to(string topicName)
        {
            string name = this.builder.newProcessorName("KSTREAM-SINK-");
            StreamSinkNode<string, string> sinkNode = new Nodes.StreamSinkNode<string, string>(topicName, name, Produced<string, string>.with(new StringSerDes(), new StringSerDes()));
            this.builder.addGraphNode(node, sinkNode);
        }
    }
}