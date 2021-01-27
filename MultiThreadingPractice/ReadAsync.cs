using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingPractice
{
    static class ReadAsync
    {
        const int ITERS = 100;
        public static void Run()
        {
            byte[] data = new byte[0x10000000];
            while (true)
            {
                TrackGcs(new MemStream1(data));
                TrackGcs(new MemStream2(data));
                Console.ReadLine();
            }
        }
        static void TrackGcs(Stream input)
        {
            int gen0 = GC.CollectionCount(0), gen1 = GC.CollectionCount(1), gen2 = GC.CollectionCount(2);
            for (int i = 0; i < ITERS; i++)
            {
                input.Position = 0;
                input.CopyToAsync(Stream.Null).Wait();
            }
            int newGen0 = GC.CollectionCount(0), newGen1 = GC.CollectionCount(1), newGen2 = GC.CollectionCount(2);
            Console.WriteLine($"{input.GetType().Name}\tGen0:{newGen0 - gen0}   Gen1:{newGen1 - gen1}   Gen2:{newGen2 - gen2}");
        }
        class MemStream1 : MemoryStream
        {
            public MemStream1(byte[] data) : base(data) { }
            public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Read(buffer, offset, count);
            }
        }
        class MemStream2 : MemoryStream
        {
            public MemStream2(byte[] data) : base(data) { }
            private Task<int> m_lastTask;
            public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    var tcs = new TaskCompletionSource<int>();
                    tcs.SetCanceled();
                    return tcs.Task;
                }

                try
                {
                    int numRead = Read(buffer, offset, count);
                    if (m_lastTask != null && m_lastTask.Result == numRead) return m_lastTask;
                    m_lastTask = Task.FromResult(numRead);
                    return m_lastTask;
                }
                catch (Exception ex)
                {
                    var tcs = new TaskCompletionSource<int>();
                    tcs.SetException(ex);
                    return tcs.Task;
                }
            }
        }
    }
}
