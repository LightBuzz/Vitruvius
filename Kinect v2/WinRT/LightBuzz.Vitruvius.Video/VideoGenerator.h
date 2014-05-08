#pragma once
#include <Windows.h>
#include <mfapi.h>
#include <mfidl.h>
#include <Mfreadwrite.h>
#include <mferror.h>
#include <wrl\client.h>
#include <memory>

using namespace Platform;
using namespace Microsoft::WRL;

namespace LightBuzz_Vitruvius_Video
{
	public ref class VideoGenerator sealed
	{
		UINT32 _videoWidth;
		UINT32 _videoHeight;
		UINT32 _fps;
		UINT32 _bitRate;
		UINT32 _frameSize;
		GUID   _encodingFormat;
		GUID   _inputFormat;

		DWORD  _streamIndex;
		ComPtr<IMFSinkWriter> _sinkWriter;

		bool   _initiated;

		LONGLONG _rtStart;
		UINT64 _rtDuration;

	private:
		HRESULT InitializeSinkWriter(Windows::Storage::Streams::IRandomAccessStream^ stream);
		HRESULT WriteFrame(DWORD *videoFrameBuffer, const LONGLONG& rtStart, const LONGLONG& rtDuration);

	public:
		VideoGenerator(UINT32 width, UINT32 height, Windows::Storage::Streams::IRandomAccessStream^ stream, UINT32 fps, UINT32 delay);
		virtual ~VideoGenerator();

		void AppendNewFrame(const Array<byte> ^videoFrameBuffer);
		void SetBitRate(UINT32 bitRate);
		void Finalize();
	};
}
