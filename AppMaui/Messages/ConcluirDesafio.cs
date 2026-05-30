using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AppMaui.Messages
{
    public class ConcluirDesafio: ValueChangedMessage<bool>
    {
        public ConcluirDesafio() : base(true) 
        {
        }
    }
}
