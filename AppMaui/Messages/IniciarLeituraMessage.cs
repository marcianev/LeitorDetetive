using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AppMaui.Messages
{
    public class IniciarLeituraMessage : ValueChangedMessage<bool>
    {
        public IniciarLeituraMessage() : base(true)
        {

        }
    }
}
    