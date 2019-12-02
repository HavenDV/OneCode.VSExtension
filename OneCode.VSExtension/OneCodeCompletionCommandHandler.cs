﻿using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace OneCode.VsExtension
{
    [Export(typeof(IVsTextViewCreationListener))]
    [Name("OneCode Token Completion Handler")]
    [ContentType("plaintext")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal class OneCodeCompletionHandlerProvider : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService;
        [Import]
        internal ICompletionBroker CompletionBroker { get; set; }
        [Import]
        internal SVsServiceProvider ServiceProvider { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            if (!(AdapterService.GetWpfTextView(textViewAdapter) is ITextView textView))
            {
                return;
            }

            textView.Properties.GetOrCreateSingletonProperty(
                () => new OneCodeCompletionCommandHandler(textViewAdapter, textView, this));
        }
    }
}
