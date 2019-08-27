using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _01_InitialProject
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private async void BtnStart_Click(object sender, RoutedEventArgs e)
    {
      txtResult.Clear();
      txtResult.Text += "Ready for Download.\n";

      // Get the URL List
      var Urls = SetupURLs();

      // Process each URLs
      await ProcessURLsAsync(Urls);
    }

    private async Task ProcessURLsAsync(List<string> urls)
    {
      try
      {
        HttpClient client = new HttpClient();
        foreach (var url in urls)
        {
          HttpResponseMessage response = await client.GetAsync(url);
          byte[] content = await response.Content.ReadAsByteArrayAsync();
          txtResult.Text += $"{url} \t {content.Length}\n";
          await Task.Delay(250);
        }
        txtResult.Text += "Download completed.\n";
      }
      catch (Exception)
      {
        txtResult.Text += "Download failed. \n";
      }
    }

    private List<string> SetupURLs() => new List<string>
    {
      "http://msdn.microsoft.com",
      "http://msdn.microsoft.com/library/windows/apps/br211380.aspx",
      "http://msdn.microsoft.com/en-us/library/hh290136.aspx",
      "http://msdn.microsoft.com/en-us/library/dd470362.aspx",
      "http://msdn.microsoft.com/en-us/library/aa578028.aspx",
      "http://msdn.microsoft.com/en-us/library/ms404677.aspx",
      "http://msdn.microsoft.com/en-us/library/ff730837.aspx"
    };
  }
}
