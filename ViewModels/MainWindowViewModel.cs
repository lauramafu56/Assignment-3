namespace Assignment3.ViewModels;

using Assignment3.Views;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class MainWindowViewModel : ViewModelBase
{
    private FirstView firstView {get;} = new FirstView() { DataContext= new FirstViewModel() };
    private SecondView secondView {get;} = new SecondView() { DataContext= new SecondViewModel() };
    private ThirdView thirdView {get;} = new ThirdView() { DataContext= new ThirdViewModel() };

    [ObservableProperty]
    private UserControl _currentView;

    public MainWindowViewModel()
    {
        CurrentView = firstView;
    }

    public void NextView()
    {
        if (CurrentView == firstView)
        {
            CurrentView = secondView;
        }
        else if (CurrentView == secondView)
        {
            CurrentView = thirdView;
        }
        else
        {
            CurrentView = firstView;
        }
 
    }
}
