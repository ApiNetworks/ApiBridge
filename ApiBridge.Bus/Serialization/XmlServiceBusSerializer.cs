using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using ApiBridge.Bus.Interfaces;

namespace ApiBridge.Bus.Serialization
{
    public class XmlServiceBusSerializer : ServiceBusSerializerBase {

        MemoryStream serializedStream;

        /// <summary>
        /// Create an instance of the serializer
        /// </summary>
        /// <returns></returns>
        public override IServiceBusSerializer Create() {
            return new XmlServiceBusSerializer();
        }

        /// <summary>
        /// Serialize the message
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Stream Serialize(object obj) {
            var serial = new XmlSerializer(obj.GetType());
            serializedStream = new MemoryStream();

            using (var writer = XmlDictionaryWriter.CreateBinaryWriter(serializedStream, null, null, false)) {
                serial.Serialize(writer, obj);
            }

            serializedStream.Position = 0; //make sure you always set the stream position to where you want to serialize.
            return serializedStream;
        }

        /// <summary>
        /// Deserialize the message
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object Deserialize(Stream stream, Type type) {
            var serial = new XmlSerializer(type);
            using (var reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max)) {
                return serial.Deserialize(reader);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() {
            if (serializedStream != null) {
                serializedStream.Dispose();
                serializedStream = null;
            }
        }

        public override string SerializeJson(object obj)
        {
            throw new NotImplementedException();
        }

        public override object DeserializeJson(string jsonString, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
