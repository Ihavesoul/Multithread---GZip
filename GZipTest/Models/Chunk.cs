using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipTest.Models
{
    public class Chunk
    {
        public Chunk()
        {
        }

        public Chunk(int id, byte[] bytes)
        {
            Id = id;
            Bytes = bytes;
        }

        public int Id { get; set; }

        public byte[] Bytes { get; set; }
    }
}
