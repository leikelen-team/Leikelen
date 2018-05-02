using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Media;

namespace cl.uv.leikelen.Util
{
    /// <summary>
    /// Static class to test that inputs are integer
    /// </summary>
    public static class IntegerInput
    {
        /// <summary>
        /// Determines whether [is text a integer value] [the specified text].
        /// If not, plays a beep sound.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        ///   <c>true</c> if [is text allowed] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTextAllowed(string text)
        {
            bool result = int.TryParse(text, out int value);
            if(!result)
                SystemSounds.Beep.Play();
            return result;

        }

        /// <summary>
        /// Test if the new character typed is a integer or not.
        /// </summary>
        /// <remarks>Called when a new character is typed into the input box.</remarks>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        public static void PreviewTextInputHandler(Object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        
        /// <summary>
        /// Test if the pasted text is a integer or not.
        /// </summary>
        /// <remarks>Called when pasting to the input box</remarks>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataObjectPastingEventArgs"/> instance containing the event data.</param>
        public static void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
    }
}
