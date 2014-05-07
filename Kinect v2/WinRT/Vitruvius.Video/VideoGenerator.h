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

namespace Vitruvius_Video
{
	public ref class VideoGenerator sealed
	{
		UINT32 videoWidth;
		UINT32 videoHeight;
		UINT32 fps;
		UINT32 bitRate;
		UINT32 frameSize;
		GUID   encodingFormat;
		GUID   inputFormat;

		DWORD  streamIndex;
		ComPtr<IMFSinkWriter> sinkWriter;

		bool   initiated;

		LONGLONG rtStart;
		UINT64 rtDuration;

	private:
		HRESULT InitializeSinkWriter(Windows::Storage::Streams::IRandomAccessStream^ stream);
		HRESULT WriteFrame(DWORD *videoFrameBuffer, const LONGLONG& rtStart, const LONGLONG& rtDuration);

	public:
		VideoGenerator(UINT32 width, UINT32 height, Windows::Storage::Streams::IRandomAccessStream^ stream, UINT32 delay);
		virtual ~VideoGenerator();

		void AppendNewFrame(const Array<byte> ^videoFrameBuffer);
		void Finalize();
	};
}
