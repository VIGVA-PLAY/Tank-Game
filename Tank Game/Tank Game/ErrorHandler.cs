using System.Windows;

namespace Tank_Game
{
    internal static class ErrorHandler
    {
        public static void ThrowError(string message)
        {
            MessageBox.Show(message);
            Application.Current.Shutdown();
        }
    }
}
