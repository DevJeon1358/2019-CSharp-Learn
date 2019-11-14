using StarBucks.Network;
using System;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using System.Windows;
using ToastNotifications.Lifetime.Clear;

namespace StarBucks
{
    public class notify
    {
        static socket sock;
        private Notifier notifier;
        public notify()
        {
            sock = App.socketController;
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: App.main,
                    corner: Corner.BottomCenter,
                    offsetX: 0,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(2));

                cfg.DisplayOptions.Width = 300;

                cfg.Dispatcher = App.Current.Dispatcher;
            });
        }

        public notify(Window window, Corner corner)
        {
            sock = App.socketController;
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: window,
                    corner: corner,
                    offsetX: 0,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(2));

                cfg.DisplayOptions.Width = 300;
                cfg.Dispatcher = App.Current.Dispatcher;
            });
        }

        public void listenMessage()
        {
            sock.messageEvent += Sock_messageEvent;
            sock.lostEvent += Sock_lostEvent;
        }

        private void Sock_lostEvent(object sender, lostConnectionArgs e)
        {
            App.Current.Dispatcher.Invoke(new Action(delegate
            {
                notifier.Dispose();
            }));
        }

        private void Sock_messageEvent(object sender, messageArgs e)
        {
            App.Current.Dispatcher.Invoke(new Action(delegate
            {
                var socketInstance = sock.GetSocketInstance();
                if(socketInstance != null)
                {
                    if(socketInstance.lastSendtime != null)
                    {
                        TimeSpan timediff = DateTime.Now - socketInstance.lastSendtime;
                        if (timediff.TotalSeconds > 5)
                        {
                            notifier.ClearMessages(new ClearAll());
                            notifier.ShowInformation("본사에서 메시지가 도착했습니다.\n" + e.message);
                        }
                    }
                }
            }));
        }

        public void Send_Disconnected_Message()
        {
            App.Current.Dispatcher.Invoke(new Action(delegate
            {
                notifier.ClearMessages(new ClearAll());
                notifier.ShowError("본사 서버와의 연결이 종료되었습니다.\n연결을 다시 시도하고 있습니다.");
            }));
        }

        public void Send_Reconnected_Message()
        {
            App.Current.Dispatcher.Invoke(new Action(delegate
            {
                notifier.ClearMessages(new ClearAll());
                notifier.ShowSuccess("본사 서버와 다시 연결되었습니다.");
            }));
        }

        public void Send_Reconnecting_Message(int secound)
        {
            App.Current.Dispatcher.Invoke(new Action(delegate
            {
                notifier.ClearMessages(new ClearAll());
                notifier.ShowError("본사 서버와의 연결이 종료되었습니다.\n" + secound.ToString() + "초 후 다시 연결을 시도합니다.");
            }));
        }

        public void showError(string message)
        {
            App.Current.Dispatcher.Invoke(new Action(delegate
            {
                notifier.ClearMessages(new ClearAll());
                notifier.ShowError(message);
            }));
        }

        public void Dispose()
        {
            notifier.Dispose();
        }
    }
}
