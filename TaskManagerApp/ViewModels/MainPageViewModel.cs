using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManagerApp.Commands;
using TaskManagerApp.Services;

namespace TaskManagerApp.ViewModels;

public class MainPageViewModel : NotificationService
{
    public ICommand addbtn { get; set; }

    private string combotext;

    public string ComboText
    {
        get { return combotext; }
        set { combotext = value; OnPropertyChanged(); }
    }

    private ObservableCollection<string> blackStrings;

    public ObservableCollection<string> BlackStrings
    {
        get { return blackStrings; }
        set { blackStrings = value; OnPropertyChanged(); }
    }

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

    private Process[] blackprocesses;

    public Process[] BlackProcesses
    {
        get { return blackprocesses; }
        set { blackprocesses = value; OnPropertyChanged(); }
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
        addbtn = new RelayCommand(addblack!);
        BlackStrings = new ObservableCollection<string>();
        ComboText = "";
     
        Thread thread = new Thread(() =>
        {
            while (true)
            {
                var blacklistedProcesses = Process.GetProcesses();
                var black = blacklistedProcesses.Where(p => BlackStrings.Contains(p.ProcessName));
                
                foreach (Process item in black)
                {
                    item.Kill();
                    Processes.Remove(item);
                }
                
                Thread.Sleep(1000);
            }

        });
        
        thread.IsBackground = true;
        thread.Start();

    }
    public void addblack(object pa)
    {
        BlackStrings.Add(ComboText);
        ComboText = "";
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
