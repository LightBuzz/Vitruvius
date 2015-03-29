using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius.Recording
{
    public abstract class BaseStreamReader<T>
    {
        public string Folder { get; set; }

        public ObservableCollection<T> Frames { get; set; }
    }

    public class DepthStreamReader : BaseStreamReader<DepthFrame>
    {
        public DepthStreamReader()
        {
        }

        public DepthStreamReader(StorageFolder folder)
        {
        }
    }

    public abstract class BaseStreamWriter<T>
    {
        protected string _prefix;

        protected int _index = 0;

        public StorageFolder Folder { get; set; }

        public IBuffer Buffer { get; set; }

        public virtual void AddFrame(T frame)
        {
            if (frame == null)
            {
                throw new Exception("Frame parameter is null.");
            }

            if (Folder == null)
            {
                throw new Exception("Folder property is null.");
            }
        }

        protected async void SaveFrame()
        {
            StorageFile file = await Folder.CreateFileAsync(string.Format("{0}-{1}.bin", _prefix, _index), CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteBufferAsync(file, Buffer);

            _index++;
        }
    }

    public class ColorStreamWriter : BaseStreamWriter<ColorFrame>
    {
        public ColorStreamWriter()
        {
            _prefix = "color";
        }

        public ColorStreamWriter(StorageFolder folder)
            : this()
        {
            Folder = folder;
        }

        public override void AddFrame(ColorFrame frame)
        {
            base.AddFrame(frame);

            if (Buffer == null)
            {
                Buffer = new Windows.Storage.Streams.Buffer(frame.FrameDescription.LengthInPixels);
            }

            frame.CopyRawFrameDataToBuffer(Buffer);

            SaveFrame();
        }
    }

    public class DepthStreamWriter : BaseStreamWriter<DepthFrame>
    {
        public DepthStreamWriter()
        {
            _prefix = "depth";
        }

        public DepthStreamWriter(StorageFolder folder)
            : this()
        {
            Folder = folder;
        }

        public override void AddFrame(DepthFrame frame)
        {
            base.AddFrame(frame);

            if (Buffer == null)
            {
                Buffer = new Windows.Storage.Streams.Buffer(frame.FrameDescription.LengthInPixels);
            }

            frame.CopyFrameDataToBuffer(Buffer);

            SaveFrame();
        }
    }

    public class InfraredStreamWriter : BaseStreamWriter<InfraredFrame>
    {
        public InfraredStreamWriter()
        {
            _prefix = "infrared";
        }

        public InfraredStreamWriter(StorageFolder folder)
            : this()
        {
            Folder = folder;
        }

        public override void AddFrame(InfraredFrame frame)
        {
            base.AddFrame(frame);

            if (Buffer == null)
            {
                Buffer = new Windows.Storage.Streams.Buffer(frame.FrameDescription.LengthInPixels);
            }

            frame.CopyFrameDataToBuffer(Buffer);

            SaveFrame();
        }
    }
}
