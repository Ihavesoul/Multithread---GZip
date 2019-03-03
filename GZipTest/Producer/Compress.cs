using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archiver.Const;
using GZipTest.Helpers;
using GZipTest.Models;

namespace GZipTest.Archiver
{
    public class Compress : AbstractArchiver
    {
        public Compress(Stream inputFileStream, Stream outputFileStream) : base(inputFileStream, outputFileStream)
        {
        }


        protected override void ReadInFile()
        {
            try
            {
                var fileSize = InputFileStream.Length;
                using (var binaryReader = new BinaryReader(InputFileStream))
                {
                    var chunkId = 0;
                    while (fileSize > 0 && HasError == null)
                    {
                        var currentChunkSize = fileSize > Const.ChunkSize ? Const.ChunkSize : fileSize;
                        var bytes = binaryReader.ReadBytes((int) currentChunkSize);
                        InputQueue.Enqueue(new Chunk(chunkId++, bytes));
                        fileSize -= currentChunkSize;
                        if (fileSize == 0)
                        {
                            InputQueue.ReadComplete();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SingleLogger.log.Error($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                SingleLogger.log.Debug($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                HasError = e;
            }
        }

        protected override void Process(int processEventId)
        {
            try
            {
                while (InputQueue.Dequeue(out Chunk chunk) && HasError == null)
                {
                    var compressChunk = GZip.GZip.CompressByBlocks(chunk.Bytes);
                    if(compressChunk == null) throw new OutOfMemoryException();
                    OutputDictionary.Add(chunk.Id, compressChunk);
                }
                ProcessEvents[processEventId].Set();
            }
            catch (Exception e)
            {
                SingleLogger.log.Error($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                SingleLogger.log.Debug($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
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
                        binaryWriter.Write(data.Length);
                        binaryWriter.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception e)
            {
                SingleLogger.log.Error($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                SingleLogger.log.Debug($"(Your exception is :{e.Message} , send a log file for us from the  folder /bin/Debug/myapp.log");
                HasError = e;
            }
        }
    }
}
