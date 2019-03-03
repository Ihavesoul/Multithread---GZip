## Multithread-GZIP

This is test task for Veeam Software

## Description
To write a console program in C #, designed for block-by-block compression and decompression of files using System.IO.Compression.GzipStream.

For compression, the source file is divided into blocks of the same size, for example, 1 megabyte. Each block is compressed and written to the output file, regardless of the other blocks.

The program should effectively parallelize and synchronize block processing in a multiprocessor environment and be able to handle files that are larger than the amount of available RAM.
In case of exceptional situations, it is necessary to inform the user with a clear message that allows the user to correct the problem, in particular if the problems are related to the limitations of the operating system.
When working with threads, it is allowed to use only standard classes and libraries from .Net 3.5 (excluding ThreadPool, BackgroundWorker, TPL). Expected to be implemented using Threads.
The program code must comply with the principles of OOP and OOD (readability, division into classes, etc.).

## Commands
`GZipTest.exe compress / decompress [source file name] [result file name]`

## Task Vision
Was used Producer/Consumer pattern.
Producers:
- **AbstractArchiver**
- **Compress**
- **Decompress**
Consumers:
- **ReadConsumer**
- **WriteConsumer**
For synchronization threads was used **AutoResetEvent** and **Monitor**

## Algorithm:

**Initialize the stream to read:**
The file is broken into chunks, the chunks are in order, the one who has time is put in a queue,

**Flow is compressed:**
They are pulled out of the queue, archived, and then added to the dictionary, where the type is [chunk number, date array]

**Stream to write to file:**
We take out the key elements and put in the stream record.
Restoring the sequence roughly.

Use log4net for logging exceptions.

## Problems
- Choose pattern *(solved)*
- Load with a usefull CPU on 100% (bottle neck in HDD)
- How to get OutOfMemory? 
- For a long time could not understand how to remove the 4 gb memory limit *(solved)*
- Sync the threads *(solved)*
- Handle Exceptions *(solved)*
- Unit tests (try implement Unit Testing for evade file init but has some troubles with that *trouble : Infinity Loop in Unit Test*) *(rly need that?)*

## Better algorithms
https://github.com/icsharpcode/SharpZipLib/tree/master/src/ICSharpCode.SharpZipLib/GZip - this Repo don't have bottle neck on HDD and works fine. But that is very hard : D
