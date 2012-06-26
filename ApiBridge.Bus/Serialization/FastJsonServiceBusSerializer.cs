using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ApiBridge.Bus.Interfaces;
using System.IO;

namespace ApiBridge.Bus.Serialization
{
    public class FastJsonServiceBusSerializer : ServiceBusSerializerBase
    {
        Stream serializedStream;

        public override IServiceBusSerializer Create()
        {
            return new FastJsonServiceBusSerializer();
        }

        public override Stream Serialize(object obj)
        {
            serializedStream = new MemoryStream();
            StreamWriter sw = new StreamWriter(serializedStream);
            
            //do not wrap in using, we don't want to close the stream
            sw.Write(SerializeJson(obj));
            sw.Flush();
            serializedStream.Position = 0; //make sure you always set the stream position to where you want to serialize.
            return serializedStream;
        }

        public override object Deserialize(Stream stream, Type type)
        {
            TextReader streamReader = new StreamReader(stream);
            string myJson = streamReader.ReadToEnd();
            return DeserializeJson(myJson, type);
        }

        public override void Dispose()
        {
            if (serializedStream != null)
            {
                serializedStream.Dispose();
                serializedStream = null;
            }
        }

        public override string SerializeJson(object obj)
        {
            try
            {
                return JSON.Instance.ToJSON(obj, false, false, false, false);
            }
            catch(Exception ex)
            {
                return String.Empty;
            }
        }

        public override object DeserializeJson(string jsonString, Type type)
        {
            try
            {
                return JSON.Instance.ToObject(jsonString, type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
