using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface IMessageService
    {
        int GetMessageCount();
        void AddMessage(Message message);
        void UpdateMessage(Message message);
        IEnumerable<Message> GetMessages();
        Message GetMessageById(int id);
    }
}
