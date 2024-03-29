﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ApiBridge.Bus.Interfaces;

namespace ApiBridge.Bus.Serialization
{
    /// <summary>
    /// Abstract base class that may be used as a base class for serializers
    /// </summary>
    public abstract class ServiceBusSerializerBase : IServiceBusSerializer {

        /// <summary>
        /// Create an instance of the serializer
        /// </summary>
        /// <returns></returns>
        public abstract IServiceBusSerializer Create();

        /// <summary>
        /// Serialize the message
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract Stream Serialize(object obj);


        /// <summary>
        /// Serialize the message
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract string SerializeJson(object obj);

        /// <summary>
        /// Deserialize the message
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract object Deserialize(Stream stream, Type type);

        /// <summary>
        /// Deserialize the message
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract object DeserializeJson(string jsonString, Type type);

        /// <summary>
        /// Deserialize the message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public T Deserialize<T>(Stream stream) {
            return (T)Deserialize(stream, typeof(T));
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public abstract void Dispose();
    }
}
