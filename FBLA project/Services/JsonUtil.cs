using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace FBLA_project
{
    public class JsonUtil<T>
    {
        private T _jsonData;
        private string _fileName;
        public JsonUtil(string fileName)
        {
            _fileName = fileName;
            using (StreamReader jsonStream = new StreamReader(fileName))
            {
                string jsonString = jsonStream.ReadToEnd();
                _jsonData = JsonSerializer.Deserialize<T>(jsonString) ?? throw new Exception("Server Error");
            }
        }

        public T Access()
        {
            return _jsonData;
        }

        public void Write(T obj) { 
            _jsonData = obj;
        }

        public void Export()
        {
            using (StreamWriter File1 = new(_fileName))
            {
                string newJson = JsonSerializer.Serialize<T>(_jsonData, new JsonSerializerOptions() { WriteIndented = true });
                File1.Write(newJson);
            }
        }
    }
}

