using System.Drawing.Imaging;

namespace SortingVisualization
{
    public sealed class GifWriter : IDisposable
    {
        private readonly BinaryWriter _writer;
        private readonly MemoryStream _memory;
        private readonly int _repeat;

        private int _frames;

        public GifWriter(string path, int repeat = 0) 
            : this(new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read), repeat)
        {
        }

        public GifWriter(Stream stream, int repeat = 0)
        {
            _writer = new BinaryWriter(stream);
            _memory = new MemoryStream();
            _frames = 0;
            _repeat = repeat;
        }

        /// <summary>
        /// Writes next frame
        /// </summary>
        /// <param name="image">Next frame</param>
        /// <param name="delay">Delay after this frame in milliseconds</param>
        public void WriteFrame(Image image, int delay = 100)
        {
            _memory.Position = 0;
            image.Save(_memory, ImageFormat.Gif);
            if (_frames == 0)
                InitHeader(image.Width, image.Height);

            WriteGraphicControlBlock(delay);
            WriteImageBlock(image.Width, image.Height);

            _frames++;
        }

        /// <summary>
        /// Saves GIF file
        /// </summary>
        public void Dispose()
        {
            _writer.Write((byte)0x3b); // File Trailer
            _memory.Dispose();
            _writer.BaseStream.Dispose();
            _writer.Dispose();
        }

        private void WriteImageBlock(int width, int height)
        {
            _memory.Position = 789;
            Span<byte> headerBuffer = stackalloc byte[11];
            _memory.Read(headerBuffer);
            _writer.Write(headerBuffer[0]);
            _writer.Write((short)0); // X pos
            _writer.Write((short)0); // Y pos
            _writer.Write((short)width); // Width
            _writer.Write((short)height); // Height

            if (_frames != 0)
            {
                _memory.Position = 10;
                _writer.Write((byte)(_memory.ReadByte() & 0x3f | 0x80)); // Local color
                WriteColorTable();
            }
            else
            {
                _writer.Write((byte)(headerBuffer[9] & 0x07 | 0x07)); // Global color
            }
            _writer.Write(headerBuffer[10]); // LZW Min code size

            _memory.Position = 789 + headerBuffer.Length;

            int dataLength = _memory.ReadByte();
            while (dataLength > 0)
            {
                _writer.Write((byte)dataLength);
                for (int i = 0; i < dataLength; i++)
                    _writer.Write((byte)_memory.ReadByte());
                dataLength = _memory.ReadByte();
            }
            _writer.Write((byte)0); // Terminator
        }

        private void WriteGraphicControlBlock(int delay)
        {
            _memory.Position = 781L; // Locating the source GCE
            Span<byte> blockheadBuffer = stackalloc byte[8];
            _memory.Read(blockheadBuffer);
            _writer.Write(unchecked((short)0xf921)); // ID
            _writer.Write((byte)0x04); // Block size
            _writer.Write((byte)(blockheadBuffer[3] & 0xf7 | 0x08)); // Settings disposal flag
            _writer.Write((short)(delay / 10)); // Frame delay
            _writer.Write(blockheadBuffer[6]); // Transparent color index
            _writer.Write((byte)0); // Terminator
        }

        private void InitHeader(int width, int height)
        {
            _writer.Write("GIF".ToCharArray()); // File type
            _writer.Write("89a".ToCharArray()); // File version
            _writer.Write((short)width);
            _writer.Write((short)height);
            _memory.Position = 10L;
            _writer.Write((byte)_memory.ReadByte()); // Global color table info
            _writer.Write((byte)0); // Background color index
            _writer.Write((byte)0); // Pixel aspect ratio
            WriteColorTable();
            WriteExtensionHeader();
        }

        private void WriteExtensionHeader()
        {
            _writer.Write(unchecked((short)0xff21)); // App Extension Block ID
            _writer.Write((byte)0x0b); // App block length
            _writer.Write("NETSCAPE2.0".ToCharArray()); // App ID
            _writer.Write((byte)3); // App block length
            _writer.Write((byte)1);
            _writer.Write((short)_repeat); // Repeat count
            _writer.Write((byte)0); // Terminator
        }

        private void WriteColorTable()
        {
            _memory.Position = 13;
            for (int i = 0; i < 768; i++)
                _writer.Write((byte)_memory.ReadByte());
        }
    }
}
