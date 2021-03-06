﻿using System;
using System.Globalization;
using System.Windows.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Ref12.Tests {
	static class Extensions {
		public static SnapshotSpan FindSpan(this ITextView textView, string search) {
			var startIndex = textView.TextBuffer.CurrentSnapshot.GetText().IndexOf(search);
			if (startIndex < 0)
				throw new ArgumentException("Cannot find string " + search);
			if (startIndex != textView.TextBuffer.CurrentSnapshot.GetText().LastIndexOf(search))
				throw new ArgumentException("String " + search + " occurs multiple times.  Please use a unique string");
			return new SnapshotSpan(textView.TextBuffer.CurrentSnapshot, startIndex, search.Length);
		}

		public static void Execute(this IVsTextView textView, Enum commandId, IntPtr inHandle = default(IntPtr), IntPtr outHandle = default(IntPtr)) {
			Execute((IOleCommandTarget)textView, commandId, inHandle, outHandle);
		}
		public static void Execute(this IOleCommandTarget target, Enum commandId, IntPtr inHandle = default(IntPtr), IntPtr outHandle = default(IntPtr)) {
			var c = commandId.GetType().GUID;
			ErrorHandler.ThrowOnFailure(target.Exec(ref c, Convert.ToUInt32(commandId, CultureInfo.InvariantCulture), 0, inHandle, outHandle));
		}


		///<summary>Returns an awaitable object that resumes execution on the specified Dispatcher.</summary>
		/// <remarks>Copied from <see cref="Dispatcher.Yield"/>, which can only be called on the Dispatcher thread.</remarks>
		public static DispatcherPriorityAwaitable NextFrame(this Dispatcher dispatcher, DispatcherPriority priority = DispatcherPriority.Background) {
			return new DispatcherPriorityAwaitable(dispatcher, priority);
		}
	}
}
