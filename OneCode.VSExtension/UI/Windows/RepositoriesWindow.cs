﻿using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using OneCode.VsExtension.UI.ViewModels;

namespace OneCode.VsExtension.UI.Windows
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("84CE1945-138B-4F6B-82BD-71CE4379B992")]
    public sealed class RepositoriesWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoriesWindow"/> class.
        /// </summary>
        public RepositoriesWindow() : base(null)
        {
            Caption = "OneCode Repositories";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            Content = new Controls.RepositoriesControl
            {
                DataContext = new RepositoriesViewModel(OneCodePackage.Repositories.GetValue()),
            };
        }

        public RepositoriesWindow(string _) : this()
        {
        }
    }
}
