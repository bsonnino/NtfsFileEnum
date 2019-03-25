using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Filesystem.Ntfs;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NtfsFileEnum
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CollectionView _view;

        public MainWindow()
        {
            InitializeComponent();
            var ntfsDrives = DriveInfo.GetDrives()
                .Where(d => d.DriveFormat == "NTFS").ToList();
            DrvCombo.ItemsSource = ntfsDrives;
            DrvCombo.SelectionChanged += DrvCombo_SelectionChanged;
        }

        private async void DrvCombo_SelectionChanged(object sender, 
            System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DrvCombo.SelectedItem != null)
            {
                var driveToAnalyze = (DriveInfo) DrvCombo.SelectedItem;
                DrvCombo.IsEnabled = false;
                StatusTxt.Text = "Analyzing drive";
                List<INode> nodes = null;
                await Task.Factory.StartNew(() =>
                {
                    var ntfsReader =
                        new NtfsReader(driveToAnalyze, RetrieveMode.All);
                    nodes =
                        ntfsReader.GetNodes(driveToAnalyze.Name)
                            .Where(n => (n.Attributes &
                                         (Attributes.Hidden | Attributes.System |
                                          Attributes.Temporary | Attributes.Device |
                                          Attributes.Directory | Attributes.Offline |
                                          Attributes.ReparsePoint | Attributes.SparseFile)) == 0)
                            .OrderByDescending(n => n.Size).ToList();
                });
                FilesList.ItemsSource = nodes;
                _view = (CollectionView)CollectionViewSource.GetDefaultView(FilesList.ItemsSource);

                DrvCombo.IsEnabled = true;
                StatusTxt.Text = $"{nodes.Count} files listed. " +
                                 $"Total size: {nodes.Sum(n => (double)n.Size):N0}";
            }
            else
            {
                _view = null;
            }
        }

        private void SortCombo_OnSelectionChanged(object sender, 
            SelectionChangedEventArgs e)
        {
            if (_view == null)
                return;
            _view.GroupDescriptions.Clear();
            _view.SortDescriptions.Clear();
            switch (SortCombo.SelectedIndex)
            {
                case 1:
                    _view.GroupDescriptions.Add(new PropertyGroupDescription("FullName", 
                        new FileExtConverter()));
                    break;
                case 2:
                    _view.SortDescriptions.Add(new SortDescription("FullName",
                        ListSortDirection.Ascending));
                    _view.GroupDescriptions.Add(new PropertyGroupDescription("FullName", 
                        new FilePathConverter()));
                    break;
            }
        }
    }
}
