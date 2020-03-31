﻿using kafka_stream_core.SerDes;
using System;
using System.Collections.Generic;
using System.Text;

namespace kafka_stream_core.Crosscutting
{
    public static class StringExtensions
    {
        public static ISerDes CreateSerDes(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var type = Type.GetType(value, false);
                return type != null ? Activator.CreateInstance(type) as ISerDes : null;
            }
            else
                return null;
        }
    }
}
