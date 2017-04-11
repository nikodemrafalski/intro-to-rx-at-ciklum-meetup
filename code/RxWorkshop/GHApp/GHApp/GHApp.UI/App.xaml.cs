using System.Configuration;
using System.Windows;
using GHApp.Communication;
using GHApp.Contracts.Queries;
using Microsoft.Practices.Unity;

namespace GHApp.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = new UnityContainer();

            container.RegisterType<IUdpClientServer, UdpClientServer>(new ContainerControlledLifetimeManager());

            var serverAddress = ConfigurationManager.AppSettings.Get("ServerAddress");
            var serverPort = int.Parse(ConfigurationManager.AppSettings.Get("ServerPort"));
            container.RegisterInstance<IChannelConfig>(ChannelNames.Server,
                new ChannelConfig {Address = serverAddress, Port = serverPort},
                new ContainerControlledLifetimeManager());
            container.RegisterType<ICommunicationChannel, UdpCommunicationChannel>(
                ChannelNames.Server,
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => new UdpCommunicationChannel(c.Resolve<IUdpClientServer>(),
                    c.Resolve<IChannelConfig>(ChannelNames.Server))));

            var clientAddress = ConfigurationManager.AppSettings.Get("ClientAddress");
            var clientPort = int.Parse(ConfigurationManager.AppSettings.Get("ClientPort"));
            container.RegisterInstance<IChannelConfig>(ChannelNames.Client,
                new ChannelConfig {Address = clientAddress, Port = clientPort},
                new ContainerControlledLifetimeManager());
            container.RegisterType<ICommunicationChannel, UdpCommunicationChannel>(
                ChannelNames.Client,
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => new UdpCommunicationChannel(c.Resolve<IUdpClientServer>(),
                    c.Resolve<IChannelConfig>(ChannelNames.Client))));

            container.RegisterType(typeof(IService<,>), typeof(Service<,>), new ContainerControlledLifetimeManager());
            container.RegisterType(typeof(ITopic<>), typeof(Topic<>), new ContainerControlledLifetimeManager());

            var ch = container.Resolve<ICommunicationChannel>(ChannelNames.Client);
            ch.SendMessage(new UserQuery("xxx"));

            container.RegisterType<MainWindowViewModel, MainWindowViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<MainWindow>(new ContainerControlledLifetimeManager());

            container.Resolve<MainWindow>().Show();
        }
    }
}