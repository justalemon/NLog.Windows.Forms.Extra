﻿using NLog;
using NLog.Targets;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Lemon.NLog.WinForms
{
    /// <summary>
    /// Logs text into a Text Box control in an existing form.
    /// </summary>
    [Target("TextBox")]
    public class TextBoxTarget : TargetWithContext
    {
        /// <summary>
        /// The TextBox control that will be used for this
        /// </summary>
        public TextBox TargetTextBox { get; private set; }
        /// <summary>
        /// If a new line should be added at the end of the text.
        /// </summary>
        [DefaultValue(true)]
        public bool AddNewLine { get; set; }

        public TextBoxTarget(TextBox textBox) : base()
        {
            TargetTextBox = textBox;
        }

        protected override void Write(LogEventInfo LogEvent)
        {
            // Start by formatting the text that we need
            string text = Layout.Render(LogEvent);
            // If we need to add a new line, do it
            if (AddNewLine)
            {
                text += Environment.NewLine;
            }

            // If a handle has not been created for this TextBox
            if (!TargetTextBox.IsHandleCreated)
            {
                // Get the pointer/handle of the TextBox (is silently created if not)
                // And only call Invoke when we are not in the UI Thread (speed!)
                if (TargetTextBox.InvokeRequired)
                {
                    TargetTextBox.Invoke(new Action(() => { IntPtr pointer = TargetTextBox.Handle; }));
                }
                else
                {
                    IntPtr pointer = TargetTextBox.Handle;
                }
            }

            // Then, just go ahead and append the message
            TargetTextBox.Invoke(new Action(() => TargetTextBox.AppendText(text)));
        }
    }
}
