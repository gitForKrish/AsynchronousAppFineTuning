using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace _03_CancelAListOfTasks
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    CancellationTokenSource cts;
    public MainWindow()
    {
      InitializeComponent();
    }

    private async void BtnStart_Click(object sender, RoutedEventArgs e)
    {
      cts = new CancellationTokenSource();

      // Turning OFF Certificate validation 
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
      ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

      var urls = SetupUrls();
      txtResult.Clear();
      txtResult.Text += "Ready for Download...\n";
      await ProcessUrlsAsync(urls, cts.Token);
    }

    private async Task ProcessUrlsAsync(List<string> urls, CancellationToken token)
    {
      try
      {
        HttpClient client = new HttpClient();
        foreach (var url in urls)
        {
          HttpResponseMessage message = await client.GetAsync(url, token);
          byte[] contents = await message.Content.ReadAsByteArrayAsync();
          txtResult.Text += $"{url} \t {contents.Length}\n";
        }
        txtResult.Text += "Download completed ... \n";
      }
      catch (OperationCanceledException)
      {
        txtResult.Text += "Download canceled...\n";
      }
      catch(Exception)
      {
        txtResult.Text += "Download failed... \n";
      }
    }

    private List<string> SetupUrls() => new List<string>
    {
      "http://msdn.microsoft.com",
      "http://msdn.microsoft.com/library/windows/apps/br211380.aspx",
      "http://msdn.microsoft.com/en-us/library/hh290136.aspx",
      "http://msdn.microsoft.com/en-us/library/dd470362.aspx",
      "http://msdn.microsoft.com/en-us/library/aa578028.aspx",
      "http://msdn.microsoft.com/en-us/library/ms404677.aspx",
      "http://msdn.microsoft.com/en-us/library/ff730837.aspx"
    };

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (cts != null)
        cts.Cancel();
    }
  }
}
