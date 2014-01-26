using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common methods for voice recognition and speech synthesis.
    /// </summary>
    public class VoiceController
    {
        #region Constants

        /// <summary>
        /// The default voice recognition confidence indicator.
        /// </summary>
        readonly double DEFAULT_RECOGNITION_CONFIDENCE = 0.55;

        #endregion

        #region Members

        /// <summary>
        /// The voice recognition engine.
        /// </summary>
        SpeechRecognitionEngine _recognizer;

        /// <summary>
        /// The speech synthesizer engine.
        /// </summary>
        SpeechSynthesizer _synthesizer;

        /// <summary>
        /// The active Kinect sensor which handles the voice input and output.
        /// </summary>
        KinectSensor _sensor;

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the current voice engine is capable to recognize voice commands (e.g. has a proper input device).
        /// </summary>
        public bool IsRecognitionCapable { get; private set; }

        /// <summary>
        /// Indicates whether the current voice engine is capable to synthesize voice (e.g. has a proper output device).
        /// </summary>
        public bool IsSynthesisCapable { get; private set; }

        /// <summary>
        /// Gets or sets the desired voice recognition confidence.
        /// </summary>
        public double RecognitionConfidence { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when some voice commands have been recognized.
        /// </summary>
        public event EventHandler<SpeechRecognizedEventArgs> SpeechRecognized;

        /// <summary>
        /// Occurs when some voice commands have been hypothesized.
        /// </summary>
        public event EventHandler<SpeechHypothesizedEventArgs> SpeechHypothesized;

        /// <summary>
        /// Occurs when some voice commands have been detected.
        /// </summary>
        public event EventHandler<SpeechDetectedEventArgs> SpeechDetected;

        /// <summary>
        /// Occurs when some voice commands have been rejected.
        /// </summary>
        public event EventHandler<SpeechRecognitionRejectedEventArgs> SpeechRejected;

        /// <summary>
        /// Occurs after a specified text has been synthesized.
        /// </summary>
        public event EventHandler<SpeakCompletedEventArgs> SpeechSynthesized;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="VoiceController"/>.
        /// </summary>
        public VoiceController()
        {
            RecognitionConfidence = DEFAULT_RECOGNITION_CONFIDENCE;

            InitializeSynthesizer();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the speech synthesizer.
        /// </summary>
        private void InitializeSynthesizer()
        {
            _synthesizer = new SpeechSynthesizer();

            try
            {
                _synthesizer.SetOutputToDefaultAudioDevice();
                _synthesizer.SpeakCompleted += Synthesizer_SpeakCompleted;

                IsSynthesisCapable = true;
            }
            catch
            {
                Debug.WriteLine("No audio output device found.");
            }
        }

        /// <summary>
        /// Returns the Kinect recognition info.
        /// </summary>
        /// <returns>The <see cref="RecognizerInfo"/>.</returns>
        private RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) &&
                    "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };

            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }

        /// <summary>
        /// Creates a voice recognition grammar according to the specified voice commands.
        /// </summary>
        /// <param name="phrases">The voice commands to recognize.</param>
        /// <returns>The grammar of the phrases.</returns>
        private Grammar CreateGrammar(string[] phrases)
        {
            Choices choices = new Choices(phrases);

            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(choices);

            return new Grammar(grammarBuilder);
        }

        #endregion

        #region Event handlers

        void Synthesizer_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (SpeechSynthesized != null)
            {
                SpeechSynthesized(this, e);
            }
        }

        void Recognizer_SpeechRecognized(object sender, Microsoft.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > RecognitionConfidence)
            {
                if (SpeechRecognized != null)
                {
                    SpeechRecognized(this, e);
                }
            }
        }

        void Recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            if (SpeechHypothesized != null)
            {
                SpeechHypothesized(this, e);
            }
        }

        void Recognizer_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            if (SpeechDetected != null)
            {
                SpeechDetected(this, e);
            }
        }

        void Recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            if (SpeechRejected != null)
            {
                SpeechRejected(this, e);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts recognizing the specified voice commands.
        /// </summary>
        /// <param name="sensor">The Kinect sensor used to recognize voice.</param>
        /// <param name="phrases">The voice commands the engine should try to recognize.</param>
        public void StartRecognition(KinectSensor sensor, IEnumerable<string> phrases)
        {
            _sensor = sensor;

            if (_sensor != null)
            {
                RecognizerInfo info = GetKinectRecognizer();

                if (info != null)
                {
                    _recognizer = new SpeechRecognitionEngine(info.Id);
                    _recognizer.LoadGrammar(CreateGrammar(phrases.ToArray()));

                    _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                    _recognizer.SpeechHypothesized += Recognizer_SpeechHypothesized;
                    _recognizer.SpeechRecognitionRejected += Recognizer_SpeechRecognitionRejected;
                    _recognizer.SpeechDetected += Recognizer_SpeechDetected;

                    IsRecognitionCapable = true;
                }
            }

            if (!IsRecognitionCapable)
            {
                throw new Exception("Speech recognition is not supported.");
            }

            var thread = new Thread(() =>
            {
                var audioSource = _sensor.AudioSource;

                audioSource.BeamAngleMode = BeamAngleMode.Adaptive;
                audioSource.EchoCancellationMode = EchoCancellationMode.CancellationAndSuppression;

                var stream = audioSource.Start();

                _recognizer.SetInputToAudioStream(stream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            });

            thread.Start();
        }

        /// <summary>
        /// Stops recognizing voice commands.
        /// </summary>
        public void StopRecognition()
        {
            if (!IsRecognitionCapable)
            {
                throw new Exception("Speech recognition is not supported.");
            }

            if (_sensor != null && _recognizer != null)
            {
                _sensor.AudioSource.Stop();

                _recognizer.RecognizeAsyncCancel();
                _recognizer.RecognizeAsyncStop();

                IsRecognitionCapable = false;
            }
        }

        /// <summary>
        /// Speaks out the specified text.
        /// </summary>
        /// <param name="text">The text to speak.</param>
        public void Speak(string text)
        {
            if (!IsSynthesisCapable)
            {
                throw new Exception("Speech synthesis is not supported. Verify that you have a valid audio device connected.");
            }

            _synthesizer.SpeakAsync(text);
        }

        #endregion
    }
}
