![Vitruvius](https://raw.githubusercontent.com/LightBuzz/Vitruvius/master/Design/vitruvius_logo.png "Vitruvius")

Vitruvius is a set of easy-to-use Kinect utilities that will speed-up the development of your projects. Supports gesture detection, skeleton drawing, frame capturing, voice recognition and much more.

Great news for early-access developers: Vitruvius supports Kinect for Windows version 2 sensor!

NEW: Vitruvius now supports WinRT for Windows Store apps.

## Features

### Body extensions (universal)
* Joint scaling, proper for on-screen display
* User height
* Distance between joints
* One-line body tracking

### WPF, WinForms & WinRT utilities
* Project points on screen
* Easily display color, depth and infrared frames
* Save Kinect frames as bitmap images
* One-line skeleton drawing
* Record color, depth and infrared streams and save into video files (WinRT only)

### NUI controls (universal)
* Kinect hover button
* Kinect cursor
* Kinect smart viewer

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

### Coming very soon
* Complete finger tracking
* Posture support (jumping, dancing, etc)

## Prerequisites
* [Kinect for Windows](http://amzn.to/1k7rquZ) or [Kinect for XBOX](http://amzn.to/1dO0R0s) sensor
* [Kinect for Windows SDK v1.8](http://go.microsoft.com/fwlink/?LinkID=323588)

If you are developing using the developer preview Kinect SDK v2, you need the appropriate hardware and software provided by Microsoft.

## Installation
* Download project's source code and build the solution that matches the version of your sensor. Version 2 refers to the private Developer Preview Kinect for Windows sensor.

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
                    
                    // Capture JPEG file
                    frame.Capture("C:\\ColorFrame.jpg");
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
                    
                    // Capture JPEG file
                    frame.Capture("C:\\DepthFrame.jpg");
                }
            }
        }
        
3. Drawing a skeleton and getting its height:

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
                            // Draw the skeleton.
                            canvas.DrawSkeleton(skeleton);
                                
                            // Get the skeleton height.
                            double height = skeleton.Height();
                        }
                    }
                }
            }
        }

4. Detecting gestures:

        GestureController gestureController = new GestureController(GestureType.All);
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
            Debug.WriteLine(e.Name);
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

## Support Vitruvius
Do you use Vitruvius in your projects? Do you find it helpful? [Buy us a beer](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=N5ELYBTYB3AYE)!
