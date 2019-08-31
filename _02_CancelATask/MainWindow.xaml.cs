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

namespace _02_CancelATask
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
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
      ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
      cts = new CancellationTokenSource();
      txtResult.Clear();
      txtResult.Text += "Ready to download";
      
      var url = "http://msdn.microsoft.com/library/windows/apps/br211380.aspx";
      await ProcessURLAsync(url, cts.Token);

    }

    private async Task ProcessURLAsync(string url, CancellationToken token)
    {
      try
      {
        HttpClient client = new HttpClient();
        await Task.Delay(5000);
        Task<HttpResponseMessage> response = client.GetAsync(url, token);
        HttpResponseMessage message = await response;
        byte[] contents = await message.Content.ReadAsByteArrayAsync();
        txtResult.Text += $"\n{url} \t {contents.Length}";
        txtResult.Text += "\nDownload is completed.";
      }
      catch (OperationCanceledException)
      {
        txtResult.Text += "\nDownload is canceled";
      }
      catch (Exception)
      {
        txtResult.Text += "\nDownload is failed";
      }
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (cts != null)
        cts.Cancel();
    }
  }
}
