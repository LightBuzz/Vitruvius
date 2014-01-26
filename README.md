Vitruvius
=========

Vitruvius is a set of easy-to-use Kinect utilities that will speed-up the development of your projects. Supports gesture detection, skeleton drawing, frame capturing, voice recognition and much more.

Great news for early-access developers: Vitruvius now supports Kinect for Windows version 2 sensor!

Features
---

*Skeletal extensions*
* Joint scaling, proper for on-screen display
* User height
* Distance between joints
* One-line skeleton tracking

*WPF & WinForms utilities*
* Easily display color, depth and infrared frames
* Save Kinect frames as bitmap images
* One-line skeleton drawing (WPF-only)

*Gestures (new)*
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

*Voice recognition & text-to-speech (v1 only)*
* Recognize voice commands
* Speech synthesis

*Coming very soon*
* Posture support (jumping, dancing, etc)

Prerequisites
---
* [Kinect for Windows](http://amzn.to/1k7rquZ) or [Kinect for XBOX](http://amzn.to/1dO0R0s) sensor
* [Kinect for Windows SDK v1.8](http://go.microsoft.com/fwlink/?LinkID=323588)

If you are developing using the developer preview Kinect SDK v2, you need the appropriate hardware and software provided by Microsoft.

Installation
---
You can install Vitruvius using **either** of the following methods:

* Easiest way: Create a new WPF project in Visual Studio and select

        Tools ➙ Library Package Manager ➙ Manage NuGet Packages for Solution

then search for Vitruvius and click Install.

* Install via NuGet command-line

        PM> Install-Package Vitruvius

* Download project's source code and build the solution.

Examples
---

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

Credits
---
* Developed by [Vangos Pterneas](http://pterneas.com) for [LightBuzz](http://lightbuzz.com)
* Gesture detection partly based on [Fizbin](https://github.com/EvilClosetMonkey/Fizbin.Kinect.Gestures) library, by [Nicholas Pappas](http://www.exceptontuesdays.com/)

License
---
You are free to use these libraries in personal and commercial projects by attributing the original creator of Vitruvius. Licensed under [Apache v2 License](https://github.com/LightBuzz/Vitruvius/blob/master/LICENSE).
