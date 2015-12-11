using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace Easy.Public
{
    public static class ObjectSerializationHelper
    {
        public static Byte[] Serialize<T>(T @object)
        {
            Byte[] bytes = new Byte[0];
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter f = new BinaryFormatter();
                f.Serialize(ms, @object);
                ms.Position = 0;
                BinaryReader bs = new BinaryReader(ms);
                bytes = bs.ReadBytes((Int32)ms.Length);
                bs.Close();
            }
            return bytes;
        }
        public static T Deserialize<T>(Byte[] bytes)
        {
            T result = default(T);
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter bWriter = new BinaryWriter(ms);
                bWriter.Write(bytes);
                ms.Position = 0;
                BinaryFormatter f = new BinaryFormatter();
                result = (T)f.Deserialize(ms);
                bWriter.Close();
            }
            return result;
        }
    }
}
