using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.leikelen.Util
{
    /// <summary>
    /// Static class to show a message box asynchronously
    /// </summary>
    public static class MessageBoxAsync
    {
        private delegate void ShowMessageBoxDelegate(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage);
        
        /// <summary>
        /// Method invoked on a separate thread that shows the message box.
        /// </summary>
        /// <param name="strMessage">The string message.</param>
        /// <param name="strCaption">The string caption.</param>
        /// <param name="enmButton">The type of message button.</param>
        /// <param name="enmImage">The message image.</param>
        private static void ShowMessageBox(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage)
        {
            MessageBox.Show(strMessage, strCaption, enmButton, enmImage);
        }
        
        /// <summary>
        /// Shows a message box from a separate worker thread.   
        /// </summary>
        /// <param name="strMessage">The string message.</param>
        /// <param name="strCaption">The string caption.</param>
        /// <param name="enmButton">The type of message button.</param>
        /// <param name="enmImage">The message image.</param>
        public static void ShowMessageBoxAsync(string strMessage, string strCaption, MessageBoxButton enmButton, MessageBoxImage enmImage)
        {
            ShowMessageBoxDelegate caller = new ShowMessageBoxDelegate(ShowMessageBox);
            caller.BeginInvoke(strMessage, strCaption, enmButton, enmImage, null, null);
        }
      
        /// <summary>
        /// Shows a message box from a separate worker thread. The specified asynchronous
        /// result object allows the caller to monitor whether the message box has been 
        /// closed. This is useful for showing only one message box at a time. 
        /// </summary>
        /// <param name="strMessage">The string message.</param>
        /// <param name="strCaption">The string caption.</param>
        /// <param name="enmButton">The type of message button.</param>
        /// <param name="enmImage">The message image.</param>
        /// <param name="asyncResult">The asynchronous result.</param>
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
