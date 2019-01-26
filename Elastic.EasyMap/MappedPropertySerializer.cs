using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Newtonsoft.Json;

namespace Elastic.EasyMap
{
    public class MappedPropertySerializer : IElasticsearchSerializer
    {
        private readonly JsonSerializer _serializer;

        public MappedPropertySerializer()
        {
            _serializer = new JsonSerializer
            {
                ContractResolver = new MappedPropertyResolver(),
                DefaultValueHandling = DefaultValueHandling.Populate
            };
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return _serializer.Deserialize<T>(jsonReader);
                }
            }
        }

        public object Deserialize(Type type, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return _serializer.Deserialize(reader, type);
            }
        }

        public Task<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => Deserialize<T>(stream), cancellationToken);
        }


        public Task<object> DeserializeAsync(Type type, Stream stream, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => Deserialize(type, stream), cancellationToken);
        }


        public void Serialize<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.Indented)
        {
            using (var writer = new StreamWriter(stream))
            {
                _serializer.Serialize(writer, data);
            }
        }


        public Task SerializeAsync<T>(T data, Stream stream, SerializationFormatting formatting = SerializationFormatting.Indented,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.Run(() => Serialize(data, stream, formatting), cancellationToken);
        }

    }
}