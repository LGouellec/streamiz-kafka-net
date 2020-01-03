﻿using kafka_stream_core.Nodes.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace kafka_stream_core.Nodes
{
    internal class StreamSourceNode<K, V> : StreamGraphNode
    {
        private string topicName;
        private Consumed<K, V> consumed;

        public StreamSourceNode(string topicName, string streamGraphNode, Consumed<K,V> consumed) 
            : base(streamGraphNode)
        {
            this.topicName = topicName;
            this.consumed = consumed;
        }

        public override void writeToTopology(InternalTopologyBuilder builder)
        {
            builder.setSourceOperator(topicName, this.streamGraphNode, consumed);
        }
    }
}
