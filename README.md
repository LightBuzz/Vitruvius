![Vitruvius](https://raw.githubusercontent.com/LightBuzz/Vitruvius/master/LOGO.png "Vitruvius")

Vitruvius is a set of easy-to-use Kinect utilities that will speed-up the development of your projects. Supports WPF and Windows Store. Requires Kinect 2.

## Installation
Grab the package using NuGet

        PM> Install-Package lightbuzz-vitruvius

... and import Vitruvius in your project

        using LightBuzz.Vitruvius;

*You'll also need to set the project configuration to Release and the target processor to x64 (preferred) or x86.*

## [Academic & Premium Versions](http://vitruviuskinect.com)
[vitruviuskinect.com](http://vitruviuskinect.com)

## Features

### Bitmap Generators
        var bitmap = colorFrame.ToBitmap();
        var bitmap = depthFrame.ToBitmap();
        var bitmap = infraredFrame.ToBitmap();

### Bitmap Capture
        bitmap.Save("Capture.png");

### Background Removal
        var bitmap = colorFrame.GreenScreen(depthFrame, bodyIndexFrame);

### Closest Body
        var body = bodyFrame.Bodies().Closest();

### Body Height
        double height = body.Height();

### Body Visualization
        viewer.DrawBody(body);

### Angles between joints

![Angles](http://i2.wp.com/vitruviuskinect.com/wp-content/uploads/2015/05/vitruvius-mathematics.jpg "Vitruvius Angles")

        double angle = elbow.Angle(shoulder, wrist);
        double angle = elbow.Angle(shoulder, wrist, Axis.Z);
        double radians = angle.ToRadians();
        double degrees = radians.ToDegrees();

### Automatic Coordinate Mapping
        var point = joint.Position.ToPoint(Visualization.Color);

### Gesture detection
        void GestureRecognized(object sender, GestureEventArgs e)
        {
           var gesture = e.GestureType;

           switch (gesture)
           {
        	   case (GestureType.JoinedHands): break;
        	   case (GestureType.Menu): break;
        	   case (GestureType.SwipeDown): break;
        	   case (GestureType.SwipeLeft): break;
        	   case (GestureType.SwipeRight): break;
        	   case (GestureType.SwipeUp): break;
        	   case (GestureType.WaveLeft): break;
        	   case (GestureType.WaveRight): break;
        	   case (GestureType.ZoomIn): break;
        	   case (GestureType.ZoomOut): break;
           }
        }

### XAML Controls
        KinectViewer		// Displays streams and skeletons.
        KinectAngle		// Displays an arc.
        KinectJointSelector	// Allows you to select a joint visually.

### Avateering ([Academic & Premium Versions](http://vitruviuskinect.com))

![Avateering](http://i2.wp.com/vitruviuskinect.com/wp-content/uploads/2015/10/vitruvius-kinect-body-models.jpg "Vitruvius Avateering")

        Avateering.Update(model, body);
        
### Recording & Playback ([Academic & Premium versions](http://vitruviuskinect.com))
        // Recording
        recorder = new VitruviusRecorder(path);
        recorder.RecordFrame(imageFrame, frameEffect, bodyFrame, faceFrame);
        
        // Playback
        player = new VitruviusPlayer(this, StreamFromPlayback, path));
        player.SeekFrame(seekDelta);

### HD Face with properties ([Academic & Premium Versions](http://vitruviuskinect.com))

![Face](http://vitruviuskinect.com/wp-content/uploads/2015/10/vitruvius-kinect-face-cover.svg "Vitruvius HD Face")

        Face face = faceFrame.Face();
        var nose = face.Nose;
        var mouth = face.Mouth;
        var chin = face.Chin;
        var jaw = face.Jaw;
        var eyeLeft = face.EyeLeft;
        var eyeRight = face.EyeRight;
        var cheekLeft = face.CheekLeft;
        var cheekRight = face.CheekRight;
        var forehead = face.Forehead;

## Contributors
* [Vangos Pterneas](http://pterneas.com) from [LightBuzz](http://lightbuzz.com) - Microsoft Kinect MVP
* [George Karakatsiotis](http://gkarak.com) from [LightBuzz](http://lightbuzz.com)
* Michael Miropoulos from [LightBuzz](http://lightbuzz.com)
* George Georgopoulos from [LightBuzz](http://lightbuzz.com)
* George Birbilis from [Zoomicon](http://zoomicon.com)

## License
You are free to use these libraries in personal and commercial projects by attributing the original creator of Vitruvius. Licensed under [Apache v2 License](https://github.com/LightBuzz/Vitruvius/blob/master/LICENSE).
