using System;
using System.Collections.Generic;
using System.Linq;
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

namespace _05_CancelRemainingTasks
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
      txtResult.Clear();
      txtResult.Text += "Ready for download ... \n";
      try
      {
        await AccessWebAsync(cts.Token);
        txtResult.Text += "Download completed ... \n";
      }
      catch (OperationCanceledException)
      {
        txtResult.Text += "Download canceled ... \n";
      }
      catch (Exception)
      {
        txtResult.Text += "Download failed ... \n";
      }
      cts = null;
    }

    private async Task AccessWebAsync(CancellationToken token)
    {
      var urls = GetUrls();
      HttpClient client = new HttpClient();

      IEnumerable<Task<int>> downloadTaskQuery = urls.Select(url => ProcessUrlAsync(client, url, token));
      Task<int>[] downloadTasks = downloadTaskQuery.ToArray();
      Task<int> FirstFinishedTask = await Task.WhenAny(downloadTasks);
      cts.Cancel();
      int length = await FirstFinishedTask;
      txtResult.Text += $"Length downloaded = {length}\n";
    }

    private async Task<int> ProcessUrlAsync(HttpClient client, string url, CancellationToken token)
    {
      HttpResponseMessage responseMessage = await client.GetAsync(url, token);
      byte[] contents = await responseMessage.Content.ReadAsByteArrayAsync();
      return contents.Length;
    }

    private List<string> GetUrls() => new List<string>
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
