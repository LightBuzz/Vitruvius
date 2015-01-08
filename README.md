![Vitruvius](https://raw.githubusercontent.com/LightBuzz/Vitruvius/master/LOGO.png "Vitruvius")

Vitruvius is a set of easy-to-use Kinect utilities that will speed-up the development of your projects. Supports gesture detection, skeleton drawing, frame capturing, voice recognition and much more.

Great news for early-access developers: Vitruvius supports Kinect for Windows version 2 sensor!

NEW: Vitruvius now supports WinRT for Windows Store apps.

## Features

### Body extensions (universal)
* Joint scaling, proper for on-screen display
* User height
* Distance between joints
* One-line body tracking
* Angle calculations

### WinRT, WPF, & WinForms extensions
* Project points on screen
* Easily display color, depth and infrared frames
* Save Kinect frames as bitmap images
* One-line skeleton drawing
* Record color, depth and infrared streams and save into video files (WinRT only)

### NUI controls (universal)
* Kinect Smart Viewer
* Kinect Angle

### Gestures (universal)
* WaveLeft
* WaveRight
* SwipeLeft
* SwipeRight
* JoinedHands
* SwipeUp
* SwipeDown
* ZoomIn
* ZoomOut
* Menu

### Voice recognition & text-to-speech (v1 only)
* Recognize voice commands
* Speech synthesis

## Prerequisites
Kinect version 2
* [Kinect for Windows v2 sensor](http://amzn.to/1DQtBSV) or [Kinect for XBOX sensor](http://amzn.to/1AvdswC) with an [adapter](http://amzn.to/1wPJG55)
* [Kinect for Windows SDK v2](http://www.microsoft.com/en-us/download/details.aspx?id=44561)
Kinect version 1
* [Kinect for Windows sensor](http://amzn.to/1k7rquZ) or [Kinect for XBOX](http://amzn.to/1dO0R0s) sensor
* [Kinect for Windows SDK v1.8](http://go.microsoft.com/fwlink/?LinkID=323588)

## Installation
* Download project's source code and build the solution that matches the version of your sensor. NuGet packages will be available soon.

## Examples

1. Displaying Kinect color frames:

        void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenColorImageFrame())
            {
                if (frame != null)
                {
                    // Display on screen
                    image.Source = frame.ToBitmap();
                    
                    // Save the JPEG file
                    frame.Save("C:\\ColorFrame.jpg");
                }
            }
        }
        
2. Displaying Kinect depth frames:

        void Sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenDepthImageFrame())
            {
                if (frame != null)
                {
                    // Display on screen
                    image.Source = frame.ToBitmap();
                    
                    // Save the JPEG file
                    frame.Save("C:\\DepthFrame.jpg");
                }
            }
        }
        
3. Getting the height of a body:

        void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    canvas.ClearSkeletons();
                    
                    var skeletons = frame.Skeletons().Where(s => s.TrackingState == SkeletonTrackingState.Tracked);
                    
                    foreach (var skeleton in skeletons)
                    {
                        if (skeleton != null)
                        {
                            // Get the skeleton height.
                            double height = skeleton.Height();
                        }
                    }
                }
            }
        }

4. Detecting gestures:

        GestureController gestureController = new GestureController();
        gestureController.GestureRecognized += GestureController_GestureRecognized;
        
        // ...
        
        void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    var skeletons = frame.Skeletons().Where(s => s.TrackingState == SkeletonTrackingState.Tracked);
                    
                    foreach (var skeleton in skeletons)
                    {
                        if (skeleton != null)
                        {
                            // Update skeleton gestures.
                            gestureController.Update(skeleton);
                        }
                    }
                }
            }
        }
        
        // ...
        
        void GestureController_GestureRecognized(object sender, GestureEventArgs e)
        {
            // Display the recognized gesture's name.
            Debug.WriteLine(e.GestureType);
        }

5. Recognizing and synthesizing voice:

        VoiceController voiceController = new VoiceController();
        voiceController.SpeechRecognized += VoiceController_SpeechRecognized;
        
        KinectSensor sensor = SensorExtensions.Default();
        List<string> phrases = new List<string> { "Hello", "Goodbye" };
        
        voiceController.StartRecognition(sensor, phrases);
        
        // ...
        
        void VoiceController_SpeechRecognized(object sender, Microsoft.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            string text = e.Result.Text;
            
            voiceController.Speak("I recognized the words: " + text);
        }

## Contributors
* [Vangos Pterneas](http://pterneas.com) from [LightBuzz](http://lightbuzz.com)
* [George Karakatsiotis](http://gkarak.com) from [LightBuzz](http://lightbuzz.com)
* George Georgopoulos from [LightBuzz](http://lightbuzz.com)
* Gesture detection partly based on [Fizbin](https://github.com/EvilClosetMonkey/Fizbin.Kinect.Gestures) library, by [Nicholas Pappas](http://www.exceptontuesdays.com/)

## License
You are free to use these libraries in personal and commercial projects by attributing the original creator of Vitruvius. Licensed under [Apache v2 License](https://github.com/LightBuzz/Vitruvius/blob/master/LICENSE).
