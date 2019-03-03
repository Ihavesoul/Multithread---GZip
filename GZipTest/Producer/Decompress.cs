using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GZipTest.Models;
using GZipTest.GZip;
using GZipTest.Helpers;

namespace GZipTest.Archiver
{
    class Decompress : AbstractArchiver
    {
        public Decompress(Stream inputFileStream, Stream outputFileStream) : base(inputFileStream, outputFileStream)
        {
        }


        protected override void ReadInFile()
        {
            try
            {
                using (var binaryReader = new BinaryReader(InputFileStream))
                {
                    var fileSize = InputFileStream.Length;

                    const int intSize = 4;
                    var chunkId = 0;

                    while (fileSize > 0 && HasError == null)
                    {
                        var chunkSize = binaryReader.ReadInt32();
                        var bytes = binaryReader.ReadBytes(chunkSize);
                        InputQueue.Enqueue(new Chunk(chunkId++, bytes));
                        fileSize -= (chunkSize + intSize);
                        if (fileSize == 0)
                        {
                            InputQueue.ReadComplete();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SingleLogger.log.Debug($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                SingleLogger.log.Error($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                HasError = e;
            }
        }

        protected override void WriteOutFile()
        {
            try
            {
                using (var binaryWriter = new BinaryWriter(OutputFileStream))
                {
                    while (OutputDictionary.GetValueByKey(out var data) && HasError == null)
                    {
                        binaryWriter.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception e)
            {
                SingleLogger.log.Debug($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                SingleLogger.log.Error($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                HasError = e;
            }
        }

        protected override void Process(int processEventId)
        {   
            try
            {
                while (InputQueue.Dequeue(out Chunk chunk) && HasError == null)
                {
                    var decompressedChunkData = GZip.GZip.DecompressByBlocks(chunk.Bytes);
                    if (decompressedChunkData == null) throw new OutOfMemoryException();
                    OutputDictionary.Add(chunk.Id, decompressedChunkData);
                }
                ProcessEvents[processEventId].Set();
            }
            catch (Exception e)
            {
                SingleLogger.log.Debug($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                SingleLogger.log.Error($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                HasError = e;
            }
        }
    }
}
