﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConscriptDesigner.Anchorables {
	public class OutputConsole : RequestCloseAnchorable {

		//-----------------------------------------------------------------------------
		// Classes
		//-----------------------------------------------------------------------------

		/// <summary>A text writer to hijack the console output.</summary>
		private class OutputTextWriter : TextWriter {
			/// <summary>The output console to write to.</summary>
			private OutputConsole console;

			/// <summary>Constructs the console text writer.</summary>
			public OutputTextWriter(OutputConsole console) {
				this.console = console;
			}

			/// <summary>Gets the encoding.</summary>
			public override Encoding Encoding {
				get { return Encoding.UTF8; }
			}

			/// <summary>Write text to the console.</summary>
			public override void Write(string value) {
				console.Write(value);
			}

			/// <summary>Write a character to the console.</summary>
			public override void Write(char value) {
				console.Write(new string(value, 1));
			}
		}


		//-----------------------------------------------------------------------------
		// Constants
		//-----------------------------------------------------------------------------

		/// <summary>The maximum number of lines allowed in the console.</summary>
		private const int MaxLines = 400;


		//-----------------------------------------------------------------------------
		// Members
		//-----------------------------------------------------------------------------

		/// <summary>The scroll viewer for displaying the console log.</summary>
		private ScrollViewer scrollViewer;
		/// <summary>The stack panel for each line of text.</summary>
		private VirtualizingStackPanel stackPanel;


		//-----------------------------------------------------------------------------
		// Constructor
		//-----------------------------------------------------------------------------

		/// <summary>Constructs the output console.</summary>
		public OutputConsole() {
			Border border = CreateBorder();
			this.scrollViewer = new ScrollViewer();
			this.scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
			this.scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			this.scrollViewer.Background = new SolidColorBrush(Color.FromRgb(230, 231, 232));
			this.scrollViewer.FontFamily = new FontFamily("Lucida Console");
			this.scrollViewer.FontSize = 11;
			this.scrollViewer.CanContentScroll = true;
			TextOptions.SetTextFormattingMode(this.scrollViewer, TextFormattingMode.Display);
			border.Child = this.scrollViewer;

			this.stackPanel = new VirtualizingStackPanel();
			this.scrollViewer.Content = this.stackPanel;


			Closed += OnAnchorableClosed;
			AppendLine("");
			Console.SetOut(new OutputTextWriter(this));
			
			Title = "Output Console";
			Content = border;
		}


		//-----------------------------------------------------------------------------
		// Event Handlers
		//-----------------------------------------------------------------------------

		/// <summary>Called when closing to re-establish the proper console output channel.</summary>
		private void OnAnchorableClosed(object sender, EventArgs e) {
			// Return console output to its rightful owner
			var standardOutput = new StreamWriter(Console.OpenStandardOutput());
			standardOutput.AutoFlush = true;
			Console.SetOut(standardOutput);
		}


		//-----------------------------------------------------------------------------
		// Console Output
		//-----------------------------------------------------------------------------

		/// <summary>Clears the console.</summary>
		public void Clear() {
			Dispatcher.Invoke(() => {
				stackPanel.Children.Clear();
				AppendLine("");
			});
		}

		/// <summary>Creates a new line if the current line isn't empty.</summary>
		public void NewLine() {
			TextBlock textBlock = (TextBlock) stackPanel.Children[stackPanel.Children.Count - 1];
			if (textBlock.Text != "")
				AppendLine("");
		}

		/// <summary>Write a line to the console.</summary>
		public void WriteLine(string line) {
			Write(line + "\n");
		}

		/// <summary>Write text to the console.</summary>
		public void Write(string text) {
			Dispatcher.Invoke(() => {
				string[] lines = text.Replace("\r", "").Split('\n');
				AppendText(lines[0]);
				for (int i = 1; i < lines.Length; i++) {
					AppendLine(lines[i]);
				}
			});
		}


		//-----------------------------------------------------------------------------
		// Internal Methods
		//-----------------------------------------------------------------------------

		/// <summary>Appends text to the last line in the stack panel.</summary>
		private void AppendText(string text) {
			TextBlock textBlock = (TextBlock) stackPanel.Children[stackPanel.Children.Count - 1];
			textBlock.Text += text;
		}

		/// <summary>Appends a line to the stack panel.</summary>
		private void AppendLine(string text) {
			TextBlock textBlock = new TextBlock();
			textBlock.Padding = new Thickness(10, 1, 0, 0);
			textBlock.Text = text;
			stackPanel.Children.Add(textBlock);
			while (stackPanel.Children.Count > MaxLines) {
				stackPanel.Children.RemoveAt(0);
			}
			scrollViewer.ScrollToBottom();
		}
	}
}