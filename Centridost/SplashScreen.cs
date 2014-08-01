using System.Threading;
using System.Windows.Forms;

namespace Centridost
{
    public partial class SplashScreen : Form
    {
        private static SplashScreen splashScreen = null;
        private static Thread worker = null;

        public SplashScreen()
        {
            InitializeComponent();
        }

        private static void ShowForm()
        {
            splashScreen = new SplashScreen();
            Application.Run(splashScreen);
        }

        private delegate void CloseCallback();
        public static void CloseForm()
        {
            if (splashScreen != null)
                if (!splashScreen.IsDisposed)
                    splashScreen.Invoke(new CloseCallback(splashScreen.Close));
        }

        public static void ShowSplashScreen()
        {
            if (splashScreen != null)
                return;

            worker = new Thread(new ThreadStart(SplashScreen.ShowForm));
            worker.IsBackground = true;
            worker.SetApartmentState(ApartmentState.STA);
            worker.Start();
        }
    }
}
