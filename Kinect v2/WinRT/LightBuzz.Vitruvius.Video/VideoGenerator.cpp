#include "pch.h"
#include "VideoGenerator.h"

#pragma comment(lib, "mfreadwrite")
#pragma comment(lib, "mfplat")
#pragma comment(lib, "mfuuid")

using namespace LightBuzz_Vitruvius_Video;

VideoGenerator::VideoGenerator(UINT32 width, UINT32 height, Windows::Storage::Streams::IRandomAccessStream^ stream, UINT32 fps, UINT32 delay)
{
	_videoWidth = width;
	_videoHeight = height;
	_fps = fps;
	_bitRate = 1000000;
	_frameSize = _videoWidth * _videoHeight;
	_encodingFormat = MFVideoFormat_WMV2;
	_inputFormat = MFVideoFormat_RGB32;

	HRESULT hr = CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
	if (SUCCEEDED(hr))
	{
		hr = MFStartup(MF_VERSION);
		if (SUCCEEDED(hr))
		{
			hr = InitializeSinkWriter(stream);
			if (SUCCEEDED(hr))
			{
				_initiated = true;
				_rtStart = 0;
				_rtDuration = (10000000 * delay) / 1000;
			}
		}
	}
}

VideoGenerator::~VideoGenerator()
{
	Finalize();
}

void VideoGenerator::SetBitRate(UINT32 bitRate)
{
	_bitRate = bitRate;
}

void VideoGenerator::Finalize()
{
	if (!_initiated)
		return;

	_initiated = false;
	_sinkWriter->Finalize();
	MFShutdown();
}

void VideoGenerator::AppendNewFrame(const Platform::Array<byte> ^videoFrameBuffer)
{
	auto length = videoFrameBuffer->Length / sizeof(DWORD);
	DWORD *buffer = (DWORD *)(videoFrameBuffer->Data);
	std::unique_ptr<DWORD[]> target(new DWORD[length]);

	for (UINT32 index = 0; index < length; index++)
	{
		DWORD color = buffer[index];
		BYTE b = (BYTE)((color & 0x00FF0000) >> 16);
		BYTE g = (BYTE)((color & 0x0000FF00) >> 8);
		BYTE r = (BYTE)((color & 0x000000FF));

#if ARM
		auto row = index / videoWidth;
		auto targetRow = videoHeight - row - 1;
		auto column = index - (row * videoWidth);
		target[(targetRow * videoWidth) + column] = (r << 16) + (g << 8) + b;
#else
		target[index] = (r << 16) + (g << 8) + b;
#endif
	}

	// Send frame to the sink writer.
	HRESULT hr = WriteFrame(target.get(), _rtStart, _rtDuration);
	if (FAILED(hr))
	{
		throw Platform::Exception::CreateException(hr);
	}
	_rtStart += _rtDuration;
}

HRESULT VideoGenerator::InitializeSinkWriter(Windows::Storage::Streams::IRandomAccessStream^ stream)
{
	ComPtr<IMFAttributes> spAttr;
	ComPtr<IMFMediaType>  mediaTypeOut;
	ComPtr<IMFMediaType>  mediaTypeIn;
	ComPtr<IMFByteStream> spByteStream;
	HRESULT hr = MFCreateMFByteStreamOnStreamEx((IUnknown*)stream, &spByteStream);

	if (SUCCEEDED(hr))
	{
		MFCreateAttributes(&spAttr, 10);
		spAttr->SetUINT32(MF_READWRITE_ENABLE_HARDWARE_TRANSFORMS, true);

		hr = MFCreateSinkWriterFromURL(L".wmv", spByteStream.Get(), spAttr.Get(), &_sinkWriter);
	}

	// Set the output media type.
	if (SUCCEEDED(hr))
	{
		hr = MFCreateMediaType(&mediaTypeOut);
	}
	if (SUCCEEDED(hr))
	{
		hr = mediaTypeOut->SetGUID(MF_MT_MAJOR_TYPE, MFMediaType_Video);
	}
	if (SUCCEEDED(hr))
	{
		hr = mediaTypeOut->SetGUID(MF_MT_SUBTYPE, _encodingFormat);
	}
	if (SUCCEEDED(hr))
	{
		hr = mediaTypeOut->SetUINT32(MF_MT_AVG_BITRATE, _bitRate);
	}
	if (SUCCEEDED(hr))
	{
		hr = mediaTypeOut->SetUINT32(MF_MT_INTERLACE_MODE, MFVideoInterlace_Progressive);
	}
	if (SUCCEEDED(hr))
	{
		hr = MFSetAttributeSize(mediaTypeOut.Get(), MF_MT_FRAME_SIZE, _videoWidth, _videoHeight);
	}
	if (SUCCEEDED(hr))
	{
		hr = MFSetAttributeRatio(mediaTypeOut.Get(), MF_MT_FRAME_RATE, _fps, 1);
	}
	if (SUCCEEDED(hr))
	{
		hr = MFSetAttributeRatio(mediaTypeOut.Get(), MF_MT_PIXEL_ASPECT_RATIO, 1, 1);
	}
	if (SUCCEEDED(hr))
	{
		hr = _sinkWriter->AddStream(mediaTypeOut.Get(), &_streamIndex);
	}

	// Set the input media type.
	if (SUCCEEDED(hr))
	{
		hr = MFCreateMediaType(&mediaTypeIn);
	}
	if (SUCCEEDED(hr))
	{
		hr = mediaTypeIn->SetGUID(MF_MT_MAJOR_TYPE, MFMediaType_Video);
	}
	if (SUCCEEDED(hr))
	{
		hr = mediaTypeIn->SetGUID(MF_MT_SUBTYPE, _inputFormat);
	}
	if (SUCCEEDED(hr))
	{
		hr = mediaTypeIn->SetUINT32(MF_MT_INTERLACE_MODE, MFVideoInterlace_Progressive);
	}
	if (SUCCEEDED(hr))
	{
		hr = MFSetAttributeSize(mediaTypeIn.Get(), MF_MT_FRAME_SIZE, _videoWidth, _videoHeight);
	}
	if (SUCCEEDED(hr))
	{
		hr = MFSetAttributeRatio(mediaTypeIn.Get(), MF_MT_FRAME_RATE, _fps, 1);
	}
	if (SUCCEEDED(hr))
	{
		hr = MFSetAttributeRatio(mediaTypeIn.Get(), MF_MT_PIXEL_ASPECT_RATIO, 1, 1);
	}
	if (SUCCEEDED(hr))
	{
		hr = _sinkWriter->SetInputMediaType(_streamIndex, mediaTypeIn.Get(), NULL);
	}

	// Tell the sink writer to start accepting data.
	if (SUCCEEDED(hr))
	{
		hr = _sinkWriter->BeginWriting();
	}

	return hr;
}

HRESULT VideoGenerator::WriteFrame(
	DWORD *videoFrameBuffer,
	const LONGLONG& rtStart,        // Time stamp.
	const LONGLONG& rtDuration      // Frame duration.
	)
{
	ComPtr<IMFSample> sample;
	ComPtr<IMFMediaBuffer> buffer;

	const LONG cbWidth = 4 * _videoWidth;
	const DWORD cbBuffer = cbWidth * _videoHeight;

	BYTE *pData = NULL;

	// Create a new memory buffer.
	HRESULT hr = MFCreateMemoryBuffer(cbBuffer, &buffer);

	// Lock the buffer and copy the video frame to the buffer.
	if (SUCCEEDED(hr))
	{
		hr = buffer->Lock(&pData, NULL, NULL);
	}
	if (SUCCEEDED(hr))
	{
		hr = MFCopyImage(
			pData,                      // Destination buffer.
			cbWidth,                    // Destination stride.
			(BYTE*)videoFrameBuffer,    // First row in source image.
			cbWidth,                    // Source stride.
			cbWidth,                    // Image width in bytes.
			_videoHeight                // Image height in pixels.
			);
	}
	if (buffer.Get())
	{
		buffer->Unlock();
	}

	// Set the data length of the buffer.
	if (SUCCEEDED(hr))
	{
		hr = buffer->SetCurrentLength(cbBuffer);
	}

	// Create a media sample and add the buffer to the sample.
	if (SUCCEEDED(hr))
	{
		hr = MFCreateSample(&sample);
	}
	if (SUCCEEDED(hr))
	{
		hr = sample->AddBuffer(buffer.Get());
	}

	// Set the time stamp and the duration.
	if (SUCCEEDED(hr))
	{
		hr = sample->SetSampleTime(rtStart);
	}
	if (SUCCEEDED(hr))
	{
		hr = sample->SetSampleDuration(rtDuration);
	}

	// Send the sample to the Sink Writer.
	if (SUCCEEDED(hr))
	{
		hr = _sinkWriter->WriteSample(_streamIndex, sample.Get());
	}

	return hr;
}

