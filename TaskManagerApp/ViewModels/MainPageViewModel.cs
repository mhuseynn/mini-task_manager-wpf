using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManagerApp.Commands;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels;

public class MainPageViewModel : NotificationService
{

    public ICommand startbtn { get; set; }

    public ICommand stopbtn { get; set; }

    private string textbox;

    public string Textbox
    {
        get { return textbox; }
        set { textbox = value; OnPropertyChanged(); }
    }

    private ObservableCollection<Process> processes;

    public ObservableCollection<Process> Processes
    {
        get { return processes; }
        set { processes = value; OnPropertyChanged(); }
    }



    private Process process1;

    public Process Process1
    {
        get { return process1; }
        set { process1 = value; }
    }


    public MainPageViewModel()
    {
        Process1 = new Process();
        Textbox = "";
        Processes = new ObservableCollection<Process>();
        startbtn = new RelayCommand(starttask!);
        stopbtn = new RelayCommand(endtask!);

        
    }

    public void starttask(object pa)
    {
        if (pa is Label label)
        {
            if (!string.IsNullOrEmpty(Textbox))
            {
                try
                {
                    label.Content = "";
                    Process1 = Process.Start(Textbox.ToString());                  
                    Processes.Add(Process1);
                    Process1 = new Process();
                    Textbox = "";
                }
                catch (Exception ex)
                {
                    label.Content = "Tapilmadi";
                    Textbox = "";
                    Process1 = new Process();
                }

            }
            else
            {
                label.Content = "Tapilmadi";
            }
        }
    }

    public void endtask(object pa)
    {
        if (pa is ListView box)
        {
            Process process2 = new Process();
            process2 = box.SelectedItem as Process;
            process2!.Kill();
            Processes.Remove(process2);

        }
    }


}
