using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.leikelen.Util
{
    public static class MessageBoxAsync
    {
        private delegate void ShowMessageBoxDelegate(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage);
        // Method invoked on a separate thread that shows the message box.
        private static void ShowMessageBox(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage)
        {
            MessageBox.Show(strMessage, strCaption, enmButton, enmImage);
        }
        // Shows a message box from a separate worker thread.
        public static void ShowMessageBoxAsync(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage)
        {
            ShowMessageBoxDelegate caller = new ShowMessageBoxDelegate(ShowMessageBox);
            caller.BeginInvoke(strMessage, strCaption, enmButton, enmImage, null, null);
        }

        // Shows a message box from a separate worker thread. The specified asynchronous 
        // result object allows the caller to monitor whether the message box has been 
        // closed. This is useful for showing only one message box at a time. 
        public static void ShowMessageBoxAsync(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage, ref IAsyncResult asyncResult)
        {
            if ((asyncResult == null) || asyncResult.IsCompleted)
            {
                ShowMessageBoxDelegate caller = new ShowMessageBoxDelegate(ShowMessageBox);
                asyncResult = caller.BeginInvoke(strMessage, strCaption, enmButton, enmImage, null, null);
            }
        }
    }
}
