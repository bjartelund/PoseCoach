using AForge.Video;
using AForge.Video.DirectShow;
using Timer = System.Windows.Forms.Timer;

namespace PoseCoachApp;

public partial class Form1 : Form
{
    private VideoCaptureDevice _videoSource;
    private Timer _captureTimer; // Timer for timed captures
    private readonly ICoach _coach;

    public Form1(ICoach coach)
    {
        _coach = coach;
        InitializeComponent();
        InitializeTimer();
    }


    private void InitializeTimer()
    {
        // Initialize the timer
        _captureTimer = new Timer
        {
            Interval = 120000 // 60,000 ms = 1 minute
        };
        _captureTimer.Start();
        _captureTimer.Tick += CaptureTimer_Tick;
        CaptureFrame();
    }


    private void CaptureTimer_Tick(object? sender, EventArgs e)
    {
        CaptureFrame();
    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {
        // Get the list of video devices (webcams)
        CaptureFrame();
    }

    private void CaptureFrame()
    {
        var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        if (videoDevices.Count == 0)
        {
            MessageBox.Show(@"No video devices found!");
            return;
        }

        // Select the first available webcam
        _videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
        _videoSource.VideoResolution = _videoSource.VideoCapabilities
            .FirstOrDefault(res => res.FrameSize is { Width: 640, Height: 480 });


        // Set the NewFrame event to handle each frame
        _videoSource.NewFrame += VideoSource_NewFrame;

        // Start the webcam feed
        _videoSource.Start();
        
        if(pictureBox1.Image == null) return;
    }


    private async void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
        // Get the current frame and display it in the PictureBox
        var bitmap = (Bitmap)eventArgs.Frame.Clone();
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

        pictureBox1.Image = bitmap;
        
        textBox1.Text = await _coach.GetPoseEvaluation((Bitmap)pictureBox1.Image);


        _videoSource.NewFrame -= VideoSource_NewFrame;

        // Stop the webcam feed
        _videoSource.SignalToStop();
        _videoSource.WaitForStop();
    }
}