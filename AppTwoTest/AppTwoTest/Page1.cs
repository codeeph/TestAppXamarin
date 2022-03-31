using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Xamarin.Forms;

namespace AppTwoTest
{
    public class Page1 : ContentPage
    {

        string currentClient = "none";
        string currentProc = "none";
        Label textLabel;
        Picker pickerCons, pickerProcs;
        Entry portEntry, IPEntry,MesEntry;
        Button buttonSendMessage, buttonGetListConnects, buttonProcKill, buttonGetProcList, buttonReloadProcsList;
        public Page1()
        {
            RelativeLayout RelativeLayout = new RelativeLayout();
            portEntry = new Entry 
            { 
                Placeholder = "Port", 
                Text = "7892", 
            };
            
            
            IPEntry = new Entry
            { 
                Placeholder = "IP", 
                Text = "109.194.51.25",
            };

            MesEntry = new Entry
            {
                Placeholder = "Message",
                Text = "cons",
            };

            textLabel = new Label 
            { 
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) 
            };

            pickerCons = new Picker 
            { 
                Title = "Подключения" 
            };
            pickerProcs = new Picker
            {
                Title = "Процессы"
            };

            buttonSendMessage = new Button
            {
                Text = "Отправить!!!"
            };

            buttonGetListConnects = new Button 
            { 
                Text = "Тек.Подкл." 
            };

            buttonProcKill = new Button
            {
                Text = "УбитьПроц."
            };

            buttonGetProcList = new Button
            {
                Text = "Пол.Спис.Проц."
            };

            buttonReloadProcsList = new Button
            {
                Text = "Обн.Спис.Проц."
            };

            // Действия
            portEntry.TextChanged += loginEntry_TextChanged;
            buttonSendMessage.Clicked += ConnectBut_Clicked;
            pickerCons.SelectedIndexChanged += PickerCons_SelectedIndexChanged;
            pickerProcs.SelectedIndexChanged += PickerProcs_SelectedIndexChanged;
            buttonGetListConnects.Clicked += ButtonGetListConnects_Clicked;
            buttonProcKill.Clicked += ButtonProcKill_Clicked;
            buttonGetProcList.Clicked += ButtonGetProcList_Clicked;
            buttonReloadProcsList.Clicked += ButtonReloadProcsList_Clicked;

            // Текст бокс port
            RelativeLayout.Children.Add(portEntry, Constraint.RelativeToParent((parent) =>
            {
                return 0; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return 0;   // установка координаты Y
            }),
            Constraint.Constant(80)
            );

            // Текст бокс ip
            RelativeLayout.Children.Add(IPEntry, Constraint.RelativeToParent((parent) =>
            {
                return portEntry.Width + 1; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return 0; // Y
            }),
            Constraint.Constant(160)
            );

            // Текст бокс mes
            RelativeLayout.Children.Add(MesEntry, Constraint.RelativeToParent((parent) =>
            {
                return 0; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return portEntry.Height + 1; // Y
            }),
            Constraint.Constant(250) // ширина
            );

            // Кнопка подключения и отправки
            RelativeLayout.Children.Add(buttonSendMessage, Constraint.RelativeToParent((parent) =>
            {
                return MesEntry.Width + 1; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return portEntry.Height + 1; // Y
            })
            );

            // Листбокс текущих подключений
            RelativeLayout.Children.Add(pickerCons, Constraint.RelativeToParent((parent) =>
            {
                return 0; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return buttonSendMessage.Y + buttonSendMessage.Height + 1; // Y
            }),
            Constraint.Constant(380) // ширина
            );

            // Листбокс процессов клиента
            RelativeLayout.Children.Add(pickerProcs, Constraint.RelativeToParent((parent) =>
            {
                return 0; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return pickerCons.Y + pickerCons.Height + 1; // Y
            }),
            Constraint.Constant(380) // ширина
            );

            // Кнопка получения текущих подключений
            RelativeLayout.Children.Add(buttonGetListConnects, Constraint.RelativeToParent((parent) =>
            {
                return 0; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return pickerProcs.Y + pickerProcs.Height + 1; // Y
            })
            );

            // Кнопка закрытия процесса
            RelativeLayout.Children.Add(buttonProcKill, Constraint.RelativeToParent((parent) =>
            {
                return buttonGetListConnects.Width + 1; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return pickerProcs.Y + pickerProcs.Height + 1; // Y
            })
            );

            // Кнопка получения списка процессов
            RelativeLayout.Children.Add(buttonGetProcList, Constraint.RelativeToParent((parent) =>
            {
                return buttonProcKill.X + buttonProcKill.Width + 1; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return pickerProcs.Y + pickerProcs.Height + 1; // Y
            })
            );

            // Кнопка получения списка процессов
            RelativeLayout.Children.Add(buttonReloadProcsList, Constraint.RelativeToParent((parent) =>
            {
                return 0; // X
            }),
            Constraint.RelativeToParent((parent) =>
            {
                return buttonGetProcList.Y + buttonGetProcList.Height + 1; // Y
            })
            );

            this.Content = RelativeLayout;
        }

        private void ButtonReloadProcsList_Clicked(object sender, EventArgs e)
        {
            if (currentClient == "none")
            {
                DisplayAlert("Как так то!", "Вы не выбрали клиента!", "Окей");
                return;
            }
            MessageHandler("comsnd$proclist$" + currentClient);
        }

        private void ButtonGetProcList_Clicked(object sender, EventArgs e)
        {
            if (currentClient == "none")
            {
                DisplayAlert("Как так то!", "Вы не выбрали клиента!", "Окей");
                return;
            }
            MessageHandler("getprocs" + "$" +currentClient);
        }

        private void PickerProcs_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentIndex = pickerProcs.SelectedIndex;
            if (currentIndex != -1 & currentIndex != 0)
            {
                currentProc = pickerProcs.Items[currentIndex].Split('.')[1];
                DisplayAlert("Ну типа!", "Выбранный процесс - " + currentProc, "Окей");
            }
            else
            {
                currentProc = "none";
                DisplayAlert("Как так то!", "Выбранный процесс не соответсвует нормам!", "Окей");
            }
        }

        private void ButtonProcKill_Clicked(object sender, EventArgs e)
        {
            if(currentClient == "none")
            {
                DisplayAlert("Как так то!", "Вы не выбрали клиента!", "Окей");
                return;
            }
            if(currentProc == "none")
            {
                DisplayAlert("Как так то!", "Вы не выбрали процесс!", "Окей");
                return;
            }
            MessageHandler("comsnd$" + "prockill~" + currentProc + "$" + currentClient);
        }

        private void ButtonGetListConnects_Clicked(object sender, EventArgs e)
        {
            MessageHandler("cons");
        }

        private void PickerCons_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currentIndex = pickerCons.SelectedIndex;
            currentClient = pickerCons.Items[currentIndex];
            DisplayAlert("Ну типа!", "Выбранный клиент - " + currentClient, "Окей");
        }

        private void ConnectBut_Clicked(object sender, EventArgs e)
        {
            MessageHandler(MesEntry.Text);
        }

        private void MessageHandler(string sendMessage)
        {
            byte[] buffer = new byte[2048];
            IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IPEntry.Text), Convert.ToInt32(portEntry.Text));
            Socket send = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            send.Connect(iPEnd);
            send.Send(Encoding.Unicode.GetBytes(sendMessage));
            string data = string.Empty;
            if (sendMessage == "cons")
            {
                int size = send.Receive(buffer);
                data = Encoding.UTF8.GetString(buffer, 0, size);
                string[] cons = data.Split('\n');
                pickerCons.Items.Clear();
                for (int i = 0; i < cons.Length - 1; i++)
                {
                    if (cons[i] != "127.0.0.1;0")
                        pickerCons.Items.Add(cons[i].Substring(0, cons[i].Length - cons[i].Split(';')[2].Length - 1));
                }
            }

            if (sendMessage.Contains("getprocs"))
            {
                int size = send.Receive(buffer);
                data = Encoding.UTF8.GetString(buffer, 0, size);
                string[] procs = data.Split('\n');
                if(data.Contains("exception.FileNotFound"))
                {
                    DisplayAlert("Как так то!", "Файл не найден!", "Окей");
                }
                pickerProcs.Items.Clear();
                for (int i = 0; i < procs.Length - 1; i++)
                {
                    pickerProcs.Items.Add(procs[i]);
                }
            }

            send.Shutdown(SocketShutdown.Both);
            send.Close();
        }
        private void loginEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            textLabel.Text = portEntry.Text;
        }
    }
}