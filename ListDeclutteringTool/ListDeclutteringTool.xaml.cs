using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace LDT {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		// VAR
		private List<String> output = new List<String>();

		public MainWindow() {
			InitializeComponent();

			// Executing example
			InputTextBox.Text = LDT.Properties.Resources.Example;
			StringForRemoval.Text = "(noun)";
			ItemSeparationComboBox.SelectedIndex = 1;
			Declutter(null, null);
		}

		/// <summary>
		/// Declutters String from InputTextBox and saves result to OutputTextBox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Declutter(object sender, RoutedEventArgs e) {

			// VAR
			String[] input;
			String stringForRemoval = StringForRemoval.Text;

			// Clearing List.output
			output.Clear();

			// Executing input.Split()
			switch (ItemSeparationComboBox.SelectedIndex) {
				case 0: // Separate by Spaces ' '

					input = InputTextBox.Text.Split(' ', '\n', '\r', (char)0x2028, (char)0x2029);
					break;

				case 1: // Separate by Tabs '\t'

					input = InputTextBox.Text.Split('\t', '\n', '\r', (char)0x2028, (char)0x2029);
					break;

				case 2: // Separate by Dots '.'

					input = InputTextBox.Text.Split('.', '\n', '\r', (char)0x2028, (char)0x2029);
					break;

				case 3: // Custom Separation

					String temp = InputTextBox.Text.Replace(CustomSeparationTextBox.Text, Environment.NewLine);
					input = temp.Split('\n', '\r', (char)0x2028, (char)0x2029);
					break;

				default:
					input = InputTextBox.Text.Split('\n', '\r', (char)0x2028, (char)0x2029);
					break;
			}

			// Storing Switch results into List.output
			foreach (String currentInput in input) {
				if (!(RemoveNumbers.IsChecked.Value && int.TryParse(currentInput, out int n))) {
					output.Add(currentInput);
				}
			}

			// Removing all occurrences of stringForRemoval from List.Output
			for (int i = 0; i < output.Count; i++) {
				output[i] = output[i].Replace(stringForRemoval, String.Empty)
									.Replace(Environment.NewLine, String.Empty);
			}

			// Removing Empty array elements from List.output and Clearing previous OutputTextBox
			output.RemoveAll(String.IsNullOrEmpty);
			OutputTextBox.Clear();

			// Displaying results in OutputTextBox
			foreach (String currentOutput in output) {
				OutputTextBox.AppendText(currentOutput + '\n');
			}
		}

		/// <summary>
		/// Saves List.output as a Text file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveTextFile_Click(object sender, RoutedEventArgs e) {

			// Opening new SaveFileDialog
			SaveFileDialog saveFileDialog = new SaveFileDialog {
				Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
				Title = "Save your Output:"
			};
			saveFileDialog.ShowDialog();

			if (saveFileDialog.FileName != String.Empty) { // Checking for empty names
				System.IO.FileStream fileStream = (System.IO.FileStream)saveFileDialog.OpenFile();
				TextWriter textWriter = new StreamWriter(fileStream);

				foreach (String current in output) {
					textWriter.WriteLine(current);
				}

				// Closing
				textWriter.Close();
				fileStream.Close();
			}
		}

		/// <summary>
		/// InputTextBox receives String[] from Text file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OpenTextFile_Click(object sender, RoutedEventArgs e) {

			OpenFileDialog openFileDialog = new OpenFileDialog {
				Filter = "txt files (*.txt)|*.txt",
				Title = "Open Text file for decluttering:"
			};
			openFileDialog.ShowDialog();

			if (openFileDialog.FileName != String.Empty) { // Checking for empty names
				System.IO.FileStream fileStream = (System.IO.FileStream)openFileDialog.OpenFile();

				TextReader textReader = new StreamReader(fileStream);
				InputTextBox.Clear();
				InputTextBox.AppendText(textReader.ReadToEnd());

				// Closing
				textReader.Close();
				fileStream.Close();
			}
		}

		private void InputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {

		}

		private void ClearButton_Click(object sender, RoutedEventArgs e) {
			InputTextBox.Text = String.Empty;
			OutputTextBox.Text = String.Empty;
		}

		private void ItemSeparationComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
			if (CustomSeparationStackPanel != null) {
				if (ItemSeparationComboBox.SelectedIndex == 3) {
					CustomSeparationStackPanel.Visibility = Visibility.Visible;

				}
				else {
					CustomSeparationStackPanel.Visibility = Visibility.Collapsed;
				}
			}
		}

		private void CopyButton_Click(object sender, RoutedEventArgs e) {
			Clipboard.SetText(OutputTextBox.Text);
		}

		private void PasteButton_Click(object sender, RoutedEventArgs e) {
			InputTextBox.Text = Clipboard.GetText();
		}
	}
}
